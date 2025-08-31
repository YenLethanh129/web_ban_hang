using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_transfer_requests")]
public partial class IngredientTransferRequest : BaseAuditableEntity
{
    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("request_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string RequestNumber { get; set; } = null!;

    [Column("request_date")]
    public DateTime RequestDate { get; set; }

    [Column("required_date")]
    public DateTime RequiredDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED, COMPLETED

    [Column("total_items")]
    public int TotalItems { get; set; }

    [Column("approved_date")]
    public DateTime? ApprovedDate { get; set; }

    [Column("completed_date")]
    public DateTime? CompletedDate { get; set; }

    [Column("note")]
    [StringLength(500)]
    public string? Note { get; set; }

    [Column("requested_by")]
    [StringLength(100)]
    public string RequestedBy { get; set; } = null!;

    [Column("approved_by")]
    [StringLength(100)]
    public string? ApprovedBy { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("IngredientTransferRequests")]
    public virtual Branch Branch { get; set; } = null!;

    [InverseProperty("TransferRequest")]
    public virtual ICollection<IngredientTransferRequestDetail> TransferRequestDetails { get; set; } = new List<IngredientTransferRequestDetail>();
}
