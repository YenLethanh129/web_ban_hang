using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.EnumTypes;

[Table("delivery_statuses")]
[Index("Name", Name = "UQ__delivery__72E12F1BA335DE00", IsUnique = true)]
public partial class DeliveryStatus : BaseEntity
{
    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();
}
