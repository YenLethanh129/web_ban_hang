using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities.Products
{
    [Table("recipe_ingredients")]
    public class RecipeIngredient : BaseAuditableEntity
    {
        [Required]
        public long RecipeId { get; set; }

        [Required]
        public long IngredientId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = string.Empty; // gram, ml, kg, piece, etc.

        [Column(TypeName = "decimal(18,4)")]
        public decimal? WastePercentage { get; set; } = 0; // % hao hụt trong quá trình chế biến

        [Column(TypeName = "decimal(18,4)")]
        public decimal ActualQuantityNeeded => Quantity * (1 + (WastePercentage ?? 0) / 100);

        [StringLength(500)]
        public string? Notes { get; set; }

        public bool IsOptional { get; set; } = false; // Nguyên liệu tùy chọn

        public int SortOrder { get; set; } = 0; // Thứ tự trong công thức

        // Navigation properties
        public virtual Recipe Recipe { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
}