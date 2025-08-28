using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_warehouse")]
public partial class IngredientWarehouse
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientWarehouses")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
