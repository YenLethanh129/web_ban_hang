using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dashboard.DataAccess.Data;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<T> Repository<T>() where T : class;
    Task SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
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

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction started.");
        }
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        await _context.DisposeAsync();
    }
}