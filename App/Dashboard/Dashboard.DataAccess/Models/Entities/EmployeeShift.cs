using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("employee_shifts")]
public partial class EmployeeShift
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("shift_date")]
    public DateOnly ShiftDate { get; set; }

    [Column("start_time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("created_at")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("last_modified")]
    [Precision(6)]
    public DateTime LastModified { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeShifts")]
    public virtual Employee Employee { get; set; } = null!;
}
