using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.RBAC;

[Table("tokens")]
public partial class Token : BaseEntity
{

    [Column("expired")]
    public bool Expired { get; set; }

    [Column("revoked")]
    public bool Revoked { get; set; }

    [Column("expiration_date")]
    [Precision(6)]
    public DateTime? ExpirationDate { get; set; }

    [Column("token_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TokenType { get; set; }

    [Column("token")]
    [StringLength(255)]
    [Unicode(false)]
    public string? TokenValue { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual EmployeeUserAccount? User { get; set; }
    [ForeignKey("UserId")]
    public virtual CustomerUser? CustomerUser { get; set; }
}
