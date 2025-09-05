using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Dashboard.DataAccess.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Where { get; }
    List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}

public class Specification<T> : ISpecification<T>
{
    public Specification()
    {
    }

    public Specification(Expression<Func<T, bool>> where)
    {
        Where = where;
    }

    public Expression<Func<T, bool>>? Where { get; }

    public List<string> IncludeStrings { get; } = new();

    public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new();

    protected void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
}