using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.DataAccess.Models.Entities.Base;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities.Employees;

[Table("employees")]
public partial class Employee : BaseAuditableEntity
{
    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("full_name")]
    [StringLength(255)]
    public string FullName { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string Address { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("position_id")]
    public long PositionId { get; set; }

    [Column("hire_date")]
    public DateTime? HireDate { get; set; }

    [Column("resign_date")]
    public DateTime? ResignDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; } = "ACTIVE";

    [ForeignKey(nameof(BranchId))]
    [InverseProperty(nameof(Branch.Employees))]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey(nameof(PositionId))]
    [InverseProperty(nameof(EmployeePosition.Employees))]
    public virtual EmployeePosition Position { get; set; } = null!;

    // Quan hệ 1-1
    [InverseProperty(nameof(EmployeeUserAccount.Employee))]
    public virtual EmployeeUserAccount? EmployeeUserAccount { get; set; }

    // Các quan hệ khác giữ nguyên
    [InverseProperty(nameof(EmployeeSalary.Employee))]
    public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();

    [InverseProperty(nameof(EmployeeShift.Employee))]
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

    [InverseProperty(nameof(GoodsReceivedNote.WarehouseStaff))]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty(nameof(IngredientPurchaseOrder.Employee))]
    public virtual ICollection<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; } = new List<IngredientPurchaseOrder>();

    [InverseProperty(nameof(OrderDeliveryTracking.DeliveryPerson))]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty(nameof(EmployeePayroll.Employee))]
    public virtual ICollection<EmployeePayroll> Payrolls { get; set; } = new List<EmployeePayroll>();

    [InverseProperty(nameof(PurchaseReturn.ApprovedByNavigation))]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();
}
