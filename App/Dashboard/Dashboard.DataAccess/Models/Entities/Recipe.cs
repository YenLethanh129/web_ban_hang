using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities
{
    [Table("recipes")]
    public class Recipe : BaseAuditableEntity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServingSize { get; set; } = 1;

        [StringLength(50)]
        public string Unit { get; set; } = "portion";

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}