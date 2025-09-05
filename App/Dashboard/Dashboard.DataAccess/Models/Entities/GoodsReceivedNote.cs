using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Models.Entities;

[Table("goods_received_notes")]
[Index("GrnCode", Name = "UQ__goods_re__D27DBA9E53D946EE", IsUnique = true)]
public partial class GoodsReceivedNote : BaseAuditableEntity
{
    [Column("grn_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string GrnCode { get; set; } = null!;

    [Column("purchase_order_id")]
    public long? PurchaseOrderId { get; set; }

    [Column("invoice_id")]
    public long? InvoiceId { get; set; }

    [Column("supplier_id")]
    public long SupplierId { get; set; }

    [Column("branch_id")]
    public long BranchId { get; set; }

    [Column("warehouse_staff_id")]
    public long? WarehouseStaffId { get; set; }

    [Column("received_date")]
    [Precision(6)]
    public DateTime? ReceivedDate { get; set; }

    [Column("status_id")]
    public long? StatusId { get; set; }

    [Column("total_quantity_ordered", TypeName = "decimal(18, 2)")]
    public decimal? TotalQuantityOrdered { get; set; }

    [Column("total_quantity_received", TypeName = "decimal(18, 2)")]
    public decimal? TotalQuantityReceived { get; set; }

    [Column("total_quantity_rejected", TypeName = "decimal(18, 2)")]
    public decimal? TotalQuantityRejected { get; set; }

    [Column("delivery_note_number")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DeliveryNoteNumber { get; set; }

    [Column("vehicle_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? VehicleNumber { get; set; }

    [Column("driver_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DriverName { get; set; }

    [Column("note")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual Branch Branch { get; set; } = null!;

    [InverseProperty("Grn")]
    public virtual ICollection<GoodsReceivedDetail> GoodsReceivedDetails { get; set; } = new List<GoodsReceivedDetail>();

    [ForeignKey("InvoiceId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual PurchaseInvoice? Invoice { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual IngredientPurchaseOrder? PurchaseOrder { get; set; }

    [InverseProperty("Grn")]
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();

    [ForeignKey("StatusId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual GoodsReceivedStatus? Status { get; set; }

    [ForeignKey("SupplierId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual Supplier Supplier { get; set; } = null!;

    [ForeignKey("WarehouseStaffId")]
    [InverseProperty("GoodsReceivedNotes")]
    public virtual Employee? WarehouseStaff { get; set; }
}
