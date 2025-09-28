using Dashboard.StockWorker.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Dashboard.StockWorker.Services;

public interface IFinancialReportTemplateService
{
    Task<string> GenerateFinancialReportHtml(FinancialReportData data);
}

public class FinancialReportTemplateService : IFinancialReportTemplateService
{
    private readonly ILogger<FinancialReportTemplateService> _logger;

    public FinancialReportTemplateService(ILogger<FinancialReportTemplateService> logger)
    {
        _logger = logger;
    }

    public async Task<string> GenerateFinancialReportHtml(FinancialReportData data)
    {
        try
        {
            // Try to load from file first
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "financial-report.html");
            if (File.Exists(templatePath))
            {
                var template = await File.ReadAllTextAsync(templatePath);
                return ReplaceTemplatePlaceholders(template, data);
            }

            // Fallback to inline template
            var inlineTemplate = GetInlineTemplate();
            return ReplaceTemplatePlaceholders(inlineTemplate, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating financial report HTML");
            return GetSimpleTemplate(data);
        }
    }

    private string ReplaceTemplatePlaceholders(string template, FinancialReportData data)
    {
        var summary = CalculateSummary(data);

        template = template
            .Replace("{{PERIOD_NAME}}", data.Config.PeriodName)
            .Replace("{{GENERATED_DATE}}", data.GeneratedAt.ToString("dd/MM/yyyy HH:mm:ss"))
            .Replace("{{TOTAL_REVENUE}}", FormatCurrency(summary.TotalRevenue))
            .Replace("{{TOTAL_EXPENSES}}", FormatCurrency(summary.TotalExpenses))
            .Replace("{{NET_PROFIT}}", FormatCurrency(summary.NetProfit))
            .Replace("{{PROFIT_MARGIN}}", $"{summary.ProfitMargin:F2}%")
            .Replace("{{TOTAL_ORDERS}}", summary.TotalOrders.ToString("N0"))
            .Replace("{{TOTAL_BRANCHES}}", summary.TotalBranches.ToString())
            .Replace("{{AVG_ORDER_VALUE}}", FormatCurrency(summary.AverageOrderValue))
            .Replace("{{PERFORMANCE_INDICATOR}}", summary.PerformanceIndicator)
            .Replace("{{REVENUE_GROWTH}}", $"{summary.RevenueGrowth:F1}%")
            .Replace("{{PROFIT_GROWTH}}", $"{summary.ProfitGrowth:F1}%")
            .Replace("{{REVENUE_TREND_CARDS}}", GenerateRevenueTrendCards(data))
            .Replace("{{BRANCH_PERFORMANCE_CARDS}}", GenerateBranchPerformanceCards(data))
            .Replace("{{TOP_PRODUCTS_LIST}}", GenerateTopProductsList(data))
            .Replace("{{EXPENSE_BREAKDOWN_CARDS}}", GenerateExpenseBreakdownCards(data))
            .Replace("{{PERFORMANCE_CLASS}}", GetPerformanceClass(summary))
            .Replace("{{PERFORMANCE_COLOR}}", GetPerformanceColor(summary));

        return template;
    }

    private FinancialReportSummary CalculateSummary(FinancialReportData data)
    {
        var summary = new FinancialReportSummary
        {
            TotalRevenue = data.DashboardSummary.TotalRevenue,
            TotalExpenses = data.DashboardSummary.TotalExpenses,
            NetProfit = data.DashboardSummary.NetProfit,
            TotalOrders = data.DashboardSummary.TotalOrders,
            TotalBranches = data.DashboardSummary.BranchPerformance?.Count ?? 0
        };

        summary.ProfitMargin = summary.TotalRevenue > 0 ? (summary.NetProfit / summary.TotalRevenue * 100) : 0;
        summary.AverageOrderValue = summary.TotalOrders > 0 ? (summary.TotalRevenue / summary.TotalOrders) : 0;

        // Calculate month-over-month growth from revenue comparison
        var monthlyData = data.RevenueComparison.Where(r => r.TotalRevenue > 0 || r.TotalExpenses > 0)
                                              .OrderByDescending(r => r.ReportDate)
                                              .Take(2)
                                              .ToList();

        if (monthlyData.Count >= 2)
        {
            var currentMonth = monthlyData[0]; // Most recent month
            var previousMonth = monthlyData[1]; // Previous month

            // Revenue growth: (current - previous) / previous * 100
            summary.RevenueGrowth = previousMonth.TotalRevenue > 0 ?
                ((currentMonth.TotalRevenue - previousMonth.TotalRevenue) / previousMonth.TotalRevenue * 100) : 0;

            // Profit growth: (current - previous) / previous * 100
            summary.ProfitGrowth = previousMonth.NetProfit > 0 ?
                ((currentMonth.NetProfit - previousMonth.NetProfit) / previousMonth.NetProfit * 100) :
                (previousMonth.NetProfit == 0 && currentMonth.NetProfit > 0 ? 100 : 0);
        }
        else
        {
            summary.RevenueGrowth = 0;
            summary.ProfitGrowth = 0;
        }

        // Determine performance indicator
        summary.PerformanceIndicator = summary.ProfitMargin switch
        {
            >= 15 => "Xuất sắc",
            >= 10 => "Tốt",
            >= 5 => "Ổn định",
            _ => "Cần cải thiện"
        };

        return summary;
    }

    private string GetInlineTemplate()
    {
        return @"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Báo Cáo Tài Chính - {{PERIOD_NAME}}</title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #2c3e50;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
        }
        
        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .header {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            padding: 40px;
            text-align: center;
            margin-bottom: 30px;
            box-shadow: 0 20px 40px rgba(0,0,0,0.1);
        }
        
        .header h1 {
            font-size: 2.5em;
            margin-bottom: 10px;
            background: linear-gradient(135deg, #667eea, #764ba2);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
        }
        
        .header .period {
            font-size: 1.2em;
            color: #7f8c8d;
            margin-bottom: 20px;
        }
        
        .summary-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 20px;
            margin: 30px 0;
        }
        
        .summary-card {
            background: white;
            padding: 30px;
            border-radius: 16px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            text-align: center;
            transition: transform 0.3s ease;
        }
        
        .summary-card:hover {
            transform: translateY(-5px);
        }
        
        .summary-card .icon {
            font-size: 3em;
            margin-bottom: 15px;
        }
        
        .summary-card .value {
            font-size: 2em;
            font-weight: bold;
            margin-bottom: 10px;
        }
        
        .summary-card .label {
            color: #7f8c8d;
            font-size: 1.1em;
        }
        
        .revenue { color: #27ae60; }
        .expense { color: #e74c3c; }
        .profit { color: #3498db; }
        .margin { color: #9b59b6; }
        
        .section {
            background: white;
            margin: 30px 0;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
        }
        
        .section-header {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            padding: 25px;
            font-size: 1.5em;
            font-weight: 600;
        }
        
        .section-content {
            padding: 30px;
        }
        
        .card-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
        }
        
        .data-card {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 12px;
            border-left: 5px solid #3498db;
        }
        
        .data-card h4 {
            margin-bottom: 10px;
            color: #2c3e50;
        }
        
        .data-list {
            list-style: none;
        }
        
        .data-list li {
            padding: 8px 0;
            border-bottom: 1px solid #ecf0f1;
            display: flex;
            justify-content: space-between;
        }
        
        .data-list li:last-child {
            border-bottom: none;
        }
        
        .performance-{{PERFORMANCE_CLASS}} {
            background: {{PERFORMANCE_COLOR}};
            color: white;
        }
        
        .footer {
            background: rgba(255, 255, 255, 0.9);
            backdrop-filter: blur(10px);
            border-radius: 16px;
            padding: 30px;
            text-align: center;
            margin-top: 30px;
            color: #7f8c8d;
        }
        
        .alert {
            background: #fff3cd;
            border: 1px solid #ffeaa7;
            border-radius: 8px;
            padding: 15px;
            margin: 15px 0;
        }
        
        @media (max-width: 768px) {
            .container { padding: 10px; }
            .header { padding: 20px; }
            .summary-grid { grid-template-columns: 1fr; }
            .card-grid { grid-template-columns: 1fr; }
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📊 BÁO CÁO TÀI CHÍNH</h1>
            <p class='period'>{{PERIOD_NAME}}</p>
            <p>Được tạo vào: {{GENERATED_DATE}}</p>
        </div>

        <div class='summary-grid'>
            <div class='summary-card'>
                <div class='icon revenue'>💰</div>
                <div class='value revenue'>{{TOTAL_REVENUE}}</div>
                <div class='label'>Tổng Doanh Thu</div>
            </div>
            
            <div class='summary-card'>
                <div class='icon expense'>💸</div>
                <div class='value expense'>{{TOTAL_EXPENSES}}</div>
                <div class='label'>Tổng Chi Phí</div>
            </div>
            
            <div class='summary-card'>
                <div class='icon profit'>📈</div>
                <div class='value profit'>{{NET_PROFIT}}</div>
                <div class='label'>Lợi Nhuận Ròng</div>
            </div>
            
            <div class='summary-card'>
                <div class='icon margin'>⚡</div>
                <div class='value margin'>{{PROFIT_MARGIN}}</div>
                <div class='label'>Biên Lợi Nhuận</div>
            </div>
        </div>

        <div class='section'>
            <div class='section-header'>📊 Tổng Quan Hiệu Suất</div>
            <div class='section-content'>
                <div class='card-grid'>
                    <div class='data-card'>
                        <h4>Thống Kê Đơn Hàng</h4>
                        <ul class='data-list'>
                            <li><span>Tổng số đơn hàng:</span><span>{{TOTAL_ORDERS}}</span></li>
                            <li><span>Giá trị TB/đơn:</span><span>{{AVG_ORDER_VALUE}}</span></li>
                            <li><span>Số chi nhánh:</span><span>{{TOTAL_BRANCHES}}</span></li>
                        </ul>
                    </div>
                    
                    <div class='data-card performance-{{PERFORMANCE_CLASS}}'>
                        <h4>Chỉ Số Hiệu Suất</h4>
                        <ul class='data-list'>
                            <li><span>Đánh giá:</span><span>{{PERFORMANCE_INDICATOR}}</span></li>
                            <li><span>Tăng trưởng DT:</span><span>{{REVENUE_GROWTH}}</span></li>
                            <li><span>Tăng trưởng LN:</span><span>{{PROFIT_GROWTH}}</span></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <div class='section'>
            <div class='section-header'>📈 Xu Hướng Doanh Thu</div>
            <div class='section-content'>
                {{REVENUE_TREND_CARDS}}
            </div>
        </div>

        <div class='section'>
            <div class='section-header'>🏆 Sản Phẩm Bán Chạy</div>
            <div class='section-content'>
                {{TOP_PRODUCTS_LIST}}
            </div>
        </div>

        <div class='section'>
            <div class='section-header'>💸 Cơ Cấu Chi Phí</div>
            <div class='section-content'>
                {{EXPENSE_BREAKDOWN_CARDS}}
            </div>
        </div>


        <div class='footer'>
            <p><strong>Hệ Thống Báo Cáo Tự Động</strong></p>
            <p>📎 Vui lòng kiểm tra các file Excel đính kèm để xem chi tiết đầy đủ</p>
            <p>⚠️ Báo cáo này được tạo tự động, vui lòng không trả lời email này</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GenerateRevenueTrendCards(FinancialReportData data)
    {
        var cards = new StringBuilder();
        var trends = data.RevenueComparison.OrderByDescending(r => r.ReportDate).Take(6).ToList();

        cards.AppendLine("<div class='card-grid'>");
        foreach (var trend in trends)
        {
            var profitMargin = trend.TotalRevenue > 0 ? (trend.NetProfit / trend.TotalRevenue * 100) : 0;
            cards.AppendLine($@"
                <div class='data-card'>
                    <h4>{trend.ReportDate:MM/yyyy}</h4>
                    <ul class='data-list'>
                        <li><span>Doanh thu:</span><span>{FormatCurrency(trend.TotalRevenue)}</span></li>
                        <li><span>Chi phí:</span><span>{FormatCurrency(trend.TotalExpenses)}</span></li>
                        <li><span>Lợi nhuận:</span><span>{FormatCurrency(trend.NetProfit)}</span></li>
                        <li><span>Biên LN:</span><span>{profitMargin:F1}%</span></li>
                    </ul>
                </div>");
        }
        cards.AppendLine("</div>");
        return cards.ToString();
    }

    private string GenerateBranchPerformanceCards(FinancialReportData data)
    {
        var cards = new StringBuilder();
        var branches = data.DashboardSummary.BranchPerformance?.OrderByDescending(b => b.Revenue).Take(8).ToList() ?? new();

        cards.AppendLine("<div class='card-grid'>");
        foreach (var branch in branches)
        {
            var avgOrderValue = branch.OrderCount > 0 ? branch.Revenue / branch.OrderCount : 0;
            var profitMargin = branch.Revenue > 0 ? (branch.Profit / branch.Revenue * 100) : 0;

            cards.AppendLine($@"
                <div class='data-card'>
                    <h4>{branch.BranchName}</h4>
                    <ul class='data-list'>
                        <li><span>Doanh thu:</span><span>{FormatCurrency(branch.Revenue)}</span></li>
                        <li><span>Lợi nhuận:</span><span>{FormatCurrency(branch.Profit)}</span></li>
                        <li><span>Số đơn hàng:</span><span>{branch.OrderCount:N0}</span></li>
                        <li><span>TB/đơn:</span><span>{FormatCurrency(avgOrderValue)}</span></li>
                        <li><span>Biên LN:</span><span>{profitMargin:F1}%</span></li>
                    </ul>
                </div>");
        }
        cards.AppendLine("</div>");
        return cards.ToString();
    }

    private string GenerateTopProductsList(FinancialReportData data)
    {
        var list = new StringBuilder();
        var products = data.DashboardSummary.TopProducts?.Take(10).ToList() ?? new();

        list.AppendLine("<div class='card-grid'>");
        for (int i = 0; i < products.Count; i++)
        {
            var product = products[i];
            var avgPrice = product.QuantitySold > 0 ? product.Revenue / product.QuantitySold : 0;

            list.AppendLine($@"
                <div class='data-card'>
                    <h4>#{i + 1} {product.ProductName}</h4>
                    <ul class='data-list'>
                        <li><span>Số lượng bán:</span><span>{product.QuantitySold:N0}</span></li>
                        <li><span>Doanh thu:</span><span>{FormatCurrency(product.Revenue)}</span></li>
                        <li><span>Giá TB:</span><span>{FormatCurrency(avgPrice)}</span></li>
                    </ul>
                </div>");
        }
        list.AppendLine("</div>");
        return list.ToString();
    }

    private string GenerateExpenseBreakdownCards(FinancialReportData data)
    {
        var cards = new StringBuilder();
        var expenses = data.ProfitAnalysis?.ExpenseBreakdown?.OrderByDescending(e => e.Amount).ToList() ?? new();

        cards.AppendLine("<div class='card-grid'>");
        foreach (var expense in expenses)
        {
            cards.AppendLine($@"
                <div class='data-card'>
                    <h4>{expense.Category}</h4>
                    <ul class='data-list'>
                        <li><span>Số tiền:</span><span>{FormatCurrency(expense.Amount)}</span></li>
                        <li><span>Tỷ lệ:</span><span>{expense.Percentage:F1}%</span></li>
                    </ul>
                </div>");
        }
        cards.AppendLine("</div>");
        return cards.ToString();
    }

    private string GetPerformanceClass(FinancialReportSummary summary)
    {
        return summary.ProfitMargin switch
        {
            >= 15 => "excellent",
            >= 10 => "good",
            >= 5 => "stable",
            _ => "poor"
        };
    }

    private string GetPerformanceColor(FinancialReportSummary summary)
    {
        return summary.ProfitMargin switch
        {
            >= 15 => "linear-gradient(135deg, #27ae60, #2ecc71)",
            >= 10 => "linear-gradient(135deg, #3498db, #5dade2)",
            >= 5 => "linear-gradient(135deg, #f39c12, #f4d03f)",
            _ => "linear-gradient(135deg, #e74c3c, #ec7063)"
        };
    }

    private string FormatCurrency(decimal amount)
    {
        return $"{amount:N0} ₫";
    }

    private string GetSimpleTemplate(FinancialReportData data)
    {
        return $@"
        <h2>Báo Cáo Tài Chính - {data.Config.PeriodName}</h2>
        <p>Tổng doanh thu: {FormatCurrency(data.DashboardSummary.TotalRevenue)}</p>
        <p>Tổng chi phí: {FormatCurrency(data.DashboardSummary.TotalExpenses)}</p>
        <p>Lợi nhuận ròng: {FormatCurrency(data.DashboardSummary.NetProfit)}</p>
        <p>Tổng đơn hàng: {data.DashboardSummary.TotalOrders}</p>
        <p>Được tạo vào: {data.GeneratedAt:dd/MM/yyyy HH:mm:ss}</p>";
    }
}