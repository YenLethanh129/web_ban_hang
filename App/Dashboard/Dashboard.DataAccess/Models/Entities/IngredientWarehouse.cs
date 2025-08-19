using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_warehouse")]
public partial class IngredientWarehouse : BaseAuditableEntity
{
    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientWarehouses")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
