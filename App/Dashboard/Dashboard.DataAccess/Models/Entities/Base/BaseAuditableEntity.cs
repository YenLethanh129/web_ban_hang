using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.DataAccess.Models.Entities.Base;
public abstract class BaseAuditableEntity : BaseEntity
{
    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }
    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }
}
