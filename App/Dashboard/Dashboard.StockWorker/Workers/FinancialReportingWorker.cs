using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dashboard.BussinessLogic.Services.ReportServices;
using Dashboard.StockWorker.Services;
using Dashboard.StockWorker.Models;
using System.Text;

namespace Dashboard.StockWorker;

public class FinancialReportingWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FinancialReportingWorker> _logger;
    private readonly IConfiguration _configuration;
    private Timer? _timer;

    public FinancialReportingWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FinancialReportingWorker> logger,
        IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalMinutes = _configuration.GetValue<int>("FinancialReporting:IntervalMinutes", 1440); // Default 24 hours
        var interval = TimeSpan.FromMinutes(intervalMinutes);

        _logger.LogInformation("Financial Reporting Worker started with interval: {Interval} minutes", intervalMinutes);

        _timer = new Timer(async _ => await ExecuteReporting(), null, TimeSpan.Zero, interval);

        return Task.CompletedTask;
    }

    private async Task ExecuteReporting()
    {
        try
        {
            _logger.LogInformation("Starting financial reporting process at {Time}", DateTime.Now);

            using var scope = _serviceScopeFactory.CreateScope();
            var reportingService = scope.ServiceProvider.GetRequiredService<IReportingService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var templateService = scope.ServiceProvider.GetRequiredService<IFinancialReportTemplateService>();

            var reportConfig = GetReportConfiguration();
            var reportData = await GenerateFinancialReportData(reportingService, reportConfig);

            // Generate HTML report
            var htmlReport = await templateService.GenerateFinancialReportHtml(reportData);

            // Generate Excel attachments
            var attachments = GenerateExcelAttachments(reportData);

            // Send email
            var subject = $"📊 Báo Cáo Tài Chính Tự Động - {reportConfig.PeriodName} ({DateTime.Now:dd/MM/yyyy})";

            await notificationService.SendGenericEmailAsync(subject, htmlReport, attachments);

            _logger.LogInformation("Financial report sent successfully for period: {Period}", reportConfig.PeriodName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during financial reporting");
        }
    }

    private FinancialReportConfig GetReportConfiguration()
    {
        var reportType = _configuration.GetValue<string>("FinancialReporting:ReportType", "Monthly") ?? "Monthly";
        var customDays = _configuration.GetValue<int>("FinancialReporting:CustomDays", 30);

        DateTime fromDate, toDate;
        string periodName;

        switch (reportType.ToLower())
        {
            case "daily":
                fromDate = DateTime.Today;
                toDate = DateTime.Today.AddDays(1).AddTicks(-1);
                periodName = $"Ngày {DateTime.Today:dd/MM/yyyy}";
                break;
            case "weekly":
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                fromDate = startOfWeek;
                toDate = startOfWeek.AddDays(7).AddTicks(-1);
                periodName = $"Tuần {startOfWeek:dd/MM} - {startOfWeek.AddDays(6):dd/MM/yyyy}";
                break;
            case "yearly":
                fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                toDate = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
                periodName = $"Năm {DateTime.Now.Year}";
                break;
            case "custom":
                fromDate = DateTime.Today.AddDays(-customDays);
                toDate = DateTime.Today.AddTicks(-1);
                periodName = $"{customDays} ngày gần nhất ({fromDate:dd/MM} - {toDate:dd/MM/yyyy})";
                break;
            default: // Monthly
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                toDate = fromDate.AddMonths(1).AddTicks(-1);
                periodName = $"Tháng {DateTime.Now.Month}/{DateTime.Now.Year}";
                break;
        }

        return new FinancialReportConfig
        {
            FromDate = fromDate,
            ToDate = toDate,
            PeriodName = periodName,
            ReportType = reportType
        };
    }

    private async Task<FinancialReportData> GenerateFinancialReportData(
        IReportingService reportingService,
        FinancialReportConfig config)
    {
        _logger.LogInformation("Generating financial report data for period: {From} - {To}", config.FromDate, config.ToDate);

        var dashboardSummary = await reportingService.GetDashboardSummaryAsync(config.FromDate, config.ToDate);

        var profitAnalysis = await reportingService.GetProfitAnalysisAsync(config.FromDate, config.ToDate);

        var threeMonthsAgo = DateTime.Now.AddMonths(-3).Date;
        var revenueComparison = await reportingService.GetRevenueComparisonAsync(
            threeMonthsAgo, config.ToDate, Dashboard.Common.Enums.ReportPeriodEnum.Monthly);

        return new FinancialReportData
        {
            Config = config,
            DashboardSummary = dashboardSummary,
            ProfitAnalysis = profitAnalysis,
            RevenueComparison = revenueComparison,
            GeneratedAt = DateTime.Now
        };
    }

    private Dictionary<string, byte[]> GenerateExcelAttachments(FinancialReportData data)
    {
        var attachments = new Dictionary<string, byte[]>();

        var revenueExcel = GenerateRevenueExcel(data);
        attachments.Add($"revenue-details-{DateTime.Now:yyyyMMdd}.csv", revenueExcel);

        var branchExcel = GenerateBranchPerformanceExcel(data);
        attachments.Add($"branch-performance-{DateTime.Now:yyyyMMdd}.csv", branchExcel);

        var productsExcel = GenerateTopProductsExcel(data);
        attachments.Add($"top-products-{DateTime.Now:yyyyMMdd}.csv", productsExcel);

        var expensesExcel = GenerateExpenseBreakdownExcel(data);
        attachments.Add($"expense-breakdown-{DateTime.Now:yyyyMMdd}.csv", expensesExcel);

        return attachments;
    }

    private byte[] GenerateRevenueExcel(FinancialReportData data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Ngày,Doanh Thu,Chi Phí,Lợi Nhuận,Biên Lợi Nhuận (%)");

        foreach (var report in data.DashboardSummary.FinacialReports.OrderBy(r => r.ReportDate))
        {
            var profitMargin = report.TotalRevenue > 0 ? (report.NetProfit / report.TotalRevenue * 100) : 0;
            csv.AppendLine($"\"{report.ReportDate:dd/MM/yyyy}\",\"{report.TotalRevenue:F0}\",\"{report.TotalExpenses:F0}\",\"{report.NetProfit:F0}\",\"{profitMargin:F2}\"");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    private byte[] GenerateBranchPerformanceExcel(FinancialReportData data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Chi Nhánh,Doanh Thu,Lợi Nhuận,Số Đơn Hàng,Doanh Thu Trung Bình/Đơn,Biên Lợi Nhuận (%)");

        foreach (var branch in data.DashboardSummary.BranchPerformance.OrderByDescending(b => b.Revenue))
        {
            var avgRevenuePerOrder = branch.OrderCount > 0 ? branch.Revenue / branch.OrderCount : 0;
            var profitMargin = branch.Revenue > 0 ? (branch.Profit / branch.Revenue * 100) : 0;
            csv.AppendLine($"\"{branch.BranchName}\",\"{branch.Revenue:F0}\",\"{branch.Profit:F0}\",\"{branch.OrderCount}\",\"{avgRevenuePerOrder:F0}\",\"{profitMargin:F2}\"");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    private byte[] GenerateTopProductsExcel(FinancialReportData data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("STT,Tên Sản Phẩm,Số Lượng Bán,Doanh Thu,Doanh Thu Trung Bình/Sản Phẩm");

        for (int i = 0; i < data.DashboardSummary.TopProducts.Count; i++)
        {
            var product = data.DashboardSummary.TopProducts[i];
            var avgPrice = product.QuantitySold > 0 ? product.Revenue / product.QuantitySold : 0;
            csv.AppendLine($"\"{i + 1}\",\"{product.ProductName}\",\"{product.QuantitySold}\",\"{product.Revenue:F0}\",\"{avgPrice:F0}\"");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    private byte[] GenerateExpenseBreakdownExcel(FinancialReportData data)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Loại Chi Phí,Số Tiền,Tỷ Lệ (%)");

        foreach (var expense in data.ProfitAnalysis.ExpenseBreakdown.OrderByDescending(e => e.Amount))
        {
            csv.AppendLine($"\"{expense.Category}\",\"{expense.Amount:F0}\",\"{expense.Percentage:F2}\"");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}