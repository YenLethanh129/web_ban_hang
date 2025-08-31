namespace Dashboard.BussinessLogic.Dtos.ReportDtos;

public class DashboardSummaryDto
{
    public decimal TodayRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }
    public decimal TodayProfit { get; set; }
    public decimal MonthlyProfit { get; set; }
    public decimal YearlyProfit { get; set; }
    public decimal TodayExpenses { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal YearlyExpenses { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public List<TopSellingProductDto> TopProducts { get; set; } = new();
    public List<BranchPerformanceDto> BranchPerformance { get; set; } = new();
}

public class TopSellingProductDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class BranchPerformanceDto
{
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Profit { get; set; }
    public int OrderCount { get; set; }
}