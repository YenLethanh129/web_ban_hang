namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record BranchOrderSummary(long BranchId, int TotalOrders, decimal TotalRevenue, decimal AverageOrderValue, DateTime FromDate, DateTime ToDate)
{
    public string BranchName { get; set; } = string.Empty;
}