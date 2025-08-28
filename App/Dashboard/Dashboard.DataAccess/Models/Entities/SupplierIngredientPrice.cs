using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("supplier_ingredient_prices")]
public partial class SupplierIngredientPrice
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("supplier_id")]
    public long SupplierId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("unit")]
    [StringLength(50)]
    [Unicode(false)]
    public string Unit { get; set; } = null!;

    [Column("effective_date")]
    [Precision(6)]
    public DateTime? EffectiveDate { get; set; }

    [Column("expired_date")]
    [Precision(6)]
    public DateTime? ExpiredDate { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("SupplierIngredientPrices")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("SupplierId")]
    [InverseProperty("SupplierIngredientPrices")]
    public virtual Supplier Supplier { get; set; } = null!;
}
