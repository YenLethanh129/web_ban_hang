using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class StockCalculationService
    {
        private readonly WebbanhangDbContext _context;

        public StockCalculationService(WebbanhangDbContext context)
        {
            _context = context;
        }

        public async Task CalculateAndUpdateReorderPointsAsync()
        {
            Console.WriteLine("Bắt đầu tính toán điểm đặt hàng lại...");

            var thresholds = await _context.InventoryThresholds
                .Include(t => t.Ingredient)
                .Include(t => t.Branch)
                .Where(t => t.IsActive)
                .ToListAsync();

            foreach (var threshold in thresholds)
            {
                try
                {
                    var avgDailyConsumption = await CalculateAverageDailyConsumptionAsync(
                        threshold.BranchId, threshold.IngredientId);

                    var reorderPoint = await CalculateReorderPointAsync(
                        avgDailyConsumption, threshold.LeadTimeDays, threshold.MinimumThreshold);

                    threshold.AverageDailyConsumption = avgDailyConsumption;
                    threshold.ReorderPoint = reorderPoint;
                    threshold.LastCalculatedDate = DateTime.UtcNow;
                    threshold.LastModified = DateTime.UtcNow;

                    Console.WriteLine($"Cập nhật ROP cho {threshold.Ingredient.Name}: {reorderPoint:F2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured while calculating {threshold.IngredientId}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateAverageDailyConsumptionAsync(long branchId, long ingredientId)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var orderConsumption = await _context.Orders
                .Where(o => o.BranchId == branchId && o.CreatedAt >= thirtyDaysAgo)
                .SelectMany(o => o.OrderDetails)
                .SelectMany(od => od.Product.ProductRecipes)
                .Where(pr => pr.IngredientId == ingredientId)
                .SumAsync(pr => pr.Quantity * 1); 

            var movementConsumption = await _context.InventoryMovements
                .Where(im => im.BranchId == branchId 
                           && im.IngredientId == ingredientId 
                           && im.MovementType == "OUT"
                           && im.CreatedAt >= thirtyDaysAgo)
                .SumAsync(im => im.Quantity);

            var totalConsumption = orderConsumption + movementConsumption;
            
            return Math.Max(totalConsumption / 30m, 0.1m);
        }

        public Task<decimal> CalculateReorderPointAsync(decimal avgDailyConsumption, int leadTimeDays, decimal safetyStock)
        {
            var rop = (avgDailyConsumption * leadTimeDays) + safetyStock;
            return Task.FromResult(Math.Max(rop, 1m)); 
        }

        public async Task<List<InventoryThreshold>> GetLowStockAlertsAsync()
        {
            return await _context.InventoryThresholds
                .Include(t => t.Ingredient)
                .Include(t => t.Branch)
                .Where(t => t.IsActive)
                .Join(_context.BranchIngredientInventories,
                    threshold => new { threshold.BranchId, threshold.IngredientId },
                    inventory => new { inventory.BranchId, inventory.IngredientId },
                    (threshold, inventory) => new { threshold, inventory })
                .Where(x => x.inventory.Quantity <= x.threshold.ReorderPoint)
                .Select(x => x.threshold)
                .ToListAsync();
        }

        public async Task<List<InventoryThreshold>> GetOutOfStockAlertsAsync()
        {
            return await _context.InventoryThresholds
                .Include(t => t.Ingredient)
                .Include(t => t.Branch)
                .Where(t => t.IsActive)
                .Join(_context.BranchIngredientInventories,
                    threshold => new { threshold.BranchId, threshold.IngredientId },
                    inventory => new { inventory.BranchId, inventory.IngredientId },
                    (threshold, inventory) => new { threshold, inventory })
                .Where(x => x.inventory.Quantity <= x.threshold.MinimumStock)
                .Select(x => x.threshold)
                .ToListAsync();
        }

        public async Task UpdateInventoryThresholdAsync(long thresholdId, decimal newReorderPoint, decimal newSafetyStock)
        {
            var threshold = await _context.InventoryThresholds.FindAsync(thresholdId);
            if (threshold != null)
            {
                threshold.ReorderPoint = newReorderPoint;
                threshold.MinimumThreshold = newSafetyStock;
                threshold.LastModified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
