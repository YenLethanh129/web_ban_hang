using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("positions")]
public partial class EmployeePosition : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employees { get; set; } = [];

    [Column("need_schedule")]
    public bool NeedSchedule { get; set; }
}
