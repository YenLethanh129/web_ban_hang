using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities
{
    [Table("inventory_thresholds")]
    public class InventoryThreshold : BaseAuditableEntity
    {
        [Required]
        [Column("branch_id")]
        public long BranchId { get; set; }

        [Required]
        [Column("ingredient_id")]
        public long IngredientId { get; set; }

        [Required]
        [Column("safety_stock", TypeName = "decimal(10,3)")]
        public decimal SafetyStock { get; set; }

        [Column("reorder_point", TypeName = "decimal(10,3)")]
        public decimal ReorderPoint { get; set; }

        [Column("maximum_stock", TypeName = "decimal(10,3)")]
        public decimal MaximumStock { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("lead_time_days")]
        public int LeadTimeDays { get; set; }

        [Column("average_daily_consumption", TypeName = "decimal(10,3)")]
        public decimal AverageDailyConsumption { get; set; }

        [Column("last_calculated_date")]
        public DateTime? LastCalculatedDate { get; set; }

        // Navigation properties
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;
        
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
}