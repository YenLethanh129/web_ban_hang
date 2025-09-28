namespace Dashboard.BussinessLogic.Dtos.EmployeeDtos;

public class EmployeeDto
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string? Status { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? ResignDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}

public class EmployeeSummaryDto
{
    public long TotalEmployees { get; set; }
    public long ActiveEmployees { get; set; }
    public long InactiveEmployees { get; set; }
    public DateTime AsOfDate { get; set; }
}

public class EmployeeDetailDto : EmployeeDto
{
    public decimal? BaseSalary { get; set; }
    public string? SalaryType { get; set; }
    public decimal? Allowance { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? TaxRate { get; set; }
    public DateTime? SalaryEffectiveDate { get; set; }
    public string BranchName { get; set; } = null!;
    public string BranchLocation { get; set; } = null!;
}

public class CreateEmployeeInput
{
    public long BranchId { get; set; }
    public long PositionId { get; set; }
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime HireDate { get; set; }
}
public class UpdateEmployeeInput
{
    public long EmployeeId { get; set; }
    public long? BranchId { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public long? PositionId { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? ResignDate { get; set; }
    public string? Status { get; set; }
    public decimal? BaseSalary { get; set; }
    public string? SalaryType { get; set; }
}

public class GetEmployeesInput : DefaultInput
{
    public long? BranchId { get; set; }
    public string? Status { get; set; }
    public string? BranchName { get; set; }
    public long? PositionId { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? HiredAfter { get; set; }
    public DateTime? HiredBefore { get; set; }
    public bool? IsActive { get; set; }
}  

