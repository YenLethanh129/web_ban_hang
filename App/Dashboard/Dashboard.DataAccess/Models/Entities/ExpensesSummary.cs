using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("expenses_summary")]
public partial class ExpensesSummary
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

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

    [Column("total_purchase_orders")]
    public int TotalPurchaseOrders { get; set; }

    [Column("total_ingredients")]
    public int TotalIngredients { get; set; }

    [Column("expense_before_tax", TypeName = "decimal(18, 2)")]
    public decimal ExpenseBeforeTax { get; set; }

    [Column("expense_after_tax", TypeName = "decimal(18, 2)")]
    public decimal ExpenseAfterTax { get; set; }

    [Column("tax_amount", TypeName = "decimal(18, 2)")]
    public decimal TaxAmount { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("ExpensesSummaries")]
    public virtual Branch? Branch { get; set; }
}
