using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.EnumTypes;

[Table("payment_statuses")]
[Index("Name", Name = "UQ__payment___72E12F1B7D0643E4", IsUnique = true)]
public partial class PaymentStatus : BaseEntity
{
    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("PaymentStatus")]
    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();
}
