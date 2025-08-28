using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("purchase_returns")]
[Index("ReturnCode", Name = "UQ__purchase__51FB33A06DB976E7", IsUnique = true)]
public partial class PurchaseReturn
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("return_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string ReturnCode { get; set; } = null!;

    [Column("grn_id")]
    public long? GrnId { get; set; }

    [Column("invoice_id")]
    public long? InvoiceId { get; set; }

    [Column("supplier_id")]
    public long SupplierId { get; set; }

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("return_date")]
    [Precision(6)]
    public DateTime? ReturnDate { get; set; }

    [Column("return_reason")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ReturnReason { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("total_return_amount", TypeName = "decimal(18, 2)")]
    public decimal? TotalReturnAmount { get; set; }

    [Column("refund_amount", TypeName = "decimal(18, 2)")]
    public decimal? RefundAmount { get; set; }

    [Column("credit_note_number")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CreditNoteNumber { get; set; }

    [Column("approved_by")]
    public long? ApprovedBy { get; set; }

    [Column("approval_date")]
    [Precision(6)]
    public DateTime? ApprovalDate { get; set; }

    [Column("note")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Note { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("PurchaseReturns")]
    public virtual Employee? ApprovedByNavigation { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("PurchaseReturns")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("GrnId")]
    [InverseProperty("PurchaseReturns")]
    public virtual GoodsReceivedNote? Grn { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("PurchaseReturns")]
    public virtual PurchaseInvoice? Invoice { get; set; }

    [InverseProperty("Return")]
    public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; } = new List<PurchaseReturnDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("PurchaseReturns")]
    public virtual Supplier Supplier { get; set; } = null!;
}
