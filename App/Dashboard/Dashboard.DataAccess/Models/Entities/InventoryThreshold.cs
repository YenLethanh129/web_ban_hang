using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities
{
    [Table("inventory_thresholds")]
    public class InventoryThreshold : BaseAuditableEntity
    {
        [Required]
        public long BranchId { get; set; }

        [Required]
        public long IngredientId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ReorderPoint { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumStock { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumThreshold { get; set; }

        [Required]
        public int LeadTimeDays { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AverageDailyConsumption { get; set; } = 0;

        public DateTime? LastCalculatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Branch Branch { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
}