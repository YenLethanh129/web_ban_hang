using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Dashboard.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IEmployeeShiftRepository : IRepository<EmployeeShift>
{
    Task<List<EmployeeShift>> GetShiftsByDateRangeAsync(DateOnly fromDate, DateOnly toDate, long? employeeId = null, long? branchId = null);
    Task<List<EmployeeShift>> GetShiftsByEmployeeAndMonthAsync(long employeeId, int month, int year);
    Task<decimal> GetTotalWorkingHoursAsync(long employeeId, int month, int year);
    Task<bool> HasConflictingShiftAsync(long employeeId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime, long? excludeShiftId = null);
    Task<List<EmployeeShift>> GetShiftsByStatusAsync(string status);
}

public class EmployeeShiftRepository : Repository<EmployeeShift>, IEmployeeShiftRepository
{
    public EmployeeShiftRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<List<EmployeeShift>> GetShiftsByDateRangeAsync(DateOnly fromDate, DateOnly toDate, long? employeeId = null, long? branchId = null)
    {
        var query = _context.EmployeeShifts
            .Include(s => s.Employee)
            .ThenInclude(e => e.Branch)
            .Where(s => s.ShiftDate >= fromDate && s.ShiftDate <= toDate);

        if (employeeId.HasValue)
        {
            query = query.Where(s => s.EmployeeId == employeeId.Value);
        }

        if (branchId.HasValue)
        {
            query = query.Where(s => s.Employee.BranchId == branchId.Value);
        }

        return await query.OrderBy(s => s.ShiftDate).ThenBy(s => s.StartTime).ToListAsync();
    }

    public async Task<List<EmployeeShift>> GetShiftsByEmployeeAndMonthAsync(long employeeId, int month, int year)
    {
        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        return await _context.EmployeeShifts
            .Include(s => s.Employee)
            .Where(s => s.EmployeeId == employeeId && 
                       s.ShiftDate >= startDate && 
                       s.ShiftDate <= endDate)
            .OrderBy(s => s.ShiftDate)
            .ThenBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalWorkingHoursAsync(long employeeId, int month, int year)
    {
        var shifts = await GetShiftsByEmployeeAndMonthAsync(employeeId, month, year);
        
        return shifts
            .Where(s => s.Status == "COMPLETED" || s.Status == "CHECKED_OUT")
            .Sum(s => CalculateWorkingHours(s.StartTime, s.EndTime));
    }

    public async Task<bool> HasConflictingShiftAsync(long employeeId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime, long? excludeShiftId = null)
    {
        var query = _context.EmployeeShifts
            .Where(s => s.EmployeeId == employeeId && 
                       s.ShiftDate == shiftDate &&
                       s.Status != "CANCELLED" &&
                       ((s.StartTime <= startTime && s.EndTime > startTime) ||
                        (s.StartTime < endTime && s.EndTime >= endTime) ||
                        (s.StartTime >= startTime && s.EndTime <= endTime)));

        if (excludeShiftId.HasValue)
        {
            query = query.Where(s => s.Id != excludeShiftId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<List<EmployeeShift>> GetShiftsByStatusAsync(string status)
    {
        return await _context.EmployeeShifts
            .Include(s => s.Employee)
            .ThenInclude(e => e.Branch)
            .Where(s => s.Status == status)
            .OrderBy(s => s.ShiftDate)
            .ThenBy(s => s.StartTime)
            .ToListAsync();
    }

    private static decimal CalculateWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        var duration = endTime.ToTimeSpan() - startTime.ToTimeSpan();
        if (duration.TotalHours < 0)
        {
            // Handle shifts that cross midnight
            duration = duration.Add(TimeSpan.FromDays(1));
        }
        return (decimal)duration.TotalHours;
    }
}
