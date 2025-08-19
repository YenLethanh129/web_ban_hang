using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("order_delivery_tracking")]
public partial class OrderDeliveryTracking : BaseAuditableEntity
{
    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("tracking_number")]
    [StringLength(100)]
    [Unicode(false)]
    public string TrackingNumber { get; set; } = null!;

    [Column("status_id")]
    public long StatusId { get; set; }

    [Column("location")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Location { get; set; }

    [Column("estimated_delivery")]
    [Precision(6)]
    public DateTime? EstimatedDelivery { get; set; }

    [Column("delivery_person_id")]
    public long? DeliveryPersonId { get; set; }

    [Column("shipping_provider_id")]
    public long? ShippingProviderId { get; set; }

    [ForeignKey("DeliveryPersonId")]
    [InverseProperty("OrderDeliveryTrackings")]
    public virtual Employee? DeliveryPerson { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDeliveryTrackings")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ShippingProviderId")]
    [InverseProperty("OrderDeliveryTrackings")]
    public virtual ShippingProvider? ShippingProvider { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("OrderDeliveryTrackings")]
    public virtual DeliveryStatus Status { get; set; } = null!;
}
