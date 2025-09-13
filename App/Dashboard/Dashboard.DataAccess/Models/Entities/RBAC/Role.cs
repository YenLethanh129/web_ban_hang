using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.RBAC;

[Table("roles")]
[Index("Name", Name = "UQ__roles__72E12F1B8563DF69", IsUnique = true)]
public partial class Role : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [InverseProperty("Role")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}