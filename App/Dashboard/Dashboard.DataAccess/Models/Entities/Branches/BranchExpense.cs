using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Branches;

[Table("branch_expenses")]
public partial class BranchExpense : BaseAuditableEntity
{
    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("expense_type")]
    [StringLength(100)]
    public string ExpenseType { get; set; } = null!;

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("payment_cycle")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentCycle { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("BranchExpenses")]
    public virtual Branch Branch { get; set; } = null!;
}
