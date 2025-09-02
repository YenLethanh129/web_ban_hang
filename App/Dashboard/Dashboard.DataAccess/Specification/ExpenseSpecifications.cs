using Dashboard.DataAccess.Models.Entities;
using System.Linq.Expressions;

namespace Dashboard.DataAccess.Specification;

/// <summary>
/// Specification for filtering expenses by branch and date range
/// </summary>
public class ExpensesByBranchSpecification : Specification<BranchExpense>
{
    public ExpensesByBranchSpecification(long branchId, DateOnly fromDate, DateOnly toDate)
        : base(e => e.BranchId == branchId &&
                   e.StartDate >= fromDate &&
                   (e.EndDate == null || e.EndDate <= toDate))
    {
    }
}

/// <summary>
/// Specification for filtering expenses by type and date range
/// </summary>
public class ExpensesByTypeSpecification : Specification<BranchExpense>
{
    public ExpensesByTypeSpecification(string expenseType, DateOnly fromDate, DateOnly toDate)
        : base(e => e.ExpenseType == expenseType &&
                   e.StartDate >= fromDate &&
                   (e.EndDate == null || e.EndDate <= toDate))
    {
    }
}

/// <summary>
/// Specification for filtering expenses by period with optional branch filter
/// Uses overlapping date logic to find expenses that intersect with the given period
/// </summary>
public class ExpensesByPeriodSpecification : Specification<BranchExpense>
{
    public ExpensesByPeriodSpecification(DateTime fromDate, DateTime toDate, long? branchId = null)
        : base(BuildPredicate(fromDate, toDate, branchId))
    {
    }

    private static Expression<Func<BranchExpense, bool>> BuildPredicate(DateTime fromDate, DateTime toDate, long? branchId)
    {
        var fromDateOnly = DateOnly.FromDateTime(fromDate);
        var toDateOnly = DateOnly.FromDateTime(toDate);

        return branchId.HasValue
            ? e => e.BranchId == branchId.Value &&
                   e.StartDate <= toDateOnly &&
                   (e.EndDate ?? e.StartDate) >= fromDateOnly
            : e => e.StartDate <= toDateOnly &&
                   (e.EndDate ?? e.StartDate) >= fromDateOnly;
    }
}

/// <summary>
/// Specification for filtering expenses that intersect with a given period
/// This handles cases where expenses might span across multiple periods
/// </summary>
public class ExpensesInPeriodSpecification : Specification<BranchExpense>
{
    public ExpensesInPeriodSpecification(DateTime fromDate, DateTime toDate, long? branchId = null)
        : base(BuildPredicate(fromDate, toDate, branchId))
    {
        // Add includes if needed for navigation properties
        if (branchId.HasValue)
        {
            AddIncludes(e => e.Branch!);
        }
    }

    private static Expression<Func<BranchExpense, bool>> BuildPredicate(DateTime fromDate, DateTime toDate, long? branchId)
    {
        var fromDateOnly = DateOnly.FromDateTime(fromDate);
        var toDateOnly = DateOnly.FromDateTime(toDate);

        return branchId.HasValue
            ? e => e.BranchId == branchId.Value &&
                   e.StartDate <= toDateOnly &&
                   (e.EndDate ?? e.StartDate) >= fromDateOnly
            : e => e.StartDate <= toDateOnly &&
                   (e.EndDate ?? e.StartDate) >= fromDateOnly;
    }
}

/// <summary>
/// Specification for calculating total expenses by branch and date range
/// </summary>
public class ExpensesTotalSpecification : Specification<BranchExpense>
{
    public ExpensesTotalSpecification(long branchId, DateOnly fromDate, DateOnly toDate)
        : base(e => e.BranchId == branchId &&
                   e.StartDate >= fromDate &&
                   (e.EndDate == null || e.EndDate <= toDate))
    {
    }
}

/// <summary>
/// Advanced specification for filtering expenses with multiple criteria
/// </summary>
public class ExpensesAdvancedFilterSpecification : Specification<BranchExpense>
{
    public ExpensesAdvancedFilterSpecification(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        long? branchId = null,
        string? expenseType = null,
        decimal? minAmount = null,
        decimal? maxAmount = null)
        : base(BuildAdvancedPredicate(fromDate, toDate, branchId, expenseType, minAmount, maxAmount))
    {
        if (branchId.HasValue)
        {
            AddIncludes(e => e.Branch!);
        }
    }

    private static Expression<Func<BranchExpense, bool>> BuildAdvancedPredicate(
        DateTime? fromDate,
        DateTime? toDate,
        long? branchId,
        string? expenseType,
        decimal? minAmount,
        decimal? maxAmount)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => true;

        if (fromDate.HasValue)
        {
            var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
            predicate = CombinePredicates(predicate, e => (e.EndDate ?? e.StartDate) >= fromDateOnly);
        }

        if (toDate.HasValue)
        {
            var toDateOnly = DateOnly.FromDateTime(toDate.Value);
            predicate = CombinePredicates(predicate, e => e.StartDate <= toDateOnly);
        }

        if (branchId.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.BranchId == branchId.Value);
        }

        if (!string.IsNullOrEmpty(expenseType))
        {
            predicate = CombinePredicates(predicate, e => e.ExpenseType == expenseType);
        }

        if (minAmount.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.Amount >= minAmount.Value);
        }

        if (maxAmount.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.Amount <= maxAmount.Value);
        }

        return predicate;
    }

    private static Expression<Func<BranchExpense, bool>> CombinePredicates(
        Expression<Func<BranchExpense, bool>> first,
        Expression<Func<BranchExpense, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(BranchExpense), "e");
        var firstBody = new ParameterReplacer(parameter).Visit(first.Body);
        var secondBody = new ParameterReplacer(parameter).Visit(second.Body);
        var combined = Expression.AndAlso(firstBody!, secondBody!);
        return Expression.Lambda<Func<BranchExpense, bool>>(combined, parameter);
    }
}

/// <summary>
/// Helper class for combining expressions
/// </summary>
internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    public ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return base.VisitParameter(_parameter);
    }
}
