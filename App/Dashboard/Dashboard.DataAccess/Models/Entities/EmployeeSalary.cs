using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("employee_salaries")]
public partial class EmployeeSalary
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

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
    public DateOnly EffectiveDate { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeSalaries")]
    public virtual Employee Employee { get; set; } = null!;
}
