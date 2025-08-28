using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("payment_statuses")]
[Index("Name", Name = "UQ__payment___72E12F1B7D0643E4", IsUnique = true)]
public partial class PaymentStatus
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("PaymentStatus")]
    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();
}
