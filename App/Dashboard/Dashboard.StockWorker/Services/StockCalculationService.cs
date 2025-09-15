using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.StockWorker.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class StockCalculationService
    {
        private readonly WebbanhangDbContext _context;
        // Default lead time in days to use when the DB schema does not store per-threshold lead time
        private const int DEFAULT_LEAD_TIME_DAYS = 7;

        public StockCalculationService(WebbanhangDbContext context)
        {
            _context = context;
        }

        public async Task CalculateAndUpdateReorderPointsAsync()
        {
            Console.WriteLine("Bắt đầu tính toán điểm đặt hàng lại...");
            // Project only the columns we know exist in your schema to avoid EF trying to read
            // missing columns in mapped entities. We'll fetch ingredient/branch display fields separately.
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

                // Use a sensible default lead time when DB doesn't hold it
                var leadTimeDays = DEFAULT_LEAD_TIME_DAYS;

                var reorderPoint = await CalculateReorderPointAsync(
                    avgDailyConsumption, leadTimeDays, t.SafetyStock);

                // Update only the columns that exist in the schema using a raw SQL to avoid
                // materializing entities that map to missing columns in DB.
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE [dbo].[inventory_thresholds] SET reorder_point = {0}, safety_stock = {1}, last_modified = {2} WHERE id = {3}",
                    reorderPoint, t.SafetyStock, DateTime.UtcNow, t.Id);

                // Fetch ingredient name for logging only (select scalar to avoid loading full entity)
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

            // Some deployments store order product consumption in order tables, others rely
            // on inventory_movements (OUT). To avoid schema mismatch with Orders/OrderDetails
            // in the target DB, compute consumption from inventory_movements only using a
            // raw SQL projection that aliases snake_case columns to the EF property names.

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

        // Return rich StockAlert objects so notification logic can be centralized in the notification service
        public async Task<List<StockAlert>> GetLowStockAlertsAsync()
        {
            var results = new List<StockAlert>();

            // Avoid selecting non-existent columns by projecting only known fields and
            // retrieving display names / units with separate scalar queries.
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
                // Get current quantity as scalar to avoid loading entity properties that may not exist
                var currentQty = await _context.BranchIngredientInventories
                    .Where(bi => bi.BranchId == t.BranchId && bi.IngredientId == t.IngredientId)
                    .Select(bi => (decimal?)bi.Quantity)
                    .FirstOrDefaultAsync() ?? 0m;

                var avgDaily = await CalculateAverageDailyConsumptionAsync(t.BranchId, t.IngredientId);
                var daysRemaining = avgDaily > 0 ? (int)Math.Floor(currentQty / avgDaily) : 0;
                var level = StockAlertLevel.Low;
                if (currentQty <= t.SafetyStock) level = StockAlertLevel.OutOfStock;
                else if (currentQty <= t.ReorderPoint) level = StockAlertLevel.Critical;

                // Fetch ingredient display fields and branch name as scalars
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
                    // ThresholdId = t.Id
                };

                // Only include alerts where current <= reorder point (low)
                if (currentQty <= t.ReorderPoint)
                    results.Add(alert);
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
