using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("shipping_providers")]
public partial class ShippingProvider : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("contact_info")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ContactInfo { get; set; }

    [Column("api_endpoint")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ApiEndpoint { get; set; }

    [InverseProperty("ShippingProvider")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty("ShippingProvider")]
    public virtual ICollection<OrderShipment> OrderShipments { get; set; } = new List<OrderShipment>();
}
