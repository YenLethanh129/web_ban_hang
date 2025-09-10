using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IExpenseRepository : IRepository<BranchExpense>
{
    Task<IEnumerable<VCogsSummary>> GetCogsSummaryByBranchAndDateAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
}
public class ExpenseRepository(WebbanhangDbContext context) : Repository<BranchExpense>(context), IExpenseRepository
{
    public async Task<IEnumerable<VCogsSummary>> GetCogsSummaryByBranchAndDateAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var query = _context.VExpensesSummaries
            .Where(e => e.Month >= fromDate.Month && e.Year >= fromDate.Year &&
                        e.Month <= toDate.Month && e.Year <= toDate.Year);

        if (branchId.HasValue)
        {
            query = query.Where(e => e.BranchId == branchId.Value);
        }

        return await query.ToListAsync();
    }

}
