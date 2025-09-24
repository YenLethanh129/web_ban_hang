
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Employees;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(WebbanhangDbContext context) : base(context)
        {
        }

    }
}
