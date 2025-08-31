using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_transfer_request_details")]
public partial class IngredientTransferRequestDetail : BaseAuditableEntity
{
    [Column("transfer_request_id")]
    public long TransferRequestId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("requested_quantity", TypeName = "decimal(18, 2)")]
    public decimal RequestedQuantity { get; set; }

    [Column("approved_quantity", TypeName = "decimal(18, 2)")]
    public decimal? ApprovedQuantity { get; set; }

    [Column("transferred_quantity", TypeName = "decimal(18, 2)")]
    public decimal TransferredQuantity { get; set; } = 0;

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED, TRANSFERRED

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [ForeignKey("TransferRequestId")]
    [InverseProperty("TransferRequestDetails")]
    public virtual IngredientTransferRequest TransferRequest { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("TransferRequestDetails")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    // Computed property
    [NotMapped]
    public decimal RemainingQuantity => (ApprovedQuantity ?? RequestedQuantity) - TransferredQuantity;
}
