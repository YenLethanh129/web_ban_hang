using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.EnumTypes;

[Table("order_statuses")]
[Index("Name", Name = "UQ__order_st__72E12F1B05345EFE", IsUnique = true)]
public partial class OrderStatus : BaseEntity
{
    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
