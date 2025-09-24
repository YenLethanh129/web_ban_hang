using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Specification;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Dashboard.Winform.ViewModels.ScheduleModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Winform.Presenters
{
    public interface IScheduleManagementPresenter : IManagementPresenter<ScheduleManagementModel>, IDisposable
    {
        Task LoadSchedulesAsync(DateTime? startDate = null, DateTime? endDate = null, long? employeeId = null);
        Task LoadWeeklyScheduleAsync(DateTime weekStart);
        Task AddScheduleAsync(long employeeId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, string status = "SCHEDULED");
        Task UpdateScheduleAsync(long scheduleId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, string status);
        Task DeleteScheduleAsync(long scheduleId);
        Task GetAvailableEmployeesAsync();
        Task FilterByEmployeeAsync(long? employeeId);
        Task FilterByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task FilterByStatusAsync(string status);
        Task<WeeklyScheduleViewModel> GetWeeklyScheduleAsync(DateTime weekStart);
        Task<bool> CheckScheduleConflictAsync(long employeeId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, long? excludeScheduleId = null);
        Task<List<GanttTaskViewModel>> GetGanttDataAsync(DateTime startDate, DateTime endDate);
        Task<ScheduleStatisticsViewModel> GetScheduleStatisticsAsync(DateTime startDate, DateTime endDate);
    }

    public class ScheduleManagementPresenter : IScheduleManagementPresenter
    {
        private readonly IEmployeeShiftService _employeeShiftService;
        private readonly IEmployeeManagementService _employeeManagementService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isLoading = false;

        // Cache properties
        private List<EmployeeScheduleViewModel> _allSchedulesCache = new();
        private List<EmployeeScheduleViewModel> _filteredSchedules = new();
        private string _currentStatusFilter = "All";
        private long? _currentEmployeeFilter = null;
        private DateTime? _currentStartDate = null;
        private DateTime? _currentEndDate = null;

        public ScheduleManagementModel Model { get; }
        IManagableModel IManagementPresenter<ScheduleManagementModel>.Model
        {
            get => Model;
            set => throw new NotImplementedException();
        }

        public event EventHandler? OnDataLoaded;

        public ScheduleManagementPresenter(
            IEmployeeShiftService employeeShiftService,
            IEmployeeManagementService employeeManagementService,
            IUnitOfWork unitOfWork)
        {
            _employeeShiftService = employeeShiftService;
            _employeeManagementService = employeeManagementService;
            _unitOfWork = unitOfWork;
            Model = new ScheduleManagementModel();
        }

        public async Task LoadDataAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            await LoadSchedulesAsync(startOfWeek, startOfWeek.AddDays(6));
        }

        public async Task LoadSchedulesAsync(DateTime? startDate = null, DateTime? endDate = null, long? employeeId = null)
        {
            if (_isLoading) return;

            await _semaphore.WaitAsync();
            try
            {
                _isLoading = true;

                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today.AddDays(30);

                await LoadSchedulesToCache(start, end);
                await GetAvailableEmployeesAsync();

                _currentEmployeeFilter = employeeId;
                _currentStartDate = start;
                _currentEndDate = end;
                _currentStatusFilter = "All";

                ApplyFilters();
            }
            finally
            {
                _isLoading = false;
                _semaphore.Release();
            }
        }

        public async Task LoadWeeklyScheduleAsync(DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);
            await LoadSchedulesAsync(weekStart, weekEnd);
        }

        private async Task LoadSchedulesToCache(DateTime startDate, DateTime endDate)
        {
            var shifts = await _unitOfWork.Repository<EmployeeShift>()
                .GetQueryable(asNoTracking: true)
                .Include(s => s.Employee)
                .ThenInclude(e => e.Position)
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .Where(s => s.Employee.Position.NeedSchedule)
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

            _allSchedulesCache = shifts.Select(shift => new EmployeeScheduleViewModel
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                EmployeeName = shift.Employee.FullName,
                PositionName = shift.Employee.Position.Name,
                ShiftDate = shift.ShiftDate,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                Status = shift.Status ?? "SCHEDULED"
            }).ToList();
        }

        public async Task GetAvailableEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetQueryable(asNoTracking: true)
                .Include(e => e.Position)
                .Where(e => e.Position.NeedSchedule && e.Status == "ACTIVE")
                .OrderBy(e => e.FullName)
                .ToListAsync();

            Model.AvailableEmployees.Clear();

            // Add "All" option for filtering
            Model.AvailableEmployees.Add(new EmployeeViewModel
            {
                Id = 0,
                FullName = "Tất cả nhân viên",
                PositionName = ""
            });

            foreach (var emp in employees)
            {
                Model.AvailableEmployees.Add(new EmployeeViewModel
                {
                    Id = emp.Id,
                    FullName = emp.FullName,
                    PositionName = emp.Position.Name,
                    PositionId = emp.PositionId
                });
            }
        }

        private void ApplyFilters()
        {
            var query = _allSchedulesCache.AsQueryable();

            if (_currentEmployeeFilter.HasValue && _currentEmployeeFilter > 0)
            {
                query = query.Where(s => s.EmployeeId == _currentEmployeeFilter.Value);
            }

            if (_currentStatusFilter != "All")
            {
                query = query.Where(s => s.Status == _currentStatusFilter);
            }

            if (!string.IsNullOrWhiteSpace(Model.SearchText))
            {
                query = query.Where(s =>
                    s.EmployeeName.Contains(Model.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.PositionName.Contains(Model.SearchText, StringComparison.OrdinalIgnoreCase));
            }

            _filteredSchedules = query.OrderBy(s => s.ShiftDate).ThenBy(s => s.StartTime).ToList();

            UpdateModelWithPagination();
            var currentPageSchedules = GetCurrentPageSchedules();

            Model.Schedules.Clear();
            foreach (var schedule in currentPageSchedules)
            {
                Model.Schedules.Add(schedule);
            }

            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateModelWithPagination()
        {
            Model.TotalItems = _filteredSchedules.Count;

            if (Model.CurrentPage < 1) Model.CurrentPage = 1;
            if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
                Model.CurrentPage = Model.TotalPages;
        }

        private List<EmployeeScheduleViewModel> GetCurrentPageSchedules()
        {
            int skip = (Model.CurrentPage - 1) * Model.PageSize;
            return _filteredSchedules.Skip(skip).Take(Model.PageSize).ToList();
        }

        public Task<WeeklyScheduleViewModel> GetWeeklyScheduleAsync(DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);

            var schedules = _allSchedulesCache
                .Where(s => s.ShiftDate >= weekStart && s.ShiftDate <= weekEnd)
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.StartTime)
                .ToList();

            var weeklySchedule = new WeeklyScheduleViewModel
            {
                WeekStart = weekStart,
                WeekEnd = weekEnd
            };

            for (int i = 0; i < 7; i++)
            {
                var currentDate = weekStart.AddDays(i);
                var daySchedules = schedules.Where(s => s.ShiftDate.Date == currentDate.Date).ToList();

                weeklySchedule.Days.Add(new DayScheduleViewModel
                {
                    Date = currentDate,
                    DayName = currentDate.ToString("dddd", new System.Globalization.CultureInfo("vi-VN")),
                    Schedules = daySchedules
                });
            }

            return Task.FromResult(weeklySchedule);
        }

        public async Task<List<GanttTaskViewModel>> GetGanttDataAsync(DateTime startDate, DateTime endDate)
        {
            var schedules = _allSchedulesCache
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .GroupBy(s => s.EmployeeId)
                .ToList();

            var ganttTasks = new List<GanttTaskViewModel>();

            foreach (var employeeGroup in schedules)
            {
                var employee = employeeGroup.First();
                var tasks = employeeGroup.OrderBy(s => s.ShiftDate).Select(s => new GanttTaskViewModel
                {
                    Id = s.Id,
                    Name = $"{s.EmployeeName} - {s.ShiftDuration}",
                    StartDate = s.ShiftDate.Add(s.StartTime.ToTimeSpan()),
                    EndDate = s.ShiftDate.Add(s.EndTime.ToTimeSpan()),
                    Progress = GetProgressByStatus(s.Status),
                    Color = GetColorByStatus(s.Status),
                    EmployeeId = s.EmployeeId,
                    EmployeeName = s.EmployeeName,
                    Status = s.Status
                }).ToList();

                ganttTasks.AddRange(tasks);
            }

            return await Task.FromResult(ganttTasks.OrderBy(t => t.EmployeeName).ThenBy(t => t.StartDate).ToList());
        }

        public Task<ScheduleStatisticsViewModel> GetScheduleStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            
            var schedules = _allSchedulesCache
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .ToList();

            return Task.FromResult(new ScheduleStatisticsViewModel
            {
                TotalSchedules = schedules.Count,
                ScheduledCount = schedules.Count(s => s.Status == "SCHEDULED"),
                CompletedCount = schedules.Count(s => s.Status == "COMPLETED"),
                AbsentCount = schedules.Count(s => s.Status == "ABSENT"),
                CancelledCount = schedules.Count(s => s.Status == "CANCELLED"),
                TotalEmployees = schedules.Select(s => s.EmployeeId).Distinct().Count(),
                TotalWorkingHours = schedules.Sum(s => (s.EndTime - s.StartTime).TotalHours),
                AverageShiftDuration = schedules.Any() ?
                    schedules.Average(s => (s.EndTime - s.StartTime).TotalHours) : 0
            });
        }

        private int GetProgressByStatus(string status)
        {
            return status switch
            {
                "COMPLETED" => 100,
                "SCHEDULED" => 0,
                "ABSENT" => 0,
                "CANCELLED" => 0,
                _ => 0
            };
        }

        private string GetColorByStatus(string status)
        {
            return status switch
            {
                "SCHEDULED" => "#0078D4",
                "COMPLETED" => "#4CAF50",
                "ABSENT" => "#F44336",
                "CANCELLED" => "#9E9E9E",
                _ => "#6C757D"
            };
        }

        public async Task AddScheduleAsync(long employeeId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, string status = "SCHEDULED")
        {
            // Check for conflicts
            if (await CheckScheduleConflictAsync(employeeId, shiftDate, startTime, endTime))
            {
                throw new InvalidOperationException("Nhân viên đã có lịch làm việc trong khoảng thời gian này.");
            }

            var shift = new EmployeeShift
            {
                EmployeeId = employeeId,
                ShiftDate = shiftDate,
                StartTime = startTime,
                EndTime = endTime,
                Status = status
            };

            await _unitOfWork.Repository<EmployeeShift>().AddAsync(shift);
            await _unitOfWork.SaveChangesAsync();

            // Refresh cache
            await LoadSchedulesToCache(_currentStartDate ?? DateTime.Today.AddDays(-30),
                                     _currentEndDate ?? DateTime.Today.AddDays(30));
            ApplyFilters();
        }

        public async Task UpdateScheduleAsync(long scheduleId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, string status)
        {
            var shift = await _unitOfWork.Repository<EmployeeShift>().GetAsync(scheduleId);
            if (shift == null)
                throw new ArgumentException("Không tìm thấy lịch làm việc.");

            // Check for conflicts (excluding current schedule)
            if (await CheckScheduleConflictAsync(shift.EmployeeId, shiftDate, startTime, endTime, scheduleId))
            {
                throw new InvalidOperationException("Nhân viên đã có lịch làm việc trong khoảng thời gian này.");
            }

            shift.ShiftDate = shiftDate;
            shift.StartTime = startTime;
            shift.EndTime = endTime;
            shift.Status = status;

            _unitOfWork.Repository<EmployeeShift>().Update(shift);
            await _unitOfWork.SaveChangesAsync();

            // Refresh cache
            await LoadSchedulesToCache(_currentStartDate ?? DateTime.Today.AddDays(-30),
                                     _currentEndDate ?? DateTime.Today.AddDays(30));
            ApplyFilters();
        }

        public async Task DeleteScheduleAsync(long scheduleId)
        {
            var shift = await _unitOfWork.Repository<EmployeeShift>().GetAsync(scheduleId);
            if (shift != null)
            {
                _unitOfWork.Repository<EmployeeShift>().Remove(shift);
                await _unitOfWork.SaveChangesAsync();

                // Refresh cache
                await LoadSchedulesToCache(_currentStartDate ?? DateTime.Today.AddDays(-30),
                                         _currentEndDate ?? DateTime.Today.AddDays(30));
                ApplyFilters();
            }
        }

        public async Task<bool> CheckScheduleConflictAsync(long employeeId, DateTime shiftDate, TimeOnly startTime, TimeOnly endTime, long? excludeScheduleId = null)
        {
            var existingShifts = await _unitOfWork.Repository<EmployeeShift>()
                .GetQueryable()
                .Where(s => s.EmployeeId == employeeId && s.ShiftDate.Date == shiftDate.Date)
                .Where(s => excludeScheduleId == null || s.Id != excludeScheduleId.Value)
                .ToListAsync();

            return existingShifts.Any(s =>
                (startTime >= s.StartTime && startTime < s.EndTime) ||
                (endTime > s.StartTime && endTime <= s.EndTime) ||
                (startTime <= s.StartTime && endTime >= s.EndTime));
        }

        public async Task FilterByEmployeeAsync(long? employeeId)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentEmployeeFilter = employeeId == 0 ? null : employeeId; // 0 means "All"
                Model.CurrentPage = 1;
                ApplyFilters();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task FilterByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentStartDate = startDate;
                _currentEndDate = endDate;

                await LoadSchedulesToCache(startDate, endDate);
                Model.CurrentPage = 1;
                ApplyFilters();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task FilterByStatusAsync(string status)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentStatusFilter = status ?? "All";
                Model.CurrentPage = 1;
                ApplyFilters();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
}