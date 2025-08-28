using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("order_payments")]
public partial class OrderPayment
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("payment_method_id")]
    public long PaymentMethodId { get; set; }

    [Column("payment_status_id")]
    public long PaymentStatusId { get; set; }

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("payment_date")]
    [Precision(6)]
    public DateTime? PaymentDate { get; set; }

    [Column("transaction_id")]
    [StringLength(100)]
    [Unicode(false)]
    public string? TransactionId { get; set; }

    [Column("notes")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Notes { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderPayments")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("OrderPayments")]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    [ForeignKey("PaymentStatusId")]
    [InverseProperty("OrderPayments")]
    public virtual PaymentStatus PaymentStatus { get; set; } = null!;
}
