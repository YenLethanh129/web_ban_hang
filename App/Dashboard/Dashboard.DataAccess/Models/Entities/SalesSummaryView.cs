using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("v_sales_summary")]
[Keyless]
public class SalesSummaryView
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

    [Column("total_orders")]
    public int TotalOrders { get; set; }

    [Column("total_products")]
    public int TotalProducts { get; set; }

    [Column("revenue_before_tax", TypeName = "decimal(18, 2)")]
    public decimal RevenueBeforeTax { get; set; }

    [Column("revenue_after_tax", TypeName = "decimal(18, 2)")]
    public decimal RevenueAfterTax { get; set; }

    [Column("tax_amount", TypeName = "decimal(18, 2)")]
    public decimal TaxAmount { get; set; }

    // Navigation property
    [ForeignKey("BranchId")]
    public virtual Branch? Branch { get; set; }
}
