using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_purchase_orders")]
public partial class IngredientPurchaseOrder : BaseEntity
{
    [Column("supplier_id")]
    public long? SupplierId { get; set; }

    [Column("order_date")]
    [Precision(6)]
    public DateTime? OrderDate { get; set; }

    [Column("total_money", TypeName = "decimal(18, 2)")]
    public decimal? TotalMoney { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<IngredientPurchaseOrderDetail> IngredientPurchaseOrderDetails { get; set; } = new List<IngredientPurchaseOrderDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("IngredientPurchaseOrders")]
    public virtual Supplier? Supplier { get; set; }
}
