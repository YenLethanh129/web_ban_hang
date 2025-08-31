namespace Dashboard.BussinessLogic.Dtos.ReportDtos;

public class ProfitAnalysisDto
{
    public decimal GrossProfit { get; set; }
    public decimal OperatingExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public decimal ProfitMargin { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public List<ExpenseBredownDto> ExpenseBreakdown { get; set; } = new();
}


public class ExpenseBredownDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}