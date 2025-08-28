using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("goods_received_statuses")]
[Index("Name", Name = "UQ__goods_re__72E12F1B5B4629D9", IsUnique = true)]
public partial class GoodsReceivedStatus
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();
}
