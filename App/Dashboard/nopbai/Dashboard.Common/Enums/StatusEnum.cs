using System.ComponentModel;

namespace Dashboard.Common.Enums;
public enum PurchaseOrderStatusEnum
{
    [Description("Đơn đặt hàng đang chờ xử lý")]
    Pending = 1,

    [Description("Đơn đặt hàng đã được xác nhận")]
    Confirmed = 2,

    [Description("Đơn đặt hàng đang được xử lý")]
    Processing = 3,

    [Description("Đơn đặt hàng đã được giao")]
    Shipped = 4,

    [Description("Đơn đặt hàng đã giao thành công")]
    Delivered = 5,

    [Description("Đơn đặt hàng đã bị hủy")]
    Cancelled = 6,

    [Description("Đơn đặt hàng đã được trả lại")]
    Returned = 7
}

public enum OrderStatusEnum
{
    Pending = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    Returned = 7,
}

public enum PaymentStatusEnum
{
    Pending = 1,
    Paid = 2,
    Refunded = 3,
    Voided = 4
}

public enum DeliveryStatusEnum
{
    Pending = 1,
    InTransit = 2,
    OutForDelivery = 3,
    Delivered = 4,
    DeliveryFailed = 5,
    Returned = 6
}

public enum InvoiceStatusEnum
{
    [Description("Hóa đơn chờ xử lý")]
    Pending = 1,

    [Description("Hóa đơn đã duyệt")]
    Approved = 2,

    [Description("Hóa đơn đã thanh toán")]
    Paid = 3,

    [Description("Hóa đơn thanh toán một phần")]
    PartiallyPaid = 4,

    [Description("Hóa đơn quá hạn")]
    Overdue = 5,

    [Description("Hóa đơn đã hủy")]
    Cancelled = 6,

    [Description("Hóa đơn có tranh chấp")]
    Disputed = 7
}

public enum GoodsReceivedStatusEnum
{
    [Description("Chờ nhận hàng")]
    Pending = 1,

    [Description("Đang kiểm tra hàng")]
    InProgress = 2,

    [Description("Đã nhận hàng hoàn tất")]
    Completed = 3,

    [Description("Nhận hàng một phần")]
    PartiallyReceived = 4,

    [Description("Từ chối nhận hàng")]
    Rejected = 5,

    [Description("Tạm dừng nhận hàng")]
    OnHold = 6
}

public enum PaymentMethodEnum
{
    Cash = 1,
    Card = 2,
    BankTransfer = 3,
    EWallet = 4,
    COD = 5
}
