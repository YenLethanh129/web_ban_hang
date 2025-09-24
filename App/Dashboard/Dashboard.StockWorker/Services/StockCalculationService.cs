using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.StockWorker.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class StockCalculationService
    {
        private readonly WebbanhangDbContext _context;
        private const int DEFAULT_LEAD_TIME_DAYS = 7;

        public StockCalculationService(WebbanhangDbContext context)
        {
            _context = context;
        }

        public async Task CalculateAndUpdateReorderPointsAsync()
        {
            Console.WriteLine("Bắt đầu tính toán điểm đặt hàng lại...");
            var thresholds = await _context.InventoryThresholds
                .Select(t => new
                {
                    t.Id,
                    t.IngredientId,
                    t.BranchId,
                    t.ReorderPoint,
                    t.SafetyStock
                })
                .ToListAsync();

            foreach (var t in thresholds)
            {
                var avgDailyConsumption = await CalculateAverageDailyConsumptionAsync(
                    t.BranchId, t.IngredientId);

                var leadTimeDays = DEFAULT_LEAD_TIME_DAYS;

                var reorderPoint = await CalculateReorderPointAsync(
                    avgDailyConsumption, leadTimeDays, t.SafetyStock);
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE [dbo].[inventory_thresholds] SET reorder_point = {0}, safety_stock = {1}, last_modified = {2} WHERE id = {3}",
                    reorderPoint, t.SafetyStock, DateTime.UtcNow, t.Id);

                var ingredientName = await _context.Ingredients
                    .Where(i => i.Id == t.IngredientId)
                    .Select(i => i.Name)
                    .FirstOrDefaultAsync();

                Console.WriteLine($"Cập nhật ROP cho {ingredientName}: {reorderPoint:F2}");
            }
        }

        public async Task<decimal> CalculateAverageDailyConsumptionAsync(long branchId, long ingredientId)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var movementConsumption = await _context.InventoryMovements
                .FromSqlRaw(@"
                    SELECT
                        id,
                        branch_id AS BranchId,
                        ingredient_id AS IngredientId,
                        movement_type AS MovementType,
                        quantity AS Quantity,
                        unit AS Unit,
                        quantity_before AS QuantityBefore,
                        quantity_after AS QuantityAfter,
                        reference_type AS ReferenceType,
                        reference_id AS ReferenceId,
                        reference_code AS ReferenceCode,
                        notes AS Notes,
                        employee_id AS EmployeeId,
                        movement_date AS MovementDate,
                        created_at AS CreatedAt,
                        last_modified AS LastModified
                    FROM inventory_movements
                    WHERE branch_id = {0} AND ingredient_id = {1} AND movement_type = 'OUT' AND created_at >= {2}",
                    branchId, ingredientId, thirtyDaysAgo)
                .SumAsync(im => im.Quantity);

            var totalConsumption = movementConsumption;

            return Math.Max(totalConsumption / 30m, 0.1m);
        }

        public Task<decimal> CalculateReorderPointAsync(decimal avgDailyConsumption, int leadTimeDays, decimal safetyStock)
        {
            var rop = (avgDailyConsumption * leadTimeDays) + safetyStock;
            return Task.FromResult(Math.Max(rop, 1m));
        }

        public async Task<List<StockAlert>> GetLowStockAlertsAsync()
        {
            var results = new List<StockAlert>();

            var thresholds = await _context.InventoryThresholds
                .Select(t => new
                {
                    t.Id,
                    t.IngredientId,
                    t.BranchId,
                    t.ReorderPoint,
                    t.SafetyStock
                })
                .ToListAsync();

            foreach (var t in thresholds)
            {
                var currentQty = await _context.BranchIngredientInventories
                    .Where(bi => bi.BranchId == t.BranchId && bi.IngredientId == t.IngredientId)
                    .Select(bi => (decimal?)bi.Quantity)
                    .FirstOrDefaultAsync() ?? 0m;

                var avgDaily = await CalculateAverageDailyConsumptionAsync(t.BranchId, t.IngredientId);
                var daysRemaining = avgDaily > 0 ? (int)Math.Floor(currentQty / avgDaily) : 0;
                var level = StockAlertLevel.Low;
                if (currentQty <= t.SafetyStock) level = StockAlertLevel.OutOfStock;
                else if (currentQty <= t.ReorderPoint) level = StockAlertLevel.Critical;

                var ing = await _context.Ingredients
                    .Where(i => i.Id == t.IngredientId)
                    .Select(i => new { i.Name, i.Unit })
                    .FirstOrDefaultAsync();

                var branchName = await _context.Branches
                    .Where(b => b.Id == t.BranchId)
                    .Select(b => b.Name)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var alert = new StockAlert
                {
                    BranchId = t.BranchId,
                    BranchName = branchName,
                    IngredientId = t.IngredientId,
                    IngredientName = ing?.Name ?? string.Empty,
                    CurrentStock = currentQty,
                    Unit = ing?.Unit ?? string.Empty,
                    ReorderPoint = t.ReorderPoint,
                    SafetyStock = t.SafetyStock,
                    AverageDailyConsumption = avgDaily,
                    DaysRemaining = daysRemaining,
                    AlertLevel = level,
                };

                if (currentQty <= t.ReorderPoint)
                    results.Add(alert);
            }

            var warehouses = await _context.IngredientWarehouses
                .Select(w => new { w.IngredientId, w.Quantity, w.SafetyStock })
                .ToListAsync();

            foreach (var w in warehouses)
            {
                var avgDailyAcrossBranches = 0m;

                var branchIds = await _context.Branches.Select(b => b.Id).ToListAsync();
                var totalConsumption = 0m;
                foreach (var branchId in branchIds)
                {
                    totalConsumption += await CalculateAverageDailyConsumptionAsync(branchId, w.IngredientId);
                }
                avgDailyAcrossBranches = Math.Max(totalConsumption, 0m);

                var leadTimeDays = DEFAULT_LEAD_TIME_DAYS;
                var estimatedRop = await CalculateReorderPointAsync(avgDailyAcrossBranches, leadTimeDays, w.SafetyStock);

                var daysRemaining = avgDailyAcrossBranches > 0 ? (int)Math.Floor(w.Quantity / avgDailyAcrossBranches) : 0;
                var level = StockAlertLevel.Low;
                if (w.Quantity <= w.SafetyStock) level = StockAlertLevel.OutOfStock;
                else if (w.Quantity <= estimatedRop) level = StockAlertLevel.Critical;

                var ing = await _context.Ingredients
                    .Where(i => i.Id == w.IngredientId)
                    .Select(i => new { i.Name, i.Unit })
                    .FirstOrDefaultAsync();

                var warehouseAlert = new StockAlert
                {
                    BranchId = 0,
                    BranchName = "Warehouse",
                    IngredientId = w.IngredientId,
                    IngredientName = ing?.Name ?? string.Empty,
                    CurrentStock = w.Quantity,
                    Unit = ing?.Unit ?? string.Empty,
                    ReorderPoint = estimatedRop,
                    SafetyStock = w.SafetyStock,
                    AverageDailyConsumption = avgDailyAcrossBranches,
                    DaysRemaining = daysRemaining,
                    AlertLevel = level
                };

                if (warehouseAlert.CurrentStock <= warehouseAlert.ReorderPoint)
                    results.Add(warehouseAlert);
            }

            return results;
        }

        public async Task<List<StockAlert>> GetOutOfStockAlertsAsync()
        {
            var alerts = await GetLowStockAlertsAsync();
            return alerts.Where(a => a.AlertLevel == StockAlertLevel.OutOfStock).ToList();
        }

        public async Task UpdateInventoryThresholdAsync(long thresholdId, decimal newReorderPoint, decimal newSafetyStock)
        {
            var threshold = await _context.InventoryThresholds.FindAsync(thresholdId);
            if (threshold != null)
            {
                threshold.ReorderPoint = newReorderPoint;
                threshold.SafetyStock = newSafetyStock;
                threshold.LastModified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
