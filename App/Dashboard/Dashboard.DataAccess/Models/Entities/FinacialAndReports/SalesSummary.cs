using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Branches;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.FinacialAndReports;

[Table("sales_summary")]
public partial class SalesSummary : BaseAuditableEntity
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

    [ForeignKey("BranchId")]
    [InverseProperty("SalesSummaries")]
    public virtual Branch? Branch { get; set; }
}
