using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Products;

[Table("categories")]
[Index("Name", Name = "UQ__categori__72E12F1BFB7E3EEA", IsUnique = true)]
public partial class Category : BaseEntity
{
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
