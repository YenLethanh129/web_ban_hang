using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Branches;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.FinacialAndReports;

[Table("cogs_summary")]
public partial class CogsSummary : BaseAuditableEntity
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
    [InverseProperty("ExpensesSummaries")]
    public virtual Branch? Branch { get; set; }
}
