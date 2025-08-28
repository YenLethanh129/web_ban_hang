using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("suppliers")]
public partial class Supplier
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
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
    [Unicode(false)]
    public string? Address { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

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
