namespace Dashboard.BussinessLogic.Dtos.ReportDtos;

public class RevenueReportDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public DateTime ReportDate { get; set; }
    public long? BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
}