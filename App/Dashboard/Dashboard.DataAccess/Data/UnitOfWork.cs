using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Repositories;

namespace Dashboard.DataAccess.Data;

public interface IUnitOfWork : IDisposable
{
    IBranchRepository Branchs { get; }

    Task SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly WebbanhangDbContext _dbContext;

    public UnitOfWork(WebbanhangDbContext dbContext)
    {
        _dbContext = dbContext;

        Branchs = new BranchRepository(_dbContext);
    }

    public IBranchRepository Branchs { get; private set; }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}