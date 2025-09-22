using System;
using System.ComponentModel;

namespace Dashboard.Winform.ViewModels.EmployeeModels
{
    public class EmployeeDetailViewModel : ViewModelBase
    {

        public List<string> Statuses { get; set; } = [];
        #region Basic Employee Information

        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string Status { get; set; } = "ACTIVE";

        #endregion

        #region Branch Information
        private List<BranchViewModel> _existingBranches = [];
        public List<BranchViewModel> ExistingBranches
        {
            get => _existingBranches;
            set
            {
                _existingBranches = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ExistingBranches));
            }
        }

        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchAddress { get; set; } = string.Empty;
        public string BranchPhone { get; set; } = string.Empty;
        public string BranchManager { get; set; } = string.Empty;

        #endregion

        #region Position Information
        private List<PositionViewModel> _existingPositions = [];
        public List<PositionViewModel> ExistingPositions
        {
            get => _existingPositions;
            set
            {
                _existingPositions = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ExistingPositions));
            }
        }
        public long PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;

        #endregion

        #region Audit Information

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Account Information

        public bool HasAccount { get; set; }
        public string Role { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;

        #endregion

        #region Related Data Collections

        private BindingList<EmployeeSalaryViewModel> _salaries = new();
        public BindingList<EmployeeSalaryViewModel> Salaries
        {
            get => _salaries;
            set => SetProperty(ref _salaries, value);
        }

        private BindingList<PayrollViewModel> _payrolls = new();
        public BindingList<PayrollViewModel> Payrolls
        {
            get => _payrolls;
            set
            {
                _payrolls = value;
                OnPropertyChanged();
            }
        }
        public BindingList<EmployeeShiftViewModel> Shifts { get; set; } = new();

        #endregion

        #region Helper Properties
        public int TotalSalaryRecords => Salaries?.Count ?? 0;

        public int TotalPayrollRecords => Payrolls?.Count ?? 0;

        public decimal? CurrentBaseSalary
        {
            get
            {
                if (Salaries == null || Salaries.Count == 0)
                    return null;

                EmployeeSalaryViewModel? latestSalary = null;
                foreach (var salary in Salaries)
                {
                    if (latestSalary == null || salary.EffectiveDate > latestSalary.EffectiveDate)
                        latestSalary = salary;
                }

                return latestSalary?.BaseSalary;
            }
        }

        public bool IsActive => Status?.ToUpper() == "ACTIVE";

        public bool IsResigned => ResignDate.HasValue && ResignDate.Value <= DateTime.Today;

        public TimeSpan? WorkingPeriod
        {
            get
            {
                if (!HireDate.HasValue) return null;

                var endDate = ResignDate.HasValue && ResignDate.Value <= DateTime.Today
                    ? ResignDate.Value
                    : DateTime.Today;

                return endDate - HireDate.Value;
            }
        }
        public int WorkingYears => WorkingPeriod?.Days / 365 ?? 0;

        #endregion
    }
    public class EmployeeSalaryViewModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public string SalaryType { get; set; } = string.Empty;
        public decimal? Allowance { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? TaxRate { get; set; }
        public DateTime EffectiveDate { get; set; }

        #region Calculated Properties

        public decimal GrossSalary => BaseSalary + (Allowance ?? 0) + (Bonus ?? 0) - (Penalty ?? 0);

        public decimal TaxAmount => GrossSalary * ((TaxRate ?? 0) / 100);

        public decimal NetSalary => GrossSalary - TaxAmount;

        public string SalaryTypeDisplay
        {
            get
            {
                return SalaryType!.ToUpper() switch
                {
                    "MONTHLY" => "Tháng",
                    "HOURLY" => "Giờ",
                    "DAILY" => "Ngày",
                    "WEEKLY" => "Tuần",
                    _ => SalaryType
                };
            }
        }

        #endregion
    }

    public class PayrollViewModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string PayrollMonth { get; set; } = string.Empty;
        public decimal WorkingHours { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal TotalSalary { get; set; }
        public DateTime PayrollDate { get; set; }
        //public string Status { get; set; } = string.Empty;

        #region Calculated Properties

        public decimal GrossAmount => BasicSalary + Allowances + Bonuses;

        public decimal NetAmount => GrossAmount - Deductions;

        //public bool IsPaid => Status?.ToUpper() == "PAID";

        public string MonthYearDisplay
        {
            get
            {
                if (DateTime.TryParse(PayrollMonth, out DateTime date))
                {
                    return date.ToString("MM/yyyy");
                }
                return PayrollMonth;
            }
        }

        #endregion
    }

    public class EmployeeShiftViewModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; } = string.Empty;

        #region Calculated Properties

        public double WorkingHours
        {
            get
            {
                var start = StartTime.ToTimeSpan();
                var end = EndTime.ToTimeSpan();

                if (end < start)
                {
                    return (TimeSpan.FromDays(1) - start + end).TotalHours;
                }

                return (end - start).TotalHours;
            }
        }

        public string WorkingHoursDisplay => $"{WorkingHours:F1}h";

        public string ShiftDisplay => $"{StartTime:HH:mm} - {EndTime:HH:mm}";

        public string StatusDisplay
        {
            get
            {
                return Status!.ToUpper() switch
                {
                    "COMPLETED" => "Hoàn thành",
                    "IN_PROGRESS" => "Đang làm",
                    "CANCELLED" => "Đã hủy",
                    "ABSENT" => "Vắng mặt",
                    _ => Status
                };
            }
        }

        /// <summary>
        /// Màu hiển thị trạng thái
        /// </summary>
        public Color StatusColor
        {
            get
            {
                return Status?.ToUpper() switch
                {
                    "COMPLETED" => Color.Green,
                    "IN_PROGRESS" => Color.Blue,
                    "CANCELLED" => Color.Orange,
                    "ABSENT" => Color.Red,
                    _ => Color.Gray
                };
            }
        }

        #endregion
    }

    /// <summary>
    /// ViewModel cho thông tin chi nhánh (dùng trong combo box)
    /// </summary>
    public class BranchViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string DisplayText => $"{Name} - {Address}";
    }
}