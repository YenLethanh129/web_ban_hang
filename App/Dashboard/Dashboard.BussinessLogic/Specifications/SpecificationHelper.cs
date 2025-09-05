using System.Linq.Expressions;

namespace Dashboard.BussinessLogic.Specifications;

/// <summary>
/// Helper class for combining expressions in specifications
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

/// <summary>
/// Common helper methods for Specification building
/// </summary>
internal static class SpecificationHelper
{
    public static Expression<Func<T, bool>> CombinePredicates<T>(
        Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var firstBody = new ParameterReplacer(parameter).Visit(first.Body);
        var secondBody = new ParameterReplacer(parameter).Visit(second.Body);
        var combined = Expression.AndAlso(firstBody!, secondBody!);
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }
}
