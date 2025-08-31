using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dashboard.DataAccess.Data;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task SaveChangesAsync();
    void BeginTransaction();
    void Commit();
    void Rollback();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly WebbanhangDbContext _context;
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction? _transaction;
    public UnitOfWork(WebbanhangDbContext dbContext) => _context = dbContext;
    public IRepository<T> Repository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out var repo))
        {
            return (IRepository<T>)repo;
        }

        var repositoryType = typeof(Repository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context)!;

        _repositories.Add(typeof(T), repositoryInstance);
        return (IRepository<T>)repositoryInstance;
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}