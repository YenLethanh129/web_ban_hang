using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("social_accounts")]
public partial class SocialAccount
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

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

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("SocialAccounts")]
    public virtual User? User { get; set; }
}
