using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services.EmployeeServices;

public interface IEmployeeShiftService
{
    Task<PagedList<EmployeeShiftDto>> GetShiftsAsync(GetEmployeeShiftsInput input);
    Task<EmployeeShiftDto?> GetShiftByIdAsync(long id);
    Task<EmployeeShiftDto> CreateShiftAsync(CreateEmployeeShiftInput input);
    Task<EmployeeShiftDto> UpdateShiftAsync(UpdateEmployeeShiftInput input);
    Task<bool> DeleteShiftAsync(long id);
    Task<List<EmployeeShiftSummaryDto>> GetShiftSummaryAsync(long? branchId, int month, int year);
    Task<List<ShiftScheduleDto>> GetShiftScheduleAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    Task<bool> CheckInAsync(long shiftId);
    Task<bool> CheckOutAsync(long shiftId);
    Task<List<EmployeeShiftDto>> GetUpcomingShiftsAsync(long employeeId, int days = 7);
}

public class EmployeeShiftService : BaseTransactionalService, IEmployeeShiftService
{
    private readonly IEmployeeShiftRepository _shiftRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeShiftService> _logger;

    public EmployeeShiftService(
        IUnitOfWork unitOfWork,
        IEmployeeShiftRepository shiftRepository,
        IRepository<Employee> employeeRepository,
        IMapper mapper,
        ILogger<EmployeeShiftService> logger)
        : base(unitOfWork)
    {
        _shiftRepository = shiftRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<EmployeeShiftDto>> GetShiftsAsync(GetEmployeeShiftsInput input)
    {
        try
        {
            var shifts = await _shiftRepository.GetShiftsByDateRangeAsync(
                input.FromDate ?? DateTime.Now.AddDays(-30),
                input.ToDate ?? DateTime.Now.AddDays(30),
                input.EmployeeId,
                input.BranchId);

            if (!string.IsNullOrEmpty(input.Status))
            {
                shifts = shifts.Where(s => s.Status == input.Status).ToList();
            }

            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                shifts = shifts.Where(s => s.Employee.FullName.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalRecords = shifts.Count;
            var pagedShifts = shifts
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var shiftDtos = _mapper.Map<List<EmployeeShiftDto>>(pagedShifts);
            
            return new PagedList<EmployeeShiftDto>
            {
                Items = shiftDtos,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee shifts");
            throw;
        }
    }

    public async Task<EmployeeShiftDto?> GetShiftByIdAsync(long id)
    {
        try
        {
            var shift = await _shiftRepository.GetAsync(id);
            return shift != null ? _mapper.Map<EmployeeShiftDto>(shift) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shift by id {Id}", id);
            throw;
        }
    }

    public async Task<EmployeeShiftDto> CreateShiftAsync(CreateEmployeeShiftInput input)
    {
        try
        {
            // Validate employee exists
            var employee = await _unitOfWork.Repository<Employee>().GetAsync(input.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại");
            }

            // Check for conflicting shifts
            var hasConflict = await _shiftRepository.HasConflictingShiftAsync(
                input.EmployeeId, input.ShiftDate, input.StartTime, input.EndTime);

            if (hasConflict)
            {
                throw new InvalidOperationException("Ca làm việc bị trùng với ca khác của nhân viên");
            }

            // Validate shift time
            if (input.EndTime <= input.StartTime)
            {
                throw new ArgumentException("Thời gian kết thúc phải sau thời gian bắt đầu");
            }

            var shift = _mapper.Map<EmployeeShift>(input);
            await _shiftRepository.AddAsync(shift);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created shift for employee {EmployeeId} on {Date}", input.EmployeeId, input.ShiftDate);

            return _mapper.Map<EmployeeShiftDto>(shift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee shift");
            throw;
        }
    }

    public async Task<EmployeeShiftDto> UpdateShiftAsync(UpdateEmployeeShiftInput input)
    {
        try
        {
            var shift = await _shiftRepository.GetAsync(input.Id);
            if (shift == null)
            {
                throw new ArgumentException("Ca làm việc không tồn tại");
            }

            // Check for conflicting shifts if time is being updated
            if (input.ShiftDate.HasValue || input.StartTime.HasValue || input.EndTime.HasValue)
            {
                var shiftDate = input.ShiftDate ?? shift.ShiftDate;
                var startTime = input.StartTime ?? shift.StartTime;
                var endTime = input.EndTime ?? shift.EndTime;

                if (endTime <= startTime)
                {
                    throw new ArgumentException("Thời gian kết thúc phải sau thời gian bắt đầu");
                }

                var hasConflict = await _shiftRepository.HasConflictingShiftAsync(
                    shift.EmployeeId, shiftDate, startTime, endTime, shift.Id);

                if (hasConflict)
                {
                    throw new InvalidOperationException("Ca làm việc bị trùng với ca khác của nhân viên");
                }
            }

            _mapper.Map(input, shift);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Updated shift {ShiftId}", input.Id);

            return _mapper.Map<EmployeeShiftDto>(shift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee shift {ShiftId}", input.Id);
            throw;
        }
    }

    public async Task<bool> DeleteShiftAsync(long id)
    {
        try
        {
            var shift = await _shiftRepository.GetAsync(id);
            if (shift == null)
            {
                return false;
            }

            // Don't allow deletion of completed shifts
            if (shift.Status == "COMPLETED" || shift.Status == "CHECKED_OUT")
            {
                throw new InvalidOperationException("Không thể xóa ca làm việc đã hoàn thành");
            }

            _shiftRepository.Remove(shift);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Deleted shift {ShiftId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee shift {ShiftId}", id);
            throw;
        }
    }

    public async Task<List<EmployeeShiftSummaryDto>> GetShiftSummaryAsync(long? branchId, int month, int year)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var shifts = await _shiftRepository.GetShiftsByDateRangeAsync(startDate, endDate, null, branchId);

            var summary = shifts
                .GroupBy(s => new { s.EmployeeId, s.Employee.FullName })
                .Select(g => new EmployeeShiftSummaryDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.FullName,
                    TotalShifts = g.Count(),
                    WorkingDays = g.Select(s => s.ShiftDate).Distinct().Count(),
                    TotalWorkingHours = g.Sum(s => CalculateWorkingHours(s.StartTime, s.EndTime)),
                    AverageHoursPerDay = g.Select(s => s.ShiftDate).Distinct().Count() > 0 
                        ? g.Sum(s => CalculateWorkingHours(s.StartTime, s.EndTime)) / g.Select(s => s.ShiftDate).Distinct().Count()
                        : 0
                })
                .OrderBy(s => s.EmployeeName)
                .ToList();

            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shift summary");
            throw;
        }
    }

    public async Task<List<ShiftScheduleDto>> GetShiftScheduleAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        try
        {
            var shifts = await _shiftRepository.GetShiftsByDateRangeAsync(fromDate, toDate, null, branchId);
            var shiftDtos = _mapper.Map<List<EmployeeShiftDto>>(shifts);

            var schedule = shiftDtos
                .GroupBy(s => s.ShiftDate)
                .Select(g => new ShiftScheduleDto
                {
                    Date = g.Key,
                    Shifts = g.OrderBy(s => s.StartTime).ToList(),
                    TotalEmployees = g.Count(),
                    TotalHours = g.Sum(s => s.WorkingHours)
                })
                .OrderBy(s => s.Date)
                .ToList();

            return schedule;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shift schedule");
            throw;
        }
    }

    public async Task<bool> CheckInAsync(long shiftId)
    {
        try
        {
            var shift = await _shiftRepository.GetAsync(shiftId);
            if (shift == null)
            {
                return false;
            }

            if (shift.Status != "SCHEDULED")
            {
                throw new InvalidOperationException("Chỉ có thể check-in ca làm việc đã được lên lịch");
            }

            shift.Status = "CHECKED_IN";
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee checked in for shift {ShiftId}", shiftId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking in for shift {ShiftId}", shiftId);
            throw;
        }
    }

    public async Task<bool> CheckOutAsync(long shiftId)
    {
        try
        {
            var shift = await _shiftRepository.GetAsync(shiftId);
            if (shift == null)
            {
                return false;
            }

            if (shift.Status != "CHECKED_IN")
            {
                throw new InvalidOperationException("Chỉ có thể check-out sau khi đã check-in");
            }

            shift.Status = "CHECKED_OUT";
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee checked out for shift {ShiftId}", shiftId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking out for shift {ShiftId}", shiftId);
            throw;
        }
    }

    public async Task<List<EmployeeShiftDto>> GetUpcomingShiftsAsync(long employeeId, int days = 7)
    {
        try
        {
            var fromDate = DateTime.Now;
            var toDate = fromDate.AddDays(days);

            var shifts = await _shiftRepository.GetShiftsByDateRangeAsync(fromDate, toDate, employeeId);
            return _mapper.Map<List<EmployeeShiftDto>>(shifts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming shifts for employee {EmployeeId}", employeeId);
            throw;
        }
    }

    private static decimal CalculateWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        var duration = endTime.ToTimeSpan() - startTime.ToTimeSpan();
        if (duration.TotalHours < 0)
        {
            duration = duration.Add(TimeSpan.FromDays(1));
        }
        return (decimal)duration.TotalHours;
    }
}
