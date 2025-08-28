using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredients")]
public partial class Ingredient
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("category_id")]
    public long CategoryId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("unit")]
    [StringLength(50)]
    [Unicode(false)]
    public string Unit { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("tax_id")]
    public long? TaxId { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [InverseProperty("Ingredient")]
    public virtual ICollection<BranchIngredientInventory> BranchIngredientInventories { get; set; } = new List<BranchIngredientInventory>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Ingredients")]
    public virtual IngredientCategory Category { get; set; } = null!;

    [InverseProperty("Ingredient")]
    public virtual ICollection<GoodsReceivedDetail> GoodsReceivedDetails { get; set; } = new List<GoodsReceivedDetail>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientPurchaseOrderDetail> IngredientPurchaseOrderDetails { get; set; } = new List<IngredientPurchaseOrderDetail>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientTransfer> IngredientTransfers { get; set; } = new List<IngredientTransfer>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<IngredientWarehouse> IngredientWarehouses { get; set; } = new List<IngredientWarehouse>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; } = new List<PurchaseInvoiceDetail>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; } = new List<PurchaseReturnDetail>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<SupplierIngredientPrice> SupplierIngredientPrices { get; set; } = new List<SupplierIngredientPrice>();

    [ForeignKey("TaxId")]
    [InverseProperty("Ingredients")]
    public virtual Taxis? Tax { get; set; }
}
