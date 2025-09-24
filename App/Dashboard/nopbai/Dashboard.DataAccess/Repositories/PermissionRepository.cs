using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {

    }
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(WebbanhangDbContext context) : base(context)
        {
        }
    }

}
