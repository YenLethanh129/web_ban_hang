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

    [ForeignKey("BranchId")]
    [InverseProperty("Employees")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual EmployeePosition Position { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

    [InverseProperty("WarehouseStaff")]
    public virtual ICollection<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new List<GoodsReceivedNote>();

    [InverseProperty("Employee")]
    public virtual ICollection<IngredientPurchaseOrder> IngredientPurchaseOrders { get; set; } = new List<IngredientPurchaseOrder>();

    [InverseProperty("DeliveryPerson")]
    public virtual ICollection<OrderDeliveryTracking> OrderDeliveryTrackings { get; set; } = new List<OrderDeliveryTracking>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeePayroll> Payrolls { get; set; } = new List<EmployeePayroll>();

    [InverseProperty("ApprovedByNavigation")]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();

    [InverseProperty("Employee")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
