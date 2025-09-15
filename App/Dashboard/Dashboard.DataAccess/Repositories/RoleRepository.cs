using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repositories;

public interface IRoleRepository : IRepository<Role>
{
}

public class RoleRepository : Repository<Role>, IRoleRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public RoleRepository(
        WebbanhangDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }
}

