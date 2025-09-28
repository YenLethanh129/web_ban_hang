using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ReportDtos;

namespace Dashboard.StockWorker.Models;

public class FinancialReportConfig
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string PeriodName { get; set; } = null!;
    public string ReportType { get; set; } = null!;
}

public class FinancialReportData
{
    public FinancialReportConfig Config { get; set; } = null!;
    public DashboardSummaryDto DashboardSummary { get; set; } = null!;
    public ProfitAnalysisDto ProfitAnalysis { get; set; } = null!;
    public List<FinacialReportDto> RevenueComparison { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}

public class FinancialReportSummary
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public decimal ProfitMargin { get; set; }
    public int TotalOrders { get; set; }
    public int TotalBranches { get; set; }
    public decimal AverageOrderValue { get; set; }
    public string PerformanceIndicator { get; set; } = "Stable"; 
    public decimal RevenueGrowth { get; set; }
    public decimal ProfitGrowth { get; set; }
}