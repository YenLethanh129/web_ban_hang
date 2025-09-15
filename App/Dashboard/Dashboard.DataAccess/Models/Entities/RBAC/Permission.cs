using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.RBAC;

[Table("permissions")]
[Index("Name", Name = "UQ__permissions__name", IsUnique = true)]
public partial class Permission : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("resource")]
    [StringLength(50)]
    public string Resource { get; set; } = string.Empty;

    [Column("action")]
    [StringLength(50)]
    public string Action { get; set; } = string.Empty;

    [InverseProperty("Permission")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}