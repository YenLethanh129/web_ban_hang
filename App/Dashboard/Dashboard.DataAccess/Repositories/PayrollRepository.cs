using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Dashboard.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IPayrollRepository : IRepository<Payroll>
{
    Task<List<Payroll>> GetPayrollsByMonthAsync(int month, int year, long? branchId = null);
    Task<Payroll?> GetPayrollByEmployeeAndMonthAsync(long employeeId, int month, int year);
    Task<List<Payroll>> GetPayrollsByEmployeeAsync(long employeeId, int? year = null);
    Task<decimal> GetTotalSalaryByBranchAsync(long branchId, int month, int year);
    Task<bool> ExistsPayrollAsync(long employeeId, int month, int year);
}

public class PayrollRepository : Repository<Payroll>, IPayrollRepository
{
    public PayrollRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<List<Payroll>> GetPayrollsByMonthAsync(int month, int year, long? branchId = null)
    {
        var query = _context.Payrolls
            .Include(p => p.Employee)
            .ThenInclude(e => e.Branch)
            .Where(p => p.Month == month && p.Year == year);

        if (branchId.HasValue)
        {
            query = query.Where(p => p.Employee.BranchId == branchId.Value);
        }

        return await query.OrderBy(p => p.Employee.FullName).ToListAsync();
    }

    public async Task<Payroll?> GetPayrollByEmployeeAndMonthAsync(long employeeId, int month, int year)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .ThenInclude(e => e.Branch)
            .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);
    }

    public async Task<List<Payroll>> GetPayrollsByEmployeeAsync(long employeeId, int? year = null)
    {
        var query = _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.EmployeeId == employeeId);

        if (year.HasValue)
        {
            query = query.Where(p => p.Year == year.Value);
        }

        return await query.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).ToListAsync();
    }

    public async Task<decimal> GetTotalSalaryByBranchAsync(long branchId, int month, int year)
    {
        return await _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.Employee.BranchId == branchId && p.Month == month && p.Year == year)
            .SumAsync(p => p.NetSalary ?? 0);
    }

    public async Task<bool> ExistsPayrollAsync(long employeeId, int month, int year)
    {
        return await _context.Payrolls
            .AnyAsync(p => p.EmployeeId == employeeId && p.Month == month && p.Year == year);
    }
}

public interface IEmployeeSalaryRepository : IRepository<EmployeeSalary>
{
    Task<EmployeeSalary?> GetCurrentSalaryAsync(long employeeId);
    Task<List<EmployeeSalary>> GetSalaryHistoryAsync(long employeeId);
    Task<EmployeeSalary?> GetSalaryByDateAsync(long employeeId, DateTime date);
}

public class EmployeeSalaryRepository : Repository<EmployeeSalary>, IEmployeeSalaryRepository
{
    public EmployeeSalaryRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<EmployeeSalary?> GetCurrentSalaryAsync(long employeeId)
    {
        return await _context.EmployeeSalaries
            .Include(s => s.Employee)
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.EffectiveDate)
            .FirstOrDefaultAsync();
    }

    public async Task<List<EmployeeSalary>> GetSalaryHistoryAsync(long employeeId)
    {
        return await _context.EmployeeSalaries
            .Include(s => s.Employee)
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.EffectiveDate)
            .ToListAsync();
    }

    public async Task<EmployeeSalary?> GetSalaryByDateAsync(long employeeId, DateTime date)
    {
        return await _context.EmployeeSalaries
            .Include(s => s.Employee)
            .Where(s => s.EmployeeId == employeeId && s.EffectiveDate <= date)
            .OrderByDescending(s => s.EffectiveDate)
            .FirstOrDefaultAsync();
    }
}
