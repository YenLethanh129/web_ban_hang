using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("profit_summary")]
public partial class ProfitSummary : BaseAuditableEntity
{

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("period_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string PeriodType { get; set; } = null!;

    [Column("period_value")]
    [StringLength(20)]
    [Unicode(false)]
    public string PeriodValue { get; set; } = null!;

    [Column("revenue_before_tax", TypeName = "decimal(18, 2)")]
    public decimal RevenueBeforeTax { get; set; }

    [Column("revenue_after_tax", TypeName = "decimal(18, 2)")]
    public decimal RevenueAfterTax { get; set; }

    [Column("expense_before_tax", TypeName = "decimal(18, 2)")]
    public decimal ExpenseBeforeTax { get; set; }

    [Column("expense_after_tax", TypeName = "decimal(18, 2)")]
    public decimal ExpenseAfterTax { get; set; }

    [Column("output_tax", TypeName = "decimal(18, 2)")]
    public decimal OutputTax { get; set; }

    [Column("input_tax", TypeName = "decimal(18, 2)")]
    public decimal InputTax { get; set; }

    [Column("vat_to_pay", TypeName = "decimal(18, 2)")]
    public decimal VatToPay { get; set; }

    [Column("profit_before_tax", TypeName = "decimal(18, 2)")]
    public decimal ProfitBeforeTax { get; set; }

    [Column("profit_after_tax", TypeName = "decimal(18, 2)")]
    public decimal ProfitAfterTax { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("ProfitSummaries")]
    public virtual Branch? Branch { get; set; }
}
