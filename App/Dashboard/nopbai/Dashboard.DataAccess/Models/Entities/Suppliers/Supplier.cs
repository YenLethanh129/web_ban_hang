using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Suppliers;

[Table("suppliers")]
public partial class Supplier : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty("Supplier")]
    public virtual ICollection<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; } = new List<IngredientPurchaseOrder>();

    [InverseProperty("Supplier")]
    public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();

    [InverseProperty("Supplier")]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierIngredientPrice> SupplierIngredientPrices { get; set; } = new List<SupplierIngredientPrice>();

    [InverseProperty("Supplier")]
    public virtual ICollection<SupplierPerformance> SupplierPerformances { get; set; } = new List<SupplierPerformance>();
}
