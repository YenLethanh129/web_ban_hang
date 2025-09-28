namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record DailyOrderSummary(DateTime Date, int TotalOrders, decimal AverageOrderValue, decimal TotalRevenue);
