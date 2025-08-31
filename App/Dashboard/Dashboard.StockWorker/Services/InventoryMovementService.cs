using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class InventoryMovementService
    {
        private readonly WebbanhangDbContext _context;

        public InventoryMovementService(WebbanhangDbContext context)
        {
            _context = context;
        }

        public async Task ProcessOrderConsumptionAsync(long orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductRecipes)
                .ThenInclude(pr => pr.Ingredient)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found");
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                foreach (var productRecipe in orderDetail.Product.ProductRecipes)
                {
                    var requiredQuantity = productRecipe.Quantity * orderDetail.Quantity;

                    await CreateInventoryMovementAsync(
                        order.BranchId ?? 1,
                        productRecipe.IngredientId,
                        "OUT",
                        requiredQuantity,
                        productRecipe.Ingredient.Unit,
                        "ORDER",
                        orderId,
                        order.OrderCode,
                        $"Tiêu thụ cho order {order.OrderCode}",
                        null);
                }
            }
        }

        public async Task CreateInventoryMovementAsync(
            long branchId,
            long ingredientId,
            string movementType,
            decimal quantity,
            string unit,
            string? referenceType = null,
            long? referenceId = null,
            string? referenceCode = null,
            string? notes = null,
            long? employeeId = null)
        {
            var currentInventory = await _context.BranchIngredientInventories
                .FirstOrDefaultAsync(i => i.BranchId == branchId && i.IngredientId == ingredientId);

            decimal quantityBefore = currentInventory?.Quantity ?? 0;
            decimal quantityAfter = movementType == "IN" ? quantityBefore + quantity : quantityBefore - quantity;

            if (movementType == "OUT" && quantityAfter < 0)
            {
                quantityAfter = 0;
            }

            var movement = new InventoryMovement
            {
                BranchId = branchId,
                IngredientId = ingredientId,
                MovementType = movementType,
                Quantity = quantity,
                Unit = unit,
                QuantityBefore = quantityBefore,
                QuantityAfter = quantityAfter,
                ReferenceType = referenceType,
                ReferenceId = referenceId,
                ReferenceCode = referenceCode,
                Notes = notes,
                EmployeeId = employeeId,
                MovementDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            _context.InventoryMovements.Add(movement);

            if (currentInventory != null)
            {
                currentInventory.Quantity = quantityAfter;
                currentInventory.LastModified = DateTime.UtcNow;
            }
            else
            {
                var newInventory = new BranchIngredientInventory
                {
                    BranchId = branchId,
                    IngredientId = ingredientId,
                    Quantity = quantityAfter,
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };
                _context.BranchIngredientInventories.Add(newInventory);
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateStockReceiptAsync(
            long branchId,
            long ingredientId,
            decimal quantity,
            string unit,
            string? purchaseOrderCode = null,
            string? notes = null,
            long? employeeId = null)
        {
            await CreateInventoryMovementAsync(
                branchId,
                ingredientId,
                "IN",
                quantity,
                unit,
                "PURCHASE",
                null,
                purchaseOrderCode,
                notes ?? "Nhập kho",
                employeeId);
        }

        public async Task CreateStockAdjustmentAsync(
            long branchId,
            long ingredientId,
            decimal adjustmentQuantity,
            string unit,
            string reason,
            long? employeeId = null)
        {
            var movementType = adjustmentQuantity > 0 ? "IN" : "OUT";
            var quantity = Math.Abs(adjustmentQuantity);

            await CreateInventoryMovementAsync(
                branchId,
                ingredientId,
                movementType,
                quantity,
                unit,
                "ADJUSTMENT",
                null,
                null,
                $"Điều chỉnh kho: {reason}",
                employeeId);
        }

        public async Task CreateInventoryTransferAsync(
            long fromBranchId,
            long toBranchId,
            long ingredientId,
            decimal quantity,
            string unit,
            string? notes = null,
            long? employeeId = null)
        {
            await CreateInventoryMovementAsync(
                fromBranchId,
                ingredientId,
                "OUT",
                quantity,
                unit,
                "TRANSFER",
                toBranchId,
                $"TRANSFER-{DateTime.UtcNow:yyyyMMddHHmmss}",
                notes ?? $"Chuyển kho đến chi nhánh {toBranchId}",
                employeeId);

            await CreateInventoryMovementAsync(
                toBranchId,
                ingredientId,
                "IN",
                quantity,
                unit,
                "TRANSFER",
                fromBranchId,
                $"TRANSFER-{DateTime.UtcNow:yyyyMMddHHmmss}",
                notes ?? $"Nhận chuyển kho từ chi nhánh {fromBranchId}",
                employeeId);
        }

        public async Task<List<InventoryMovement>> GetMovementHistoryAsync(
            long? branchId = null,
            long? ingredientId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? movementType = null)
        {
            var query = _context.InventoryMovements
                .Include(m => m.Branch)
                .Include(m => m.Ingredient)
                .AsQueryable();

            if (branchId.HasValue)
                query = query.Where(m => m.BranchId == branchId.Value);

            if (ingredientId.HasValue)
                query = query.Where(m => m.IngredientId == ingredientId.Value);

            if (fromDate.HasValue)
                query = query.Where(m => m.MovementDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(m => m.MovementDate <= toDate.Value);

            if (!string.IsNullOrEmpty(movementType))
                query = query.Where(m => m.MovementType == movementType);

            return await query
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<decimal> GetCurrentStockAsync(long branchId, long ingredientId)
        {
            var inventory = await _context.BranchIngredientInventories
                .FirstOrDefaultAsync(i => i.BranchId == branchId && i.IngredientId == ingredientId);

            return inventory?.Quantity ?? 0;
        }

        public async Task<Dictionary<long, decimal>> GetCurrentStockAllIngredientsAsync(long branchId)
        {
            return await _context.BranchIngredientInventories
                .Where(i => i.BranchId == branchId)
                .ToDictionaryAsync(i => i.IngredientId, i => i.Quantity);
        }
    }
}
