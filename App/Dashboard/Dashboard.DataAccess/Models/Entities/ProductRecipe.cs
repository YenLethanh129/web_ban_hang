using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("product_recipes")]
public partial class ProductRecipe : BaseAuditableEntity
{

    [Column("product_id")]
    public long ProductId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("ProductRecipes")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductRecipes")]
    public virtual Product Product { get; set; } = null!;
}
