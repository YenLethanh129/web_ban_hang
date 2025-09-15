using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Orders;

[Table("order_details")]
public partial class OrderDetail : BaseAuditableEntity
{
    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }

    [Column("color")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Color { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [Column("total_amount", TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [Column("unit_price", TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
