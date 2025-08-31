using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_transfers")]
public partial class IngredientTransfer : BaseAuditableEntity
{
    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
