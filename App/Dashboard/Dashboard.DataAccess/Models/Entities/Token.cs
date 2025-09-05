using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

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

    [Column("user_id")]
    public long? UserId { get; set; }

    [Column("token_type")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TokenType { get; set; }

    [Column("token")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Token1 { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Tokens")]
    public virtual User? User { get; set; }
}
