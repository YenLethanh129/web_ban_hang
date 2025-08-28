using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("categories")]
[Index("Name", Name = "UQ__categori__72E12F1BFB7E3EEA", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
