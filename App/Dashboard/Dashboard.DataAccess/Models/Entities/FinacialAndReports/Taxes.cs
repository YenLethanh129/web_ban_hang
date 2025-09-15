using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Models.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.FinacialAndReports;

[Table("taxes")]
public partial class Taxes : BaseAuditableEntity
{

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("tax_rate", TypeName = "decimal(5, 2)")]
    public decimal TaxRate { get; set; }

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("Tax")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
