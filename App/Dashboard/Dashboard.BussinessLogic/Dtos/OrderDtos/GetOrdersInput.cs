using Dashboard.Common.Enums;

namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public class GetOrdersInput : DefaultInput
{
    public string? OrderCode { get; set; }
    public int? CustomerId { get; set; }
    public int? BranchId { get; set; } 
    public int? OrderStatusId { get; set; }
    public int? PaymentStatusId { get; set; }
    public int? DeliveryStatusId { get; set; }
    public SortByEnum? SortBy { get; set; }
    public bool IsDescending { get; set; } = false;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; } 
    public decimal? MinAmount { get; set; } 
    public decimal? MaxAmount { get; set; } 
}
