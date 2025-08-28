using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("orders")]
[Index("OrderUuid", Name = "UQ__orders__3DE398663640EAE7", IsUnique = true)]
[Index("OrderCode", Name = "UQ__orders__99D12D3FB3E1E0BA", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_uuid")]
    [StringLength(36)]
    [Unicode(false)]
    public string OrderUuid { get; set; } = null!;

    [Column("order_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string OrderCode { get; set; } = null!;

    [Column("customer_id")]
    public long CustomerId { get; set; }

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("total_money", TypeName = "decimal(18, 2)")]
    public decimal? TotalMoney { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [Column("notes")]
    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Orders")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderShipment> OrderShipments { get; set; } = new List<OrderShipment>();

    [ForeignKey("StatusId")]
    [InverseProperty("Orders")]
    public virtual OrderStatus? Status { get; set; }
}
