using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("branches")]
public partial class Branch : BaseAuditableEntity
{
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("manager")]
    [StringLength(100)]
    public string? Manager { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<BranchExpense> BranchExpenses { get; set; } = new List<BranchExpense>();

    [InverseProperty("Branch")]
    public virtual ICollection<BranchIngredientInventory> BranchIngredientInventories { get; set; } = new List<BranchIngredientInventory>();

    [InverseProperty("Branch")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Branch")]
    public virtual ICollection<CogsSummary> ExpensesSummaries { get; set; } = new List<CogsSummary>();

    [InverseProperty("Branch")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty("Branch")]
    public virtual ICollection<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; } = new List<IngredientPurchaseOrder>();

    [InverseProperty("Branch")]
    public virtual ICollection<IngredientTransfer> IngredientTransfers { get; set; } = new List<IngredientTransfer>();

    [InverseProperty("Branch")]
    public virtual ICollection<IngredientTransferRequest> IngredientTransferRequests { get; set; } = new List<IngredientTransferRequest>();

    [InverseProperty("Branch")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Branch")]
    public virtual ICollection<ProfitSummary> ProfitSummaries { get; set; } = new List<ProfitSummary>();

    [InverseProperty("Branch")]
    public virtual ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();

    [InverseProperty("Branch")]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();

    [InverseProperty("Branch")]
    public virtual ICollection<SalesSummary> SalesSummaries { get; set; } = new List<SalesSummary>();
}
