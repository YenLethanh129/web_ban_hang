using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("shipping_providers")]
[Index("Name", Name = "UQ__shipping__72E12F1B8D36D97F", IsUnique = true)]
public partial class ShippingProvider
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

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

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [InverseProperty("ShippingProvider")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty("ShippingProvider")]
    public virtual ICollection<OrderShipment> OrderShipments { get; set; } = new List<OrderShipment>();
}
