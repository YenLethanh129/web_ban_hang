using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("order_statuses")]
[Index("Name", Name = "UQ__order_st__72E12F1B05345EFE", IsUnique = true)]
public partial class OrderStatus
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
