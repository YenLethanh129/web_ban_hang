using Dashboard.BussinessLogic.Dtos.ExpenseDtos;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Sprache;
using System.Linq.Expressions;

namespace Dashboard.BussinessLogic.Specifications;

/// <summary>
/// Specification builder for Expense queries based on business logic requirements
/// </summary>
public static class ExpenseSpecifications
{
    public static Specification<BranchExpense> ByBranch(long branchId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => e.BranchId == branchId;

        if (fromDate.HasValue)
            predicate = CombinePredicates(predicate, e => (e.EndDate ?? e.StartDate) >= fromDate.Value);

        if (toDate.HasValue)
            predicate = CombinePredicates(predicate, e => e.StartDate <= toDate.Value);

        var spec = new Specification<BranchExpense>(predicate);
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchExpense> ByExpenseType(string expenseType, DateTime? fromDate = null, DateTime? toDate = null)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => e.ExpenseType == expenseType;

        if (fromDate.HasValue)
            predicate = CombinePredicates(predicate, e => (e.EndDate ?? e.StartDate) >= fromDate.Value);

        if (toDate.HasValue)
            predicate = CombinePredicates(predicate, e => e.StartDate <= toDate.Value);

        var spec = new Specification<BranchExpense>(predicate);
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchExpense> ByDateRange(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var fromDateTime = fromDate;
        var toDateTime = toDate;

        Expression<Func<BranchExpense, bool>> predicate = e => 
            e.StartDate <= toDateTime && (e.EndDate ?? e.StartDate) >= fromDateTime;

        if (branchId.HasValue)
            predicate = CombinePredicates(predicate, e => e.BranchId == branchId.Value);

        var spec = new Specification<BranchExpense>(predicate);
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchExpense> ByAmountRange(decimal? minAmount, decimal? maxAmount)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => true;

        if (minAmount.HasValue)
            predicate = CombinePredicates(predicate, e => e.Amount >= minAmount.Value);

        if (maxAmount.HasValue)
            predicate = CombinePredicates(predicate, e => e.Amount <= maxAmount.Value);

        return new Specification<BranchExpense>(predicate);
    }

    public static Specification<BranchExpense> WithAdvancedFilter(GetExpensesInput input)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => true;

        if (input.FromDate.HasValue)
        {
            var fromDateTime = input.FromDate.Value;
            predicate = CombinePredicates(predicate, e => (e.EndDate ?? e.StartDate) >= fromDateTime);
        }

        if (input.ToDate.HasValue)
        {
            var toDateTime = input.ToDate.Value;
            predicate = CombinePredicates(predicate, e => e.StartDate <= toDateTime);
        }

        if (input.BranchId.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.BranchId == input.BranchId.Value);
        }

        if (!string.IsNullOrEmpty(input.ExpenseType))
        {
            predicate = CombinePredicates(predicate, e => e.ExpenseType == input.ExpenseType);
        }

        if (input.MinAmount.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.Amount >= input.MinAmount.Value);
        }

        if (input.MaxAmount.HasValue)
        {
            predicate = CombinePredicates(predicate, e => e.Amount <= input.MaxAmount.Value);
        }

        var spec = new Specification<BranchExpense>(predicate);
        spec.IncludeStrings.Add("Branch");
        return spec;
    }
    public static Specification<VCogsSummary> CogsWithAdvancedFilter(GetExpensesInput input)
    {
        Expression<Func<VCogsSummary, bool>> predicate = e => true;
        if (input.FromDate.HasValue)
        {
            var fromDate = input.FromDate.Value;
            predicate = SpecificationHelper.CombinePredicates(predicate,
                e => e.Year > fromDate.Year
                  || (e.Year == fromDate.Year && e.Month >= fromDate.Month));
        }

        if (input.ToDate.HasValue)
        {
            var toDate = input.ToDate.Value;
            predicate = SpecificationHelper.CombinePredicates(predicate,
                e => e.Year < toDate.Year
                  || (e.Year == toDate.Year && e.Month <= toDate.Month));
        }

        if (input.BranchId.HasValue)
        {
            predicate = SpecificationHelper.CombinePredicates(predicate, e => e.BranchId == input.BranchId.Value);
        }
        return new Specification<VCogsSummary>(predicate);
    }
    public static Specification<BranchExpense> ForTotalCalculation(long? branchId, DateTime? fromDate, DateTime? toDate)
    {
        Expression<Func<BranchExpense, bool>> predicate = e => true;

        if (branchId.HasValue)
            predicate = CombinePredicates(predicate, e => e.BranchId == branchId.Value);

        if (fromDate.HasValue)
            predicate = CombinePredicates(predicate, e => (e.EndDate ?? e.StartDate) >= fromDate.Value);

        if (toDate.HasValue)
            predicate = CombinePredicates(predicate, e => e.StartDate <= toDate.Value);

        return new Specification<BranchExpense>(predicate);
    }

    private static Expression<Func<BranchExpense, bool>> CombinePredicates(
        Expression<Func<BranchExpense, bool>> first,
        Expression<Func<BranchExpense, bool>> second)
    {
        return SpecificationHelper.CombinePredicates(first, second);
    }
}
