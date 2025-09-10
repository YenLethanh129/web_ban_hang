using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Keyless]
[Table("v_cogs_summary")]
public partial class VCogsSummary
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

    [ForeignKey("BranchId")]
    public virtual Branch? Branch { get; set; }
}
