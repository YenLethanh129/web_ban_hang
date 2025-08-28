using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_transfers")]
public partial class IngredientTransfer
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

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

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
