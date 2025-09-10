using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Keyless]
[Table("v_employee_payroll")]
public partial class VEmployeePayroll
{
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("branch_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [Column("position_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PositionName { get; set; }

    [Column("base_salary", TypeName = "decimal(18, 2)")]
    public decimal? BaseSalary { get; set; }

    [Column("salary_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SalaryType { get; set; }

    [Column("total_allowances", TypeName = "decimal(18, 2)")]
    public decimal TotalAllowances { get; set; }

    [Column("total_bonus", TypeName = "decimal(18, 2)")]
    public decimal TotalBonus { get; set; }

    [Column("total_deductions", TypeName = "decimal(18, 2)")]
    public decimal TotalDeductions { get; set; }

    [Column("gross_salary", TypeName = "decimal(18, 2)")]
    public decimal GrossSalary { get; set; }

    [Column("effective_date")]
    public DateTime? EffectiveDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; } = null!;
}
