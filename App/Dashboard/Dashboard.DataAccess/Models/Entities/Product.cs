using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("category_id")]
    public long? CategoryId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("tax_id")]
    public long? TaxId { get; set; }

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("thumbnail")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Thumbnail { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();

    [ForeignKey("TaxId")]
    [InverseProperty("Products")]
    public virtual Taxis? Tax { get; set; }
}
