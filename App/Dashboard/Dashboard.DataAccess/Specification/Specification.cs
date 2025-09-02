                   using System.Linq.Expressions;

namespace Dashboard.DataAccess.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Where { get; }
    List<Expression<Func<T, object>>> Includes { get; }
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
    public List<string> IncludeStrings { get; } = [];
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    protected void AddIncludes(Expression<Func<T, object>> include)
    {
        Includes.Add(include);
    }
}
