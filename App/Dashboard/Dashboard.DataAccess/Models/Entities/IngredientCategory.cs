using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Models.Entities;

[Table("ingredient_categories")]
public class IngredientCategory : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(50)]
    [Unicode(true)]
    public string Name { get; set; } = null!;
    [Column("description")]
    [StringLength(255)]
    [Unicode(true)]
    public string? Description { get; set; }
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public override string ToString()
    {
        return $"{Name} - {Description}";
    }
}

