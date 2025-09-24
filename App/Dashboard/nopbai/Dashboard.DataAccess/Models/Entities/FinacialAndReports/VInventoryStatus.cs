using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.FinacialAndReports;

[Keyless]
[Table("v_inventory_status")]
public partial class VInventoryStatus
{
    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("ingredient_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string IngredientName { get; set; } = null!;

    [Column("location_id")]
    public long LocationId { get; set; }

    [Column("location_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string LocationName { get; set; } = null!;

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("branch_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [Column("quantity_on_hand", TypeName = "decimal(18, 2)")]
    public decimal QuantityOnHand { get; set; }

    [Column("quantity_reserved", TypeName = "decimal(18, 2)")]
    public decimal QuantityReserved { get; set; }

    [Column("available_quantity", TypeName = "decimal(18, 2)")]
    public decimal AvailableQuantity { get; set; }

    [Column("safety_stock", TypeName = "decimal(18, 2)")]
    public decimal? SafetyStock { get; set; }

    [Column("stock_status")]
    [StringLength(20)]
    [Unicode(false)]
    public string StockStatus { get; set; } = null!;

    [Column("unit_of_measure")]
    [StringLength(50)]
    [Unicode(false)]
    public string UnitOfMeasure { get; set; } = null!;

    [Column("last_updated")]
    [Precision(6)]
    public DateTime LastUpdated { get; set; }

    [ForeignKey("BranchId")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("IngredientId")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
