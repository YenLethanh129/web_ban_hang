using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("users")]
public partial class User : BaseAuditableEntity
{
    [Column("employee_id")]
    public long? EmployeeId { get; set; }

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("facebook_account_id")]
    public long? FacebookAccountId { get; set; }

    [Column("google_account_id")]
    public long? GoogleAccountId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("role_id")]
    public long RoleId { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [Column("fullname")]
    [StringLength(100)]
    public string? Fullname { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string? Address { get; set; }

    [Column("password")]
    [StringLength(200)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [InverseProperty("IdNavigation")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Users")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();

    [InverseProperty("User")]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
