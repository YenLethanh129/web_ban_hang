using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.EnumTypes;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IIngredientTransferRepository : IRepository<IngredientTransfer>
{
    Task<IEnumerable<IngredientTransfer>> GetTransfersByBranchAsync(long branchId);
    Task<IEnumerable<IngredientTransfer>> GetTransfersByIngredientAsync(long ingredientId);
    Task<IEnumerable<IngredientTransfer>> GetPendingTransfersAsync();
    Task<IngredientTransfer?> GetTransferWithDetailsAsync(long transferId);
    Task<bool> CompleteTransferAsync(long transferId);
    Task<bool> CancelTransferAsync(long transferId, string reason);
}

public class IngredientTransferRepository : Repository<IngredientTransfer>, IIngredientTransferRepository
{
    public IngredientTransferRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<IngredientTransfer>> GetTransfersByBranchAsync(long branchId)
    {
        return await _context.IngredientTransfers
            .Include(t => t.Branch)
            .Include(t => t.Ingredient)
                .ThenInclude(i => i.Category)
            .Where(t => t.BranchId == branchId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IngredientTransfer>> GetTransfersByIngredientAsync(long ingredientId)
    {
        return await _context.IngredientTransfers
            .Include(t => t.Branch)
            .Include(t => t.Ingredient)
                .ThenInclude(i => i.Category)
            .Where(t => t.IngredientId == ingredientId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IngredientTransfer>> GetPendingTransfersAsync()
    {
        return await _context.IngredientTransfers
            .Include(t => t.Branch)
            .Include(t => t.Ingredient)
                .ThenInclude(i => i.Category)
            .Where(t => t.Status == TransferStatus.PENDING)
            .OrderBy(t => t.TransferDate)
            .ToListAsync();
    }

    public async Task<IngredientTransfer?> GetTransferWithDetailsAsync(long transferId)
    {
        return await _context.IngredientTransfers
            .Include(t => t.Branch)
            .Include(t => t.Ingredient)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(t => t.Id == transferId);
    }

    public async Task<bool> CompleteTransferAsync(long transferId)
    {
        var transfer = await _context.IngredientTransfers.FindAsync(transferId);
        if (transfer == null || transfer.Status != TransferStatus.PENDING)
            return false;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Update transfer status
            transfer.Status = TransferStatus.COMPLETED;
            transfer.CompletedDate = DateTime.UtcNow;

            // Update warehouse stock (decrease)
            var warehouseStock = await _context.IngredientWarehouses
                .FirstOrDefaultAsync(w => w.IngredientId == transfer.IngredientId);
            
            if (warehouseStock == null || warehouseStock.Quantity < transfer.Quantity)
                return false;

            warehouseStock.Quantity -= transfer.Quantity;

            // Update branch inventory (increase)
            var branchInventory = await _context.BranchIngredientInventories
                .FirstOrDefaultAsync(b => b.BranchId == transfer.BranchId && b.IngredientId == transfer.IngredientId);

            if (branchInventory == null)
            {
                branchInventory = new BranchIngredientInventory
                {
                    BranchId = transfer.BranchId,
                    IngredientId = transfer.IngredientId,
                    Quantity = transfer.Quantity,
                    ReservedQuantity = 0,
                    SafetyStock = 0,
                    LastTransferDate = DateTime.UtcNow
                };
                await _context.BranchIngredientInventories.AddAsync(branchInventory);
            }
            else
            {
                branchInventory.Quantity += transfer.Quantity;
                branchInventory.LastTransferDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> CancelTransferAsync(long transferId, string reason)
    {
        var transfer = await _context.IngredientTransfers.FindAsync(transferId);
        if (transfer == null || transfer.Status != TransferStatus.PENDING)
            return false;

        transfer.Status = TransferStatus.CANCELLED;
        transfer.Note = $"{transfer.Note} | Cancelled: {reason}";
        
        await _context.SaveChangesAsync();
        return true;
    }
}
