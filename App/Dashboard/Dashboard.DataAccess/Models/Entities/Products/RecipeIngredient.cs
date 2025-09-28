using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities.Products;

[Table("recipe_ingredients")]
public class RecipeIngredient : BaseAuditableEntity
{
    [Required]
    [Column("recipe_id")]
    public long RecipeId { get; set; }

    [Required]
    [Column("ingredient_id")]
    public long IngredientId { get; set; }

    [Required]
    [Column("quantity", TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }

    [Required]
    [Column("unit")]
    [StringLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Column("waste_percentage", TypeName = "decimal(18,4)")]
    public decimal? WastePercentage { get; set; } = 0;

    [NotMapped]
    public decimal ActualQuantityNeeded => Quantity * (1 + (WastePercentage ?? 0) / 100);

    [Column("notes")]
    [StringLength(500)]
    public string? Notes { get; set; }

    [Column("is_optional")]
    public bool IsOptional { get; set; } = false;

    [Column("sort_order")]
    public int SortOrder { get; set; } = 0;

    [ForeignKey("RecipeId")]
    public virtual Recipe Recipe { get; set; } = null!;

    [ForeignKey("IngredientId")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}