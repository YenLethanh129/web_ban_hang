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

    [Column("transfer_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string TransferType { get; set; } = "OUT"; // OUT: từ warehouse đến branch, IN: từ branch về warehouse

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = "PENDING"; // PENDING, COMPLETED, CANCELLED

    [Column("transfer_date")]
    public DateTime TransferDate { get; set; }

    [Column("completed_date")]
    public DateTime? CompletedDate { get; set; }

    [Column("note")]
    [StringLength(500)]
    public string? Note { get; set; }

    [Column("requested_by")]
    [StringLength(100)]
    public string? RequestedBy { get; set; }

    [Column("approved_by")]
    [StringLength(100)]
    public string? ApprovedBy { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientTransfers")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
