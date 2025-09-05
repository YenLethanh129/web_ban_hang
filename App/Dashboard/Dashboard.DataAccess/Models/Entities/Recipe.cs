using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities
{
    [Table("recipes")]
    public class Recipe : BaseAuditableEntity
    {
        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("product_id")]
        public long ProductId { get; set; }

        [Required]
        [Column("serving_size", TypeName = "decimal(18,2)")]
        public decimal ServingSize { get; set; } = 1;

        [StringLength(50)]
        [Column("unit")]
        public string Unit { get; set; } = "portion";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        [Column("notes")]
        public string? Notes { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}