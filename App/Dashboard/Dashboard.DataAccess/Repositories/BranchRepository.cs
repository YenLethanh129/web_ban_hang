using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Branches;

namespace Dashboard.DataAccess.Repositories;

public interface IBranchRepository : IRepository<Branch>
{
}

public class BranchRepository : Repository<Branch>, IBranchRepository
{
    public BranchRepository(WebbanhangDbContext dbContext) : base(dbContext)
    {
    }

}
