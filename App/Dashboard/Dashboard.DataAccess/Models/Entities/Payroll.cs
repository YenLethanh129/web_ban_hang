using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("payrolls")]
public partial class Payroll : BaseAuditableEntity
{
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("month")]
    public int Month { get; set; }

    [Column("year")]
    public int Year { get; set; }

    [Column("total_working_hours", TypeName = "decimal(18, 2)")]
    public decimal? TotalWorkingHours { get; set; }

    [Column("base_salary", TypeName = "decimal(18, 2)")]
    public decimal? BaseSalary { get; set; }

    [Column("allowance", TypeName = "decimal(18, 2)")]
    public decimal? Allowance { get; set; }

    [Column("bonus", TypeName = "decimal(18, 2)")]
    public decimal? Bonus { get; set; }

    [Column("penalty", TypeName = "decimal(18, 2)")]
    public decimal? Penalty { get; set; }

    [Column("gross_salary", TypeName = "decimal(18, 2)")]
    public decimal? GrossSalary { get; set; }

    [Column("tax_amount", TypeName = "decimal(18, 2)")]
    public decimal? TaxAmount { get; set; }

    [Column("net_salary", TypeName = "decimal(18, 2)")]
    public decimal? NetSalary { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Payrolls")]
    public virtual Employee Employee { get; set; } = null!;
}
