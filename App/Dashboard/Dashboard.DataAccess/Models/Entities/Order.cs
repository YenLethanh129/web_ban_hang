using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("orders")]
public partial class Order : BaseAuditableEntity
{
    [Column("customer_id")]
    public long CustomerId { get; set; }

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("total_money", TypeName = "decimal(18, 2)")]
    public decimal? TotalMoney { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("notes")]
    [StringLength(500)]
    [Unicode(true)]
    public string? Notes { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Orders")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Orders")]
    public virtual OrderStatus? Status { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderShipment> OrderShipments { get; set; } = new List<OrderShipment>();
}
