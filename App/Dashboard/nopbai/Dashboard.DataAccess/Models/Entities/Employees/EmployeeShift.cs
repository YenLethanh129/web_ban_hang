using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Employees;

[Table("employee_shifts")]
public partial class EmployeeShift : BaseAuditableEntity
{
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("shift_date")]
    public DateTime ShiftDate { get; set; }

    [Column("start_time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeShifts")]
    public virtual Employee Employee { get; set; } = null!;
}
