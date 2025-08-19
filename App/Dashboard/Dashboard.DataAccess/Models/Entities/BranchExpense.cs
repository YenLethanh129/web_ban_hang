using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("branch_expenses")]
public partial class BranchExpense : BaseAuditableEntity
{

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("expense_type")]
    [StringLength(100)]
    [Unicode(false)]
    public string ExpenseType { get; set; } = null!;

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; set; }

    [Column("payment_cycle")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentCycle { get; set; }

    [Column("note")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }


    [ForeignKey("BranchId")]
    [InverseProperty("BranchExpenses")]
    public virtual Branch Branch { get; set; } = null!;
}
