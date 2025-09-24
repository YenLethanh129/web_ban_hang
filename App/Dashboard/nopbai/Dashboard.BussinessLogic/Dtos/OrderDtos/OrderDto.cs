using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record OrderDto(long Id, int CustomerId, int BranchId, DateTime OrderDate, decimal TotalAmount, decimal TaxAmount, decimal DiscountAmount, decimal FinalAmount, string? Notes, OrderPaymentDto? Payment, OrderShipmentDto? Shipment)
{
    public string OrderCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public string OrderStatusName { get; set; } = string.Empty;
    public string PaymentStatusName { get; set; } = string.Empty;
    public string DeliveryStatusName { get; set; } = string.Empty;
    public List<OrderDetailDto> OrderDetails { get; set; } = new();
}
