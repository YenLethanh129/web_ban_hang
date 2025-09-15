using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;

[Table("goods_received_details")]
public partial class GoodsReceivedDetail : BaseAuditableEntity
{

    [Column("grn_id")]
    public long GrnId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("ordered_quantity", TypeName = "decimal(18, 2)")]
    public decimal OrderedQuantity { get; set; }

    [Column("received_quantity", TypeName = "decimal(18, 2)")]
    public decimal ReceivedQuantity { get; set; }

    [Column("rejected_quantity", TypeName = "decimal(18, 2)")]
    public decimal? RejectedQuantity { get; set; }

    [Column("quality_status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? QualityStatus { get; set; }

    [Column("rejection_reason")]
    [StringLength(255)]
    [Unicode(false)]
    public string? RejectionReason { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column("expiry_date")]
    public DateTime? ExpiryDate { get; set; }

    [Column("batch_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BatchNumber { get; set; }

    [Column("storage_location")]
    [StringLength(100)]
    [Unicode(false)]
    public string? StorageLocation { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("GrnId")]
    [InverseProperty("GoodsReceivedDetails")]
    public virtual GoodsReceivedNote Grn { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("GoodsReceivedDetails")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
