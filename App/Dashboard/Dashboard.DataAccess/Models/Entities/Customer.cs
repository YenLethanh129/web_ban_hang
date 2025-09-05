using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("customers")]
public partial class Customer : BaseAuditableEntity
{
    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("fullname")]
    [StringLength(100)]
    public string Fullname { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string? Address { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Customer")]
    public virtual User IdNavigation { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = [];
    public string FullInfo => $"{Fullname} - {PhoneNumber} - {Email}";
    public bool IsActive() => IdNavigation.IsActive;
    public void InverseActiveStatus() => IdNavigation.IsActive = !IdNavigation.IsActive;
}
