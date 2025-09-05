using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_warehouse")]
[Index("IngredientId", IsUnique = true)]
public partial class IngredientWarehouse : BaseAuditableEntity
{
    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("safety_stock", TypeName = "decimal(18, 2)")]
    public decimal SafetyStock { get; set; }

    [Column("maximum_stock", TypeName = "decimal(18, 2)")]
    public decimal? MaximumStock { get; set; }

    [Column("location")]
    [StringLength(100)]
    public string? Location { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientWarehouse")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
