using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("purchase_invoice_details")]
public partial class PurchaseInvoiceDetail : BaseAuditableEntity
{

    [Column("invoice_id")]
    public long InvoiceId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("amount_before_tax", TypeName = "decimal(18, 2)")]
    public decimal AmountBeforeTax { get; set; }

    [Column("tax_rate", TypeName = "decimal(5, 2)")]
    public decimal? TaxRate { get; set; }

    [Column("tax_amount", TypeName = "decimal(18, 2)")]
    public decimal? TaxAmount { get; set; }

    [Column("amount_after_tax", TypeName = "decimal(18, 2)")]
    public decimal AmountAfterTax { get; set; }

    [Column("discount_rate", TypeName = "decimal(5, 2)")]
    public decimal? DiscountRate { get; set; }

    [Column("discount_amount", TypeName = "decimal(18, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("final_amount", TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    [Column("expiry_date")]
    public DateOnly? ExpiryDate { get; set; }

    [Column("batch_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BatchNumber { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("PurchaseInvoiceDetails")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("InvoiceId")]
    [InverseProperty("PurchaseInvoiceDetails")]
    public virtual PurchaseInvoice Invoice { get; set; } = null!;
}
