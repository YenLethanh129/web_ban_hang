using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("branch_ingredient_inventory")]
[Index("BranchId", "IngredientId", IsUnique = true)]
public partial class BranchIngredientInventory : BaseAuditableEntity
{
    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("reserved_quantity", TypeName = "decimal(18, 2)")]
    public decimal ReservedQuantity { get; set; } = 0;

    [Column("minimum_stock", TypeName = "decimal(18, 2)")]
    public decimal MinimumStock { get; set; }

    [Column("last_transfer_date")]
    public DateTime? LastTransferDate { get; set; }

    [Column("location")]
    [StringLength(100)]
    public string? Location { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("BranchIngredientInventories")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("BranchIngredientInventories")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    // Computed property for available quantity
    [NotMapped]
    public decimal AvailableQuantity => Quantity - ReservedQuantity;
}
