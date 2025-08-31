using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Sprache;

namespace Dashboard.DataAccess.Repositories;
public interface IExpenseRepository : IRepository<BranchExpense>
{
    Task<IEnumerable<BranchExpense>> GetExpensesByBranchAsync(int branchId, DateOnly fromDate, DateOnly toDate);
    Task<IEnumerable<BranchExpense>> GetExpensesByTypeAsync(string expenseType, DateOnly fromDate, DateOnly toDate);
    Task<decimal> GetTotalExpensesAsync(int branchId, DateOnly fromDate, DateOnly toDate);
    Task<IEnumerable<VExpensesSummary>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, int? branchId = null);
    Task<IEnumerable<BranchExpense>> GetExpensesByPeriodAsync(DateTime fromDate, DateTime toDate, long branchId);
}
public class ExpenseRepository(WebbanhangDbContext context) : Repository<BranchExpense>(context), IExpenseRepository
{
    public async Task<IEnumerable<BranchExpense>> GetExpensesByBranchAsync(int branchId, DateOnly fromDate, DateOnly toDate)
    {
        try
        {
            return await _context.BranchExpenses
            .Where(e => e.BranchId == branchId &&
                        e.StartDate >= fromDate &&
                        e.EndDate <= toDate)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving expenses for branch {branchId} from {fromDate} to {toDate}: {ex.Message}", ex);
        }

    }
    public async Task<IEnumerable<BranchExpense>> GetExpensesByTypeAsync(string expenseType, DateOnly fromDate, DateOnly toDate)
    {
        return await _context.BranchExpenses
            .Where(e => e.ExpenseType == expenseType &&
                        e.StartDate >= fromDate &&
                        e.EndDate <= toDate)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }
    public async Task<decimal> GetTotalExpensesAsync(int branchId, DateOnly fromDate, DateOnly toDate)
    {
        try
        {
            return await _context.BranchExpenses
                .Where(e => e.BranchId == branchId &&
                            e.StartDate >= fromDate &&
                            e.EndDate <= toDate)
                .SumAsync(e => e.Amount);
        } 
        catch (Exception ex)
        {
            throw new Exception($"Error calculating total expenses for branch {branchId} from {fromDate} to {toDate}: {ex.Message}", ex);
        }
    }
    public async Task<IEnumerable<VExpensesSummary>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, int? branchId = null)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving expenses summary from {fromDate} to {toDate} for branch {branchId}: {ex.Message}", ex);
        }
    }
    public async Task<IEnumerable<BranchExpense>> GetExpensesByPeriodAsync(DateTime fromDate, DateTime toDate, long branchId)
    {
        try
        {
            return await _context.BranchExpenses
                .Where(e => e.BranchId == branchId &&
                            e.StartDate >= DateOnly.FromDateTime(fromDate) &&
                            e.EndDate <= DateOnly.FromDateTime(toDate))
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving expenses for branch {branchId} from {fromDate} to {toDate}: {ex.Message}", ex);
        }
    }
}
