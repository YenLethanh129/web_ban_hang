namespace Dashboard.BussinessLogic.Dtos.EmployeeShiftDtos;

public class EmployeeShiftDto
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public DateOnly ShiftDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = "SCHEDULED";
    public decimal WorkingHours { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}

public class CreateEmployeeShiftInput
{
    public long EmployeeId { get; set; }
    public DateOnly ShiftDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = "SCHEDULED";
}

public class UpdateEmployeeShiftInput
{
    public long Id { get; set; }
    public DateOnly? ShiftDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Status { get; set; }
}

public class GetEmployeeShiftsInput : DefaultInput
{
    public long? EmployeeId { get; set; }
    public long? BranchId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string? Status { get; set; }
    public string? SearchTerm { get; set; }
}

public class EmployeeShiftSummaryDto
{
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int TotalShifts { get; set; }
    public decimal TotalWorkingHours { get; set; }
    public int WorkingDays { get; set; }
    public decimal AverageHoursPerDay { get; set; }
}

public class ShiftScheduleDto
{
    public DateOnly Date { get; set; }
    public List<EmployeeShiftDto> Shifts { get; set; } = new();
    public int TotalEmployees { get; set; }
    public decimal TotalHours { get; set; }
}
