using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IIngredientTransferRequestRepository : IRepository<IngredientTransferRequest>
{
    Task<IEnumerable<IngredientTransferRequest>> GetRequestsByBranchAsync(long branchId);
    Task<IEnumerable<IngredientTransferRequest>> GetPendingRequestsAsync();
    Task<IngredientTransferRequest?> GetRequestWithDetailsAsync(long requestId);
    Task<bool> ApproveRequestAsync(long requestId, string approvedBy);
    Task<bool> RejectRequestAsync(long requestId, string rejectedBy, string reason);
    Task<bool> CompleteRequestAsync(long requestId);
    Task<IngredientTransferRequest> CreateRequestWithDetailsAsync(IngredientTransferRequest request, List<IngredientTransferRequestDetail> details);
}

public class IngredientTransferRequestRepository : Repository<IngredientTransferRequest>, IIngredientTransferRequestRepository
{
    public IngredientTransferRequestRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<IngredientTransferRequest>> GetRequestsByBranchAsync(long branchId)
    {
        return await _context.IngredientTransferRequests
            .Include(r => r.Branch)
            .Include(r => r.TransferRequestDetails)
                .ThenInclude(d => d.Ingredient)
                    .ThenInclude(i => i.Category)
            .Where(r => r.BranchId == branchId)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IngredientTransferRequest>> GetPendingRequestsAsync()
    {
        return await _context.IngredientTransferRequests
            .Include(r => r.Branch)
            .Include(r => r.TransferRequestDetails)
                .ThenInclude(d => d.Ingredient)
                    .ThenInclude(i => i.Category)
            .Where(r => r.Status == TransferStatus.PENDING)
            .OrderBy(r => r.RequiredDate)
            .ToListAsync();
    }

    public async Task<IngredientTransferRequest?> GetRequestWithDetailsAsync(long requestId)
    {
        return await _context.IngredientTransferRequests
            .Include(r => r.Branch)
            .Include(r => r.TransferRequestDetails)
                .ThenInclude(d => d.Ingredient)
                    .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(r => r.Id == requestId);
    }

    public async Task<bool> ApproveRequestAsync(long requestId, string approvedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var request = await GetRequestWithDetailsAsync(requestId);
            if (request == null || request.Status != TransferStatus.PENDING)
                return false;

            // Check warehouse availability for each item
            foreach (var detail in request.TransferRequestDetails)
            {
                var warehouseStock = await _context.IngredientWarehouses
                    .FirstOrDefaultAsync(w => w.IngredientId == detail.IngredientId);

                if (warehouseStock == null || warehouseStock.Quantity < detail.RequestedQuantity)
                {
                    detail.ApprovedQuantity = warehouseStock?.Quantity ?? 0;
                    detail.Status = warehouseStock?.Quantity > 0 ? TransferStatus.APPROVED : TransferStatus.REJECTED;
                }
                else
                {
                    detail.ApprovedQuantity = detail.RequestedQuantity;
                    detail.Status = TransferStatus.APPROVED;
                }
            }

            request.Status = TransferStatus.APPROVED;
            request.ApprovedDate = DateTime.UtcNow;
            request.ApprovedBy = approvedBy;

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

    public async Task<bool> RejectRequestAsync(long requestId, string rejectedBy, string reason)
    {
        var request = await _context.IngredientTransferRequests.FindAsync(requestId);
        if (request == null || request.Status != TransferStatus.PENDING)
            return false;

        request.Status = TransferStatus.REJECTED;
        request.ApprovedBy = rejectedBy;
        request.ApprovedDate = DateTime.UtcNow;
        request.Note = $"{request.Note} | Rejected: {reason}";

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteRequestAsync(long requestId)
    {
        var request = await _context.IngredientTransferRequests
            .Include(r => r.TransferRequestDetails)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null || request.Status != TransferStatus.APPROVED)
            return false;

        // Check if all details are transferred
        var allTransferred = request.TransferRequestDetails.All(d => 
            d.Status == TransferStatus.TRANSFERRED || 
            d.Status == TransferStatus.REJECTED);

        if (allTransferred)
        {
            request.Status = TransferStatus.COMPLETED;
            request.CompletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IngredientTransferRequest> CreateRequestWithDetailsAsync(
        IngredientTransferRequest request, 
        List<IngredientTransferRequestDetail> details)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Generate request number
            var today = DateTime.Today;
            var requestCount = await _context.IngredientTransferRequests
                .CountAsync(r => r.RequestDate.Date == today);
            
            request.RequestNumber = $"TR{today:yyyyMMdd}{(requestCount + 1):D3}";
            request.TotalItems = details.Count;

            await _context.IngredientTransferRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            // Add details
            foreach (var detail in details)
            {
                detail.TransferRequestId = request.Id;
            }
            
            await _context.IngredientTransferRequestDetails.AddRangeAsync(details);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return request;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
