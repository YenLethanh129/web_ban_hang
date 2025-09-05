using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("supplier_performance")]
public partial class SupplierPerformance : BaseAuditableEntity
{

    [Column("supplier_id")]
    public long SupplierId { get; set; }

    [Column("evaluation_period")]
    [StringLength(20)]
    [Unicode(false)]
    public string EvaluationPeriod { get; set; } = null!;

    [Column("period_value")]
    [StringLength(20)]
    [Unicode(false)]
    public string PeriodValue { get; set; } = null!;

    [Column("total_orders")]
    public int? TotalOrders { get; set; }

    [Column("total_amount", TypeName = "decimal(18, 2)")]
    public decimal? TotalAmount { get; set; }

    [Column("on_time_deliveries")]
    public int? OnTimeDeliveries { get; set; }

    [Column("late_deliveries")]
    public int? LateDeliveries { get; set; }

    [Column("quality_score", TypeName = "decimal(3, 2)")]
    public decimal? QualityScore { get; set; }

    [Column("service_score", TypeName = "decimal(3, 2)")]
    public decimal? ServiceScore { get; set; }

    [Column("overall_rating", TypeName = "decimal(3, 2)")]
    public decimal? OverallRating { get; set; }

    [Column("total_returns")]
    public int? TotalReturns { get; set; }

    [Column("return_value", TypeName = "decimal(18, 2)")]
    public decimal? ReturnValue { get; set; }

    [Column("comments")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Comments { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierPerformances")]
    public virtual Supplier Supplier { get; set; } = null!;
}
