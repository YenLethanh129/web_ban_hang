using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.DataAccess.Repositories;

public interface IBranchRepository : IRepository<Branch>
{
    void Update(Branch branch);
}

public class BranchRepository : Repository<Branch>, IBranchRepository
{
    public BranchRepository(WebbanhangDbContext dbContext) : base(dbContext)
    {
    }

    public void Update(Branch branch)
    {
        _dbContext.Update(branch);
    }
}
