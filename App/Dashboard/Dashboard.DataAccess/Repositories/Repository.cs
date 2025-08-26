using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false);

    Task<int> GetCountAsync();

    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(long id);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Guid id);

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
    Task<T?> GetWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
}

public class Repository<T> where T : class
{
    protected readonly WebbanhangDbContext _dbContext;

    private readonly DbSet<T> _dbSet;

    public Repository(WebbanhangDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        return await _dbSet.ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    // specs
    public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await ApplySpec(spec).AsNoTracking().ToListAsync();
        }

        return await ApplySpec(spec).ToListAsync();
    }

    public async Task<T?> GetWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await ApplySpec(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        return await ApplySpec(spec).FirstOrDefaultAsync();
    }

    private IQueryable<T> ApplySpec(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
