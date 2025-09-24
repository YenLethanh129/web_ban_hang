using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.EnumTypes;

[Table("payment_methods")]
[Index("Name", Name = "UQ__payment___72E12F1BEC94A5AC", IsUnique = true)]
public partial class PaymentMethod : BaseEntity
{

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();
}
