using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("taxes")]
public partial class Taxis
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("tax_rate", TypeName = "decimal(5, 2)")]
    public decimal TaxRate { get; set; }

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("Tax")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
