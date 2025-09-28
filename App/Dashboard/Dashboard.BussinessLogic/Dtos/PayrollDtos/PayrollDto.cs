namespace Dashboard.BussinessLogic.Dtos.PayrollDtos;

public class PayrollDto
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal? TotalWorkingHours { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Bonus { get; set; }
    public decimal Penalty { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}

public class CreatePayrollInput
{
    public long EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? AdditionalAllowance { get; set; }
}

public class GetPayrollInput : DefaultInput
{
    public long? EmployeeId { get; set; }
    public long? BranchId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class PayrollSummaryDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public int TotalEmployees { get; set; }
    public decimal TotalGrossSalary { get; set; }
    public decimal TotalNetSalary { get; set; }
    public decimal TotalTax { get; set; }
    public decimal AverageSalary { get; set; }
    public List<PayrollDto> Payrolls { get; set; } = new();
}

public class EmployeeSalaryDto
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public string SalaryType { get; set; } = "MONTHLY";
    public decimal Allowance { get; set; }
    public decimal Bonus { get; set; }
    public decimal Penalty { get; set; }
    public decimal TaxRate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}

public class CreateEmployeeSalaryInput
{
    public long EmployeeId { get; set; }
    public decimal BaseSalary { get; set; }
    public string SalaryType { get; set; } = "MONTHLY";
    public decimal? Allowance { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? TaxRate { get; set; }
    public DateTime EffectiveDate { get; set; }
}

public class UpdateEmployeeSalaryInput
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public decimal? BaseSalary { get; set; }
    public string? SalaryType { get; set; }
    public decimal? Allowance { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? TaxRate { get; set; }
    public DateTime? EffectiveDate { get; set; }
}

public class UpdatePayrollInput
{
    public long Id { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? AdditionalAllowance { get; set; }
    public decimal? TaxRate { get; set; }
}

public class GetPayrollsInput : DefaultInput
{
    public long? EmployeeId { get; set; }
    public long? BranchId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SearchTerm { get; set; }
}

public class SalaryCalculationDto
{
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal TotalWorkingHours { get; set; }
    public decimal Allowance { get; set; }
    public decimal Bonus { get; set; }
    public decimal Penalty { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetSalary { get; set; }
    public List<string> CalculationNotes { get; set; } = new();
}

public class MonthlyPayrollReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public string? BranchName { get; set; }
    public int TotalEmployees { get; set; }
    public decimal TotalBaseSalary { get; set; }
    public decimal TotalAllowance { get; set; }
    public decimal TotalBonus { get; set; }
    public decimal TotalPenalty { get; set; }
    public decimal TotalGrossSalary { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalNetSalary { get; set; }
    public decimal AverageNetSalary { get; set; }
    public List<PayrollDto> PayrollDetails { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}
