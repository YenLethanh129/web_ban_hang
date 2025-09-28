using Dashboard.BussinessLogic.Dtos.IngredientDtos;

namespace Dashboard.BussinessLogic.Dtos.ReportDtos;

public class DashboardSummaryDto
{
    public decimal TotalRevenue { get; set; }
    public decimal NetProfit { get; set; }
    public decimal TotalExpenses { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public List<TopSellingProductDto> TopProducts { get; set; } = [];
    public List<FinacialReportDto> FinacialReports { get; set; } = [];
    public List<LowStockIngredientDto> UnderstockIngredients { get; set; } = [];
    public List<BranchPerformanceDto> BranchPerformance { get; set; } = [];
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