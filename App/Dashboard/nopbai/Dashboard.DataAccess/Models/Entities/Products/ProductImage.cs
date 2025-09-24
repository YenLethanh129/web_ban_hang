using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Products;

[Table("product_images")]
public partial class ProductImage : BaseEntity
{

    [Column("product_id")]
    public long? ProductId { get; set; }

    [Column("image_url")]
    [StringLength(300)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product? Product { get; set; }
}
