namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record DailyOrderSummary(DateTime Date, int OrderCount, decimal Revenue);
