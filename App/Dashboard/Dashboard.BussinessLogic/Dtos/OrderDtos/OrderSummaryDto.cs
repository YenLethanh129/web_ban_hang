namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record OrderSummaryDto(int TotalOrders, decimal TotalRevenue, decimal AverageOrderValue, int PendingOrders, int CompletedOrders, int CancelledOrders)
{
    public List<DailyOrderSummary> DailySummary { get; set; } = new();
    public List<BranchOrderSummary> BranchSummary { get; set; } = new();
}
