using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Employees;

[Table("employee_salaries")]
public partial class EmployeeSalary : BaseAuditableEntity
{
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("base_salary", TypeName = "decimal(18, 2)")]
    public decimal BaseSalary { get; set; }

    [Column("salary_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SalaryType { get; set; }

    [Column("allowance", TypeName = "decimal(18, 2)")]
    public decimal? Allowance { get; set; }

    [Column("bonus", TypeName = "decimal(18, 2)")]
    public decimal? Bonus { get; set; }

    [Column("penalty", TypeName = "decimal(18, 2)")]
    public decimal? Penalty { get; set; }

    [Column("tax_rate", TypeName = "decimal(18, 2)")]
    public decimal? TaxRate { get; set; }

    [Column("effective_date")]
    public DateTime EffectiveDate { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeSalaries")]
    public virtual Employee Employee { get; set; } = null!;
}
