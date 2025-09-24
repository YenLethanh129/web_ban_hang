using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.EnumTypes;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Orders;

[Table("order_shipments")]
public partial class OrderShipment : BaseAuditableEntity
{
    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("shipping_provider_id")]
    public long? ShippingProviderId { get; set; }

    [Column("shipping_address")]
    [StringLength(500)]
    public string ShippingAddress { get; set; } = null!;

    [Column("shipping_cost", TypeName = "decimal(18, 2)")]
    public decimal? ShippingCost { get; set; }

    [Column("shipping_method")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ShippingMethod { get; set; }

    [Column("estimated_delivery_date")]
    [Precision(6)]
    public DateTime? EstimatedDeliveryDate { get; set; }

    [Column("notes")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Notes { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderShipments")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ShippingProviderId")]
    [InverseProperty("OrderShipments")]
    public virtual ShippingProvider? ShippingProvider { get; set; }
}
