using Dashboard.Winform.ViewModels.EmployeeModels;
using System.ComponentModel;

namespace Dashboard.Winform.ViewModels.ScheduleModels
{
    public class ScheduleManagementModel : IManagableModel
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private DateTime _selectedDate = DateTime.Today;
        private long? _selectedEmployeeId;
        private string _searchText = string.Empty;
        private BindingList<EmployeeScheduleViewModel> _schedules = new();
        private BindingList<EmployeeViewModel> _availableEmployees = new();
        private BindingList<string> _shiftStatuses = ["All", "SCHEDULED", "COMPLETED", "ABSENT", "CANCELLED"];
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }

        public long? SelectedEmployeeId
        {
            get => _selectedEmployeeId;
            set
            {
                if (_selectedEmployeeId != value)
                {
                    _selectedEmployeeId = value;
                    OnPropertyChanged(nameof(SelectedEmployeeId));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public BindingList<EmployeeScheduleViewModel> Schedules
        {
            get => _schedules;
            set
            {
                if (_schedules != value)
                {
                    _schedules = value;
                    OnPropertyChanged(nameof(Schedules));
                }
            }
        }

        public BindingList<EmployeeViewModel> AvailableEmployees
        {
            get => _availableEmployees;
            set
            {
                if (_availableEmployees != value)
                {
                    _availableEmployees = value;
                    OnPropertyChanged(nameof(AvailableEmployees));
                }
            }
        }

        public BindingList<string> ShiftStatuses
        {
            get => _shiftStatuses;
            set
            {
                if (_shiftStatuses != value)
                {
                    _shiftStatuses = value;
                    OnPropertyChanged(nameof(ShiftStatuses));
                }
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                if (_totalItems != value)
                {
                    _totalItems = value;
                    OnPropertyChanged(nameof(TotalItems));
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
        }

        public int TotalPages => _totalItems == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                    OnPropertyChanged(nameof(TotalPages));
                    CurrentPage = 1;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmployeeScheduleViewModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; } = "SCHEDULED";
        public string ShiftDuration => $"{StartTime:HH:mm} - {EndTime:HH:mm}";
        public string FormattedDate => ShiftDate.ToString("dd/MM/yyyy");
        public string WeekDay => ShiftDate.ToString("dddd", new System.Globalization.CultureInfo("vi-VN"));
        public double DurationInHours => (EndTime - StartTime).TotalHours;
        public string StatusDisplayText => GetStatusDisplayText(Status);
        public Color StatusColor => GetStatusColor(Status);

        private string GetStatusDisplayText(string status)
        {
            return status switch
            {
                "SCHEDULED" => "Đã lên lịch",
                "COMPLETED" => "Hoàn thành",
                "ABSENT" => "Vắng mặt",
                "CANCELLED" => "Đã hủy",
                _ => status
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "SCHEDULED" => Color.FromArgb(0, 120, 215),
                "COMPLETED" => Color.FromArgb(76, 175, 80),
                "ABSENT" => Color.FromArgb(244, 67, 54),
                "CANCELLED" => Color.FromArgb(158, 158, 158),
                _ => Color.FromArgb(73, 75, 111)
            };
        }
    }

    public class WeeklyScheduleViewModel
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public List<DayScheduleViewModel> Days { get; set; } = new();
        public string WeekRange => $"{WeekStart:dd/MM} - {WeekEnd:dd/MM/yyyy}";
        public int TotalSchedules => Days.Sum(d => d.Schedules.Count);
        public double TotalWorkingHours => Days.Sum(d => d.TotalHours);
    }

    public class DayScheduleViewModel
    {
        public DateTime Date { get; set; }
        public string DayName { get; set; } = string.Empty;
        public List<EmployeeScheduleViewModel> Schedules { get; set; } = new();
        public bool IsToday => Date.Date == DateTime.Today;
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
        public int ScheduleCount => Schedules.Count;
        public double TotalHours => Schedules.Sum(s => s.DurationInHours);
        public string FormattedDate => Date.ToString("dd/MM");
        public string FullDayName => Date.ToString("dddd, dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));
    }

    public class GanttTaskViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Progress { get; set; }
        public string Color { get; set; } = "#0078D4";
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public TimeSpan Duration => EndDate - StartDate;
        public string FormattedDuration => $"{Duration.TotalHours:F1}h";
        public Rectangle Tag { get; set; }
    }

    public class ScheduleStatisticsViewModel
    {
        public int TotalSchedules { get; set; }
        public int ScheduledCount { get; set; }
        public int CompletedCount { get; set; }
        public int AbsentCount { get; set; }
        public int CancelledCount { get; set; }
        public int TotalEmployees { get; set; }
        public double TotalWorkingHours { get; set; }
        public double AverageShiftDuration { get; set; }

        public double CompletionRate => TotalSchedules > 0 ? (double)CompletedCount / TotalSchedules * 100 : 0;
        public double AbsentRate => TotalSchedules > 0 ? (double)AbsentCount / TotalSchedules * 100 : 0;
        public string FormattedTotalHours => $"{TotalWorkingHours:F1} giờ";
        public string FormattedAverageDuration => $"{AverageShiftDuration:F1} giờ/ca";
    }

    public class ScheduleConflictViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime ConflictDate { get; set; }
        public TimeOnly ConflictStartTime { get; set; }
        public TimeOnly ConflictEndTime { get; set; }
        public List<EmployeeScheduleViewModel> ConflictingSchedules { get; set; } = new();
        public string ConflictDescription => $"Xung đột lịch làm việc của {EmployeeName} vào {ConflictDate:dd/MM/yyyy}";
    }

    public class ScheduleTemplateViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ScheduleTemplateItemViewModel> Items { get; set; } = new();
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class ScheduleTemplateItemViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string DayName => DayOfWeek.ToString("G");
        public string TimeRange => $"{StartTime:HH:mm} - {EndTime:HH:mm}";
    }

    public class EmployeeWorkloadViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public int TotalSchedules { get; set; }
        public double TotalHours { get; set; }
        public double AverageHoursPerDay { get; set; }
        public int CompletedShifts { get; set; }
        public int AbsentShifts { get; set; }
        public double AttendanceRate => TotalSchedules > 0 ? (double)CompletedShifts / TotalSchedules * 100 : 0;
        public string FormattedTotalHours => $"{TotalHours:F1}h";
        public string FormattedAttendanceRate => $"{AttendanceRate:F1}%";
        public Color WorkloadColor => GetWorkloadColor(TotalHours);

        private Color GetWorkloadColor(double hours)
        {
            if (hours >= 40) return Color.FromArgb(244, 67, 54); // Red - Overloaded
            if (hours >= 30) return Color.FromArgb(255, 193, 7); // Yellow - High
            if (hours >= 20) return Color.FromArgb(76, 175, 80); // Green - Normal
            return Color.FromArgb(158, 158, 158); // Gray - Low
        }
    }
}