using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_purchase_order_details")]
public partial class IngredientPurchaseOrderDetail : BaseAuditableEntity
{
    [Column("purchase_order_id")]
    public long PurchaseOrderId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("quantity", TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("tax_price", TypeName = "decimal(18, 2)")]
    public decimal TaxPrice { get; set; }

    [Column("total_price", TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("IngredientPurchaseOrderDetails")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("IngredientPurchaseOrderDetails")]
    public virtual IngredientPurchaseOrder PurchaseOrder { get; set; } = null!;
}
