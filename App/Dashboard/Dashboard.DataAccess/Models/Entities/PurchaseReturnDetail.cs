using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("purchase_return_details")]
public partial class PurchaseReturnDetail : BaseAuditableEntity
{
    [Column("return_id")]
    public long ReturnId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Column("return_quantity", TypeName = "decimal(18, 2)")]
    public decimal ReturnQuantity { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column("return_amount", TypeName = "decimal(18, 2)")]
    public decimal? ReturnAmount { get; set; }

    [Column("return_reason")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ReturnReason { get; set; }

    [Column("batch_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BatchNumber { get; set; }

    [Column("expiry_date")]
    public DateTime? ExpiryDate { get; set; }

    [Column("quality_issue")]
    [StringLength(255)]
    [Unicode(false)]
    public string? QualityIssue { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("PurchaseReturnDetails")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("ReturnId")]
    [InverseProperty("PurchaseReturnDetails")]
    public virtual PurchaseReturn Return { get; set; } = null!;
}
