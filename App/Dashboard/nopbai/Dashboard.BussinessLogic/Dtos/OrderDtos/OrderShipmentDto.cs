namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record OrderShipmentDto(long Id, decimal ShippingCost, DateTime EstimatedDeliveryDate, DateTime? ActualDeliveryDate)
{
    public string ShippingProviderName { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
}