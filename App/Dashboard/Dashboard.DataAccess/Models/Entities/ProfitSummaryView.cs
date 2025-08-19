using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("v_profit_summary")]
[Keyless]
public class ProfitSummaryView
{
    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("year")]
    public int Year { get; set; }

    [Column("month")]
    public int Month { get; set; }

    [Column("period")]
    [StringLength(7)]
    [Unicode(false)]
    public string Period { get; set; } = null!;

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

    // Navigation property
    [ForeignKey("BranchId")]
    public virtual Branch? Branch { get; set; }
}
