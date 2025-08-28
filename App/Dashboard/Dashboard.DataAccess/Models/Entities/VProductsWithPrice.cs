using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Keyless]
[Table("v_products_with_prices")]
public partial class VProductsWithPrice
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("sku")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Sku { get; set; }

    [Column("category_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string? CategoryName { get; set; }

    [Column("current_price", TypeName = "decimal(18, 2)")]
    public decimal? CurrentPrice { get; set; }

    [Column("price_type")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PriceType { get; set; }

    [Column("tax_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? TaxName { get; set; }

    [Column("tax_rate", TypeName = "decimal(5, 2)")]
    public decimal? TaxRate { get; set; }

    [Column("unit_of_measure")]
    [StringLength(50)]
    [Unicode(false)]
    public string UnitOfMeasure { get; set; } = null!;

    [Column("weight", TypeName = "decimal(10, 3)")]
    public decimal? Weight { get; set; }

    [Column("dimensions")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Dimensions { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
