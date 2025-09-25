namespace Dashboard.Common.Options;
public class FinancialReportingOptions
{
    public int IntervalMinutes { get; set; } = 1440;
    public string ReportType { get; set; } = string.Empty;
    public int CustomDays { get; set; }
    public bool Enabled { get; set; }
}