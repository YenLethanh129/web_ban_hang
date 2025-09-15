using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock
{
    [Table("inventory_movements")]
    public class InventoryMovement : BaseAuditableEntity
    {

        [Required]
        public long BranchId { get; set; }

        [Required]
        public long IngredientId { get; set; }

        [Required]
        [StringLength(20)]
        public string MovementType { get; set; } = string.Empty; // IN, OUT, TRANSFER, ADJUSTMENT

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityAfter { get; set; }

        [StringLength(100)]
        public string? ReferenceType { get; set; } // ORDER, PURCHASE, TRANSFER, ADJUSTMENT

        public long? ReferenceId { get; set; }

        [StringLength(100)]
        public string? ReferenceCode { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public long? EmployeeId { get; set; }

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Branch Branch { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
        public virtual Employee? Employee { get; set; }
    }
}