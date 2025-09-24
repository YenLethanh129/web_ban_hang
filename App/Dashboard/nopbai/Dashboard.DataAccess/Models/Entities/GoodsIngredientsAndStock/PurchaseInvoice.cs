using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;

[Table("purchase_invoices")]
[Index("InvoiceCode", Name = "UQ__purchase__5ED70A355181E3E1", IsUnique = true)]
public partial class PurchaseInvoice : BaseAuditableEntity
{
    [Column("invoice_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string InvoiceCode { get; set; } = null!;

    [Column("purchase_order_id")]
    public long? PurchaseOrderId { get; set; }

    [Column("supplier_id")]
    public long SupplierId { get; set; }

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("invoice_date")]
    [Precision(6)]
    public DateTime InvoiceDate { get; set; }

    [Column("due_date")]
    [Precision(6)]
    public DateTime? DueDate { get; set; }

    [Column("payment_date")]
    [Precision(6)]
    public DateTime? PaymentDate { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("total_amount_before_tax", TypeName = "decimal(18, 2)")]
    public decimal? TotalAmountBeforeTax { get; set; }

    [Column("total_tax_amount", TypeName = "decimal(18, 2)")]
    public decimal? TotalTaxAmount { get; set; }

    [Column("total_amount_after_tax", TypeName = "decimal(18, 2)")]
    public decimal? TotalAmountAfterTax { get; set; }

    [Column("paid_amount", TypeName = "decimal(18, 2)")]
    public decimal? PaidAmount { get; set; }

    [Column("remaining_amount", TypeName = "decimal(18, 2)")]
    public decimal? RemainingAmount { get; set; }

    [Column("discount_amount", TypeName = "decimal(18, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentMethod { get; set; }

    [Column("payment_reference")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PaymentReference { get; set; }

    [Column("note")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("PurchaseInvoices")]
    public virtual Branch? Branch { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty("Invoice")]
    public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; } = new List<PurchaseInvoiceDetail>();

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("PurchaseInvoices")]
    public virtual IngredientPurchaseOrder? PurchaseOrder { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();

    [ForeignKey("StatusId")]
    [InverseProperty("PurchaseInvoices")]
    public virtual InvoiceStatus? Status { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("PurchaseInvoices")]
    public virtual Supplier Supplier { get; set; } = null!;
}
