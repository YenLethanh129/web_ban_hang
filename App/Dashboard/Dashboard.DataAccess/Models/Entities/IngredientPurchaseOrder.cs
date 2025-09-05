using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_purchase_orders")]
[Index("PurchaseOrderCode", Name = "UQ__ingredie__19DA46F1B121FE7C", IsUnique = true)]
public partial class IngredientPurchaseOrder : BaseAuditableEntity
{
    [Column("purchase_order_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string PurchaseOrderCode { get; set; } = null!;

    [Column("supplier_id")]
    public long? SupplierId { get; set; }

    [Column("branch_id")]
    public long? BranchId { get; set; }

    [Column("employee_id")]
    public long? EmployeeId { get; set; }

    [Column("order_date")]
    [Precision(6)]
    public DateTime? OrderDate { get; set; }

    [Column("expected_delivery_date")]
    [Precision(6)]
    public DateTime? ExpectedDeliveryDate { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("total_amount_before_tax", TypeName = "decimal(18, 2)")]
    public decimal? TotalAmountBeforeTax { get; set; }

    [Column("total_tax_amount", TypeName = "decimal(18, 2)")]
    public decimal? TotalTaxAmount { get; set; }

    [Column("total_amount_after_tax", TypeName = "decimal(18, 2)")]
    public decimal? TotalAmountAfterTax { get; set; }

    [Column("discount_amount", TypeName = "decimal(18, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("final_amount", TypeName = "decimal(18, 2)")]
    public decimal? FinalAmount { get; set; }

    [Column("note")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("IngredientPurchaseOrders")]
    public virtual Branch? Branch { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("IngredientPurchaseOrders")]
    public virtual Employee? Employee { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<IngredientPurchaseOrderDetail> IngredientPurchaseOrderDetails { get; set; } = new List<IngredientPurchaseOrderDetail>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();

    [ForeignKey("StatusId")]
    [InverseProperty("IngredientPurchaseOrders")]
    public virtual PurchaseOrderStatus? Status { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("IngredientPurchaseOrders")]
    public virtual Supplier? Supplier { get; set; }
}
