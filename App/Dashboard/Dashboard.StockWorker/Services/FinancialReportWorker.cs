using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.BussinessLogic.Services.ReportServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dashboard.StockWorker.Services;

public class FinancialReportWorker : BackgroundService
{
    private readonly ILogger<FinancialReportWorker> _logger;
    private readonly FinancialReportService _reportService;
    private readonly INotificationService _notificationService;
    private readonly IReportingService _reportingService;
    private readonly IConfiguration _configuration;

    public FinancialReportWorker(
        ILogger<FinancialReportWorker> logger,
        FinancialReportService reportService,
        INotificationService notificationService,
        IReportingService reportingService,
        IConfiguration configuration)
    {
        _logger = logger;
        _reportService = reportService;
        _reportingService = reportingService;
        _notificationService = notificationService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FinancialReportWorker started.");

        // Allow a one-time test trigger via environment variable for development/testing.
        try
        {
            var sendTest = Environment.GetEnvironmentVariable("SEND_TEST_FINANCIAL_REPORT");
            if (!string.IsNullOrWhiteSpace(sendTest) && sendTest.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("SEND_TEST_FINANCIAL_REPORT=true detected — sending test financial report now.");
                try
                {
                    await SendTestFinancialReport(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending test financial report via env trigger");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to evaluate SEND_TEST_FINANCIAL_REPORT env var");
        }

        // Read schedule from configuration
        var dayOfMonth = _configuration.GetValue("FinancialReport:DayOfMonth", 1);
        var timeOfDay = _configuration.GetValue("FinancialReport:TimeOfDay", "08:00");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                var nextRun = CalculateNextRunTime(now, dayOfMonth, timeOfDay);
                var delay = nextRun - now;

                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;

                _logger.LogInformation("FinancialReportWorker will run next at {NextRun} (UTC)", nextRun);

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

                await GenerateAndSendMonthlyReport(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("FinancialReportWorker was cancelled");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinancialReportWorker execution loop");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("FinancialReportWorker stopped.");
    }

    public async Task GenerateAndSendMonthlyReport(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow;
        var reportMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1); 

        _logger.LogInformation("Generating monthly financial report for {Month}", reportMonth.ToString("yyyy-MM"));

        try
        {
            var csvData = await _reportService.GenerateMonthlyReportAsync(reportMonth);
            var input = new Dashboard.BussinessLogic.Dtos.ReportDtos.GetFinancialReportInput
            {
                FromDate = reportMonth,
                ToDate = reportMonth.AddMonths(1).AddDays(-1),
                Period = Common.Enums.ReportPeriodEnum.Monthly
            };
            DashboardSummaryDto summary = await _reportingService.GetDashboardSummaryAsync(reportMonth);
            var periodEnd = reportMonth.AddMonths(1).AddDays(-1);
            ProfitAnalysisDto profitAnalysis = await _reportingService.GetProfitAnalysisAsync(reportMonth, periodEnd);
            var htmlContent = _reportService.GenerateEmailHtml(reportMonth, summary, profitAnalysis, isTest: false);

            var subject = $"📊 Báo Cáo Tài Chính Tháng {reportMonth.ToString("MM/yyyy", new System.Globalization.CultureInfo("vi-VN"))}";

            var attachments = new Dictionary<string, byte[]>
            {
                { $"BaoCaoTaiChinh_{reportMonth:yyyyMM}.csv", csvData }
            };

            await _notificationService.SendGenericEmailAsync(subject, htmlContent, attachments);

            _logger.LogInformation("Monthly financial report for {Month} sent successfully", reportMonth.ToString("yyyy-MM"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate and send monthly financial report for {Month}", reportMonth.ToString("yyyy-MM"));
            throw;
        }
    }

 
    public async Task SendTestFinancialReport(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating TEST financial report");

        try
        {
            var (htmlContent, csvData) = await _reportService.GenerateTestReportAsync();

            var subject = "🧪 [TEST] Báo Cáo Tài Chính - Thử Nghiệm Hệ Thống";

            var attachments = new Dictionary<string, byte[]>
            {
                { $"Test_FinancialReport_{DateTime.UtcNow:yyyyMMddHHmmss}.csv", csvData }
            };

            await _notificationService.SendGenericEmailAsync(subject, htmlContent, attachments);

            _logger.LogInformation("TEST financial report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send TEST financial report");
            throw;
        }
    }
    private static DateTime CalculateNextRunTime(DateTime nowUtc, int dayOfMonth, string timeOfDay)
    {
        var timeParts = timeOfDay.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;

        var nextMonth = nowUtc.AddMonths(1);
        var targetMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

        var actualDay = Math.Min(dayOfMonth, DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month));

        return new DateTime(targetMonth.Year, targetMonth.Month, actualDay, hour, minute, 0, DateTimeKind.Utc);
    }
}

public static class FinancialReportWorkerExtensions
{
    public static async Task TriggerTestFinancialReport(this FinancialReportWorker worker)
    {
        await worker.SendTestFinancialReport();
    }

    public static async Task TriggerMonthlyReport(this FinancialReportWorker worker)
    {
        await worker.GenerateAndSendMonthlyReport();
    }
}