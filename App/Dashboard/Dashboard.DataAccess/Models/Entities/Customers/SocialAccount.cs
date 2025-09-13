using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Customers;

[Table("social_accounts")]
public partial class SocialAccount : BaseAuditableEntity
{

    [Column("provider_id")]
    public long ProviderId { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("provider")]
    [StringLength(20)]
    [Unicode(false)]
    public string Provider { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("email")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Email { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SocialAccounts")]
    public virtual User? User { get; set; }
}
