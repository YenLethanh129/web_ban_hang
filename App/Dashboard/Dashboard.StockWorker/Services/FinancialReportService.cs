using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.BussinessLogic.Services.ReportServices;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.Common.Enums;
using Microsoft.Extensions.Logging;

namespace Dashboard.StockWorker.Services
{
    /// <summary>
    /// Clean, minimal FinancialReportService so the project can compile.
    /// Provides two lightweight methods used by the worker: GenerateMonthlyReportAsync and GenerateEmailHtmlAsync.
    /// More detailed report construction was removed temporarily to eliminate corrupted content;
    /// it can be expanded later to include full CSV and template placeholder replacement.
    /// </summary>
    public class FinancialReportService
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<FinancialReportService> _logger;

        public FinancialReportService(IReportingService reportingService, ILogger<FinancialReportService> logger)
        {
            _reportingService = reportingService ?? throw new ArgumentNullException(nameof(reportingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<byte[]> GenerateMonthlyReportAsync(DateTime forMonth)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"BÁO CÁO TÀI CHÍNH THÁNG {forMonth:MM/yyyy}");
            sb.AppendLine($"Thời gian tạo:,{DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("Ngày,Doanh thu (VNĐ),Chi phí (VNĐ),Lợi nhuận (VNĐ)");

            try
            {
                var fromDate = new DateTime(forMonth.Year, forMonth.Month, 1);
                var toDate = fromDate.AddMonths(1).AddTicks(-1);
                var daily = await _reportingService.GetRevenueComparisonAsync(fromDate, toDate, 0);
                if (daily != null && daily.Any())
                {
                    foreach (var d in daily.Take(30))
                        sb.AppendLine($"{d.ReportDate:dd/MM/yyyy},{d.TotalRevenue:N0},{d.TotalExpenses:N0},{d.NetProfit:N0}");
                }
                else
                {
                    sb.AppendLine($"{fromDate:dd/MM/yyyy},0,0,0");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get detailed revenue comparison; using placeholder row.");
                sb.AppendLine($"{DateTime.Now:dd/MM/yyyy},0,0,0");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
        }

        public Task<string> GenerateEmailHtmlAsync(DateTime reportMonth, bool isTest = false)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Template", "financialReportEmail.html");
            if (File.Exists(templatePath))
            {
                var t = File.ReadAllText(templatePath, Encoding.UTF8);
                t = t.Replace("{{MonthName}}", reportMonth.ToString("MMMM yyyy"));
                if (isTest) t = t.Replace("{{IsTestBadge}}", "<div style='color: #d6336c;'>[TEST EMAIL]</div>");
                return Task.FromResult(t);
            }

            var html = $"<h1>Báo cáo tài chính {reportMonth:MMMM yyyy}</h1><p>Không tìm thấy template, đây là nội dung mặc định.</p>";
            return Task.FromResult(html);
        }

        // Backwards-compatible synchronous email generation used by existing worker code
        public string GenerateEmailHtml(DateTime reportMonth, DashboardSummaryDto? summary, ProfitAnalysisDto? profitAnalysis, bool isTest = false)
        {
            // Try to load template and replace basic placeholders. If template missing, build a small fallback HTML.
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Template", "financialReportEmail.html");
            string monthName = reportMonth.ToString("MMMM yyyy", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));

            if (File.Exists(templatePath))
            {
                var t = File.ReadAllText(templatePath, Encoding.UTF8);
                t = t.Replace("{{MonthName}}", monthName);
                t = t.Replace("{{IsTestBadge}}", isTest ? "<div style='color:#d6336c;'>[TEST EMAIL]</div>" : string.Empty);
                t = t.Replace("{{AttachmentFilename}}", $"BaoCaoTaiChinh_{reportMonth:yyyyMM}.csv");
                t = t.Replace("{{Timestamp}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                if (summary != null)
                {
                    t = t.Replace("{{TotalRevenue}}", summary.TotalRevenue.ToString("N0"));
                    t = t.Replace("{{TotalExpenses}}", summary.TotalExpenses.ToString("N0"));
                    t = t.Replace("{{NetProfit}}", summary.NetProfit.ToString("N0"));
                    t = t.Replace("{{TotalOrders}}", summary.TotalOrders.ToString("N0"));
                    var profitMargin = summary.TotalRevenue > 0 ? (summary.NetProfit / summary.TotalRevenue * 100) : 0m;
                    t = t.Replace("{{ProfitMargin}}", profitMargin.ToString("F1") + "%");
                }

                return t;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"<h2>{(isTest ? "[TEST] " : string.Empty)}Báo Cáo Tài Chính - {monthName}</h2>");
            if (summary != null)
            {
                sb.AppendLine($"<p>Tổng doanh thu: {summary.TotalRevenue:N0} VNĐ</p>");
                sb.AppendLine($"<p>Tổng chi phí: {summary.TotalExpenses:N0} VNĐ</p>");
                sb.AppendLine($"<p>Lợi nhuận: {summary.NetProfit:N0} VNĐ</p>");
            }
            else
            {
                sb.AppendLine("<p>Không có dữ liệu tóm tắt.</p>");
            }
            sb.AppendLine($"<p>Tệp đính kèm: BaoCaoTaiChinh_{reportMonth:yyyyMM}.csv</p>");
            sb.AppendLine($"<p>Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
            return sb.ToString();
        }

        public async Task<(string htmlContent, byte[] csvAttachment)> GenerateTestReportAsync()
        {
            var currentMonth = DateTime.Now.AddMonths(-1);
            var csv = await GenerateMonthlyReportAsync(currentMonth);
            var fromDate = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var toDate = fromDate.AddMonths(1).AddTicks(-1);
            var summary = await _reportingService.GetDashboardSummaryAsync(fromDate, toDate);
            var profitAnalysis = await _reportingService.GetProfitAnalysisAsync(fromDate, toDate);
            var html = GenerateEmailHtml(currentMonth, summary, profitAnalysis, isTest: true);
            return (html, csv);
        }
    }
}
