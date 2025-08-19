using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("branches")]
public partial class Branch : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("manager")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Manager { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<BranchExpense> BranchExpenses { get; set; } = new List<BranchExpense>();

    [InverseProperty("Branch")]
    public virtual ICollection<BranchIngredientInventory> BranchIngredientInventories { get; set; } = new List<BranchIngredientInventory>();

    [InverseProperty("Branch")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Branch")]
    public virtual ICollection<IngredientTransfer> IngredientTransfers { get; set; } = new List<IngredientTransfer>();

    [InverseProperty("Branch")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesSummary> SalesSummaries { get; set; } = new List<SalesSummary>();

    [InverseProperty("Branch")]
    public virtual ICollection<ProfitSummary> ProfitSummaries { get; set; } = new List<ProfitSummary>();

    [InverseProperty("Branch")]
    public virtual ICollection<ExpensesSummary> ExpensesSummaries { get; set; } = new List<ExpensesSummary>();
}
