using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;
public interface IExpenseRepository : IRepository<BranchExpense>
{
    Task<IEnumerable<BranchExpense>> GetExpensesByBranchAsync(long branchId, DateOnly fromDate, DateOnly toDate);
    Task<IEnumerable<BranchExpense>> GetExpensesByTypeAsync(string expenseType, DateOnly fromDate, DateOnly toDate);
    Task<decimal> GetTotalExpensesAsync(long branchId, DateOnly fromDate, DateOnly toDate);
    Task<IEnumerable<VExpensesSummary>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, long? branchId = null);
    Task<IEnumerable<BranchExpense>> GetExpensesByPeriodAsync(DateTime fromDate, DateTime toDate, long branchId);
    Task<IEnumerable<BranchExpense>> GetExpensesInPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    
    // New methods using advanced specifications
    Task<IEnumerable<BranchExpense>> GetExpensesWithAdvancedFilterAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        long? branchId = null,
        string? expenseType = null,
        decimal? minAmount = null,
        decimal? maxAmount = null);
        
    Task<decimal> GetTotalExpensesWithFilterAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        long? branchId = null,
        string? expenseType = null);
}
public class ExpenseRepository(WebbanhangDbContext context) : Repository<BranchExpense>(context), IExpenseRepository
{
    public async Task<IEnumerable<BranchExpense>> GetExpensesByBranchAsync(long branchId, DateOnly fromDate, DateOnly toDate)
    {
        var specification = new ExpensesByBranchSpecification(branchId, fromDate, toDate);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.OrderByDescending(e => e.StartDate);
    }

    public async Task<IEnumerable<BranchExpense>> GetExpensesByTypeAsync(string expenseType, DateOnly fromDate, DateOnly toDate)
    {
        var specification = new ExpensesByTypeSpecification(expenseType, fromDate, toDate);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.OrderByDescending(e => e.StartDate);
    }

    public async Task<decimal> GetTotalExpensesAsync(long branchId, DateOnly fromDate, DateOnly toDate)
    {
        var specification = new ExpensesTotalSpecification(branchId, fromDate, toDate);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.Sum(e => e.Amount);
    }
    public async Task<IEnumerable<VExpensesSummary>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, long? branchId = null)
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

    public async Task<IEnumerable<BranchExpense>> GetExpensesByPeriodAsync(DateTime fromDate, DateTime toDate, long branchId)
    {
        var specification = new ExpensesByPeriodSpecification(fromDate, toDate, branchId);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.OrderByDescending(e => e.StartDate);
    }

    public async Task<IEnumerable<BranchExpense>> GetExpensesInPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var specification = new ExpensesInPeriodSpecification(fromDate, toDate, branchId);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.OrderByDescending(e => e.StartDate);
    }

    // New advanced methods using specifications
    public async Task<IEnumerable<BranchExpense>> GetExpensesWithAdvancedFilterAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        long? branchId = null,
        string? expenseType = null,
        decimal? minAmount = null,
        decimal? maxAmount = null)
    {
        var specification = new ExpensesAdvancedFilterSpecification(
            fromDate, toDate, branchId, expenseType, minAmount, maxAmount);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.OrderByDescending(e => e.StartDate);
    }

    public async Task<decimal> GetTotalExpensesWithFilterAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        long? branchId = null,
        string? expenseType = null)
    {
        var specification = new ExpensesAdvancedFilterSpecification(
            fromDate, toDate, branchId, expenseType);
        var expenses = await GetAllWithSpecAsync(specification, true);
        return expenses.Sum(e => e.Amount);
    }
}
