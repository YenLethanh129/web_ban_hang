using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Customers;
using Dashboard.DataAccess.Models.Entities.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.RBAC;

[Table("employee_users")]
public partial class EmployeeUserAccount : BaseAuditableEntity
{
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("role_id")]
    public long RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty(nameof(Token.User))]
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}


