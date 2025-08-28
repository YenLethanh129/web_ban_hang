using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dashboard.DataAccess.Data;

public interface IUnitOfWork : IDisposable
{
    IBranchRepository Branchs { get; }

    Task SaveChangesAsync();
    void BeginTransaction();
    void Commit();
    void Rollback();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly WebbanhangDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(WebbanhangDbContext dbContext)
    {
        _context = dbContext;

        Branchs = new BranchRepository(_context);
    }

    public IBranchRepository Branchs { get; private set; }

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