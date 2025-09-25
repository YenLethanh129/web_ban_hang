using Dashboard.StockWorker.Models;
using Dashboard.StockWorker.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dashboard.Common.Options;

namespace Dashboard.StockWorker.Services
{
    public class LowStockReportTemplateService : INotificationService
    {
        private readonly EmailOptions _email;
        private readonly ILogger<LowStockReportTemplateService> _logger;
        private readonly PurchaseEnrichmentService _purchaseEnrichment;

        public LowStockReportTemplateService(IOptions<EmailOptions> emailOptions, ILogger<LowStockReportTemplateService> logger, PurchaseEnrichmentService purchaseEnrichment)
        {
            _email = emailOptions?.Value ?? new EmailOptions();
            _logger = logger;
            _purchaseEnrichment = purchaseEnrichment;
        }

        public async Task SendStockAlertsAsync(List<StockAlert> alerts)
        {
            if (alerts.Count == 0)
            {
                _logger.LogInformation("No stock alerts to send");
                return;
            }

            var groupedAlerts = alerts.GroupBy(a => a.AlertLevel).ToList();
            
            await SendConsolidatedStockAlertsAsync(alerts);
        }

        public async Task SendLowStockEmailAsync(StockAlert alert)
        {
            await SendConsolidatedStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendCriticalStockEmailAsync(StockAlert alert)
        {
            await SendConsolidatedStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendOutOfStockEmailAsync(StockAlert alert)
        {
            await SendConsolidatedStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendGenericEmailAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            byte[]? attachmentData = null;
            if (attachments != null && attachments.Count > 0)
            {
                var first = attachments.First();
                attachmentData = first.Value;
            }

            await SendEmailAsync(subject, htmlBody, attachmentData);
        }


        private async Task SendConsolidatedStockAlertsAsync(List<StockAlert> alerts)
        {
            var outOfStockCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.OutOfStock);
            var criticalCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.Critical);
            var lowStockCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.Low);

            var subject = GenerateConsolidatedSubject(outOfStockCount, criticalCount, lowStockCount);
            var htmlBody = GenerateConsolidatedEmailBody(alerts, outOfStockCount, criticalCount, lowStockCount);
            
            var excelData = await GenerateExcelData(alerts);

            await SendEmailAsync(subject, htmlBody, excelData);
        }

        private string GenerateConsolidatedSubject(int outOfStock, int critical, int lowStock)
        {
            var priorities = new List<string>();
            
            if (outOfStock > 0)
                priorities.Add($"{outOfStock} hết hàng");
            if (critical > 0)
                priorities.Add($"{critical} khẩn cấp");
            if (lowStock > 0)
                priorities.Add($"{lowStock} sắp hết");

            return $"🚨 [CẢNH BÁO TỒN KHO] {string.Join(" • ", priorities)} - {DateTime.Now:dd/MM/yyyy}";
        }

        private string GenerateConsolidatedEmailBody(List<StockAlert> alerts, int outOfStock, int critical, int lowStock)
        {
            var totalAlerts = alerts.Count;
            var branches = alerts.GroupBy(a => a.BranchName).Count();
            var maxUrgentLevel = alerts.Any(a => a.AlertLevel == StockAlertLevel.OutOfStock) ? "HẾT HÀNG" :
                                alerts.Any(a => a.AlertLevel == StockAlertLevel.Critical) ? "KHẨN CẤP" : "THẤP";

            return $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Báo cáo tồn kho nguyên liệu</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f5f7fa;
        }}
        
        .email-container {{
            max-width: 800px;
            margin: 20px auto;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 20px 40px rgba(0,0,0,0.1);
        }}
        
        .header {{
            background: rgba(255,255,255,0.95);
            padding: 30px;
            text-align: center;
            backdrop-filter: blur(10px);
        }}
        
        .header h1 {{
            color: #2c3e50;
            font-size: 28px;
            margin-bottom: 10px;
            font-weight: 700;
        }}
        
        .header .subtitle {{
            color: #7f8c8d;
            font-size: 16px;
            margin-bottom: 20px;
        }}
        
        .stats-container {{
            display: flex;
            justify-content: space-around;
            margin-top: 20px;
            flex-wrap: wrap;
            gap: 15px;
        }}
        
        .stat-card {{
            background: white;
            padding: 20px;
            border-radius: 12px;
            text-align: center;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
            flex: 1;
            min-width: 120px;
        }}
        
        .stat-number {{
            font-size: 32px;
            font-weight: bold;
            margin-bottom: 5px;
        }}
        
        .stat-label {{
            font-size: 14px;
            color: #7f8c8d;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        
        .out-of-stock {{ color: #e74c3c; }}
        .critical {{ color: #f39c12; }}
        .low-stock {{ color: #3498db; }}
        .total {{ color: #27ae60; }}
        
        .content {{
            background: white;
            padding: 40px;
        }}
        
        .section {{
            margin-bottom: 40px;
        }}
        
        .section-title {{
            font-size: 20px;
            font-weight: 600;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #ecf0f1;
            display: flex;
            align-items: center;
        }}
        
        .icon {{
            margin-right: 10px;
            font-size: 24px;
        }}
        
        .alert-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }}
        
        .alert-card {{
            border-radius: 12px;
            padding: 20px;
            border-left: 5px solid;
            background: #f8f9fa;
            transition: transform 0.2s ease;
        }}
        
        .alert-card:hover {{
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(0,0,0,0.1);
        }}
        
        .alert-card.out-of-stock {{
            border-left-color: #e74c3c;
            background: linear-gradient(135deg, #fdeded 0%, #f8f9fa 100%);
        }}
        
        .alert-card.critical {{
            border-left-color: #f39c12;
            background: linear-gradient(135deg, #fef9e7 0%, #f8f9fa 100%);
        }}
        
        .alert-card.low {{
            border-left-color: #3498db;
            background: linear-gradient(135deg, #e3f2fd 0%, #f8f9fa 100%);
        }}
        
        .alert-header {{
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 15px;
        }}
        
        .ingredient-name {{
            font-size: 18px;
            font-weight: 600;
            color: #2c3e50;
        }}
        
        .alert-badge {{
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        
        .badge-out-of-stock {{
            background: #e74c3c;
            color: white;
        }}
        
        .badge-critical {{
            background: #f39c12;
            color: white;
        }}
        
        .badge-low {{
            background: #3498db;
            color: white;
        }}
        
        .alert-details {{
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            margin-bottom: 15px;
        }}
        
        .detail-item {{
            padding: 8px 12px;
            background: rgba(255,255,255,0.8);
            border-radius: 8px;
        }}
        
        .detail-label {{
            font-size: 12px;
            color: #7f8c8d;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }}
        
        .detail-value {{
            font-size: 16px;
            font-weight: 600;
            color: #2c3e50;
        }}
        
        .urgent-value {{
            color: #e74c3c;
        }}
        
        .branch-name {{
            font-size: 14px;
            color: #8e44ad;
            font-weight: 500;
        }}
        
        .recommendations {{
            background: linear-gradient(135deg, #a8edea 0%, #fed6e3 100%);
            padding: 25px;
            border-radius: 12px;
            margin: 30px 0;
        }}
        
        .recommendations h3 {{
            color: #2c3e50;
            margin-bottom: 15px;
            display: flex;
            align-items: center;
        }}
        
        .recommendations ul {{
            list-style: none;
        }}
        
        .recommendations li {{
            padding: 8px 0;
            display: flex;
            align-items: center;
        }}
        
        .recommendations li::before {{
            content: '✓';
            color: #27ae60;
            font-weight: bold;
            margin-right: 10px;
            font-size: 16px;
        }}
        
        .attachment-notice {{
            background: linear-gradient(135deg, #74b9ff 0%, #0984e3 100%);
            color: white;
            padding: 20px;
            border-radius: 12px;
            text-align: center;
            margin: 20px 0;
        }}
        
        .attachment-notice h3 {{
            margin-bottom: 10px;
            font-size: 18px;
        }}
        
        .footer {{
            background: #2c3e50;
            color: white;
            padding: 25px;
            text-align: center;
        }}
        
        .footer p {{
            margin-bottom: 10px;
        }}
        
        .timestamp {{
            color: #bdc3c7;
            font-size: 14px;
        }}
        
        @media (max-width: 768px) {{
            .email-container {{
                margin: 10px;
            }}
            
            .content {{
                padding: 20px;
            }}
            
            .stats-container {{
                flex-direction: column;
            }}
            
            .alert-grid {{
                grid-template-columns: 1fr;
            }}
            
            .alert-details {{
                grid-template-columns: 1fr;
            }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <h1>🏪 Báo Cáo Tồn Kho Nguyên Liệu</h1>
            <p class='subtitle'>Hệ thống cảnh báo tự động - Cần xử lý ngay</p>
            
            <div class='stats-container'>
                {(outOfStock > 0 ? $@"
                <div class='stat-card'>
                    <div class='stat-number out-of-stock'>{outOfStock}</div>
                    <div class='stat-label'>Hết hàng</div>
                </div>" : "")}
                
                {(critical > 0 ? $@"
                <div class='stat-card'>
                    <div class='stat-number critical'>{critical}</div>
                    <div class='stat-label'>Khẩn cấp</div>
                </div>" : "")}
                
                {(lowStock > 0 ? $@"
                <div class='stat-card'>
                    <div class='stat-number low-stock'>{lowStock}</div>
                    <div class='stat-label'>Sắp hết</div>
                </div>" : "")}
                
                <div class='stat-card'>
                    <div class='stat-number total'>{totalAlerts}</div>
                    <div class='stat-label'>Tổng cảnh báo</div>
                </div>
                
                <div class='stat-card'>
                    <div class='stat-number total'>{branches}</div>
                    <div class='stat-label'>Chi nhánh</div>
                </div>
            </div>
        </div>
        
        <div class='content'>
            <div class='section'>
                <h2 class='section-title'>
                    <span class='icon'>📊</span>
                    Chi Tiết Cảnh Báo
                </h2>
                
                <div class='alert-grid'>
                    {GenerateAlertCards(alerts)}
                </div>
            </div>
            
            <div class='recommendations'>
                <h3>💡 Khuyến Nghị Xử Lý</h3>
                <ul>
                    <li>Kiểm tra file Excel đính kèm để xem chi tiết đầy đủ</li>
                    <li>Liên hệ ngay với nhà cung cấp cho các mặt hàng hết hàng</li>
                    <li>Xem xét điều chuyển hàng từ chi nhánh khác (nếu có)</li>
                    <li>Lưu lại điểm đặt hàng và tồn kho an toàn</li>
                    <li>Kiểm tra dự báo nhu cầu và xu hướng tiêu thụ</li>
                </ul>
            </div>
            
            <div class='attachment-notice'>
                <h3>📎 File Báo Cáo Chi Tiết</h3>
                <p>Vui lòng kiểm tra file Excel đính kèm <strong>""stock-alert-{DateTime.Now:yyyyMMdd}.xlsx""</strong> để xem thông tin chi tiết về tất cả các nguyên liệu cần cảnh báo.</p>
            </div>
        </div>
        
        <div class='footer'>
            <p><strong>Hệ Thống Quản Lý Kho - Tự Động</strong></p>
            <p class='timestamp'>Thời gian tạo báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
            <p class='timestamp'>Báo cáo này được tạo tự động, vui lòng không trả lời email này</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GenerateAlertCards(List<StockAlert> alerts)
        {
            var cards = new StringBuilder();
            
            // Sort alerts by severity and then by days remaining
            var sortedAlerts = alerts.OrderBy(a => a.AlertLevel)
                                   .ThenBy(a => a.DaysRemaining)
                                   .Take(12) // Limit to first 12 for email readability
                                   .ToList();

            foreach (var alert in sortedAlerts)
            {
                var alertClass = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "out-of-stock",
                    StockAlertLevel.Critical => "critical",
                    StockAlertLevel.Low => "low",
                    _ => "low"
                };

                var badgeClass = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "badge-out-of-stock",
                    StockAlertLevel.Critical => "badge-critical",
                    StockAlertLevel.Low => "badge-low",
                    _ => "badge-low"
                };

                var badgeText = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "Hết hàng",
                    StockAlertLevel.Critical => "Khẩn cấp",
                    StockAlertLevel.Low => "Sắp hết",
                    _ => "Thấp"
                };

                cards.AppendLine($@"
                <div class='alert-card {alertClass}'>
                    <div class='alert-header'>
                        <div class='ingredient-name'>{alert.IngredientName}</div>
                        <div class='alert-badge {badgeClass}'>{badgeText}</div>
                    </div>
                    
                    <div class='branch-name'>📍 {alert.BranchName}</div>
                    
                    <div class='alert-details'>
                        <div class='detail-item'>
                            <div class='detail-label'>Tồn kho hiện tại</div>
                            <div class='detail-value {(alert.AlertLevel == StockAlertLevel.OutOfStock ? "urgent-value" : "")}'>{alert.CurrentStock:N1} {alert.Unit}</div>
                        </div>
                        
                        <div class='detail-item'>
                            <div class='detail-label'>Điểm đặt hàng</div>
                            <div class='detail-value'>{alert.ReorderPoint:N1} {alert.Unit}</div>
                        </div>
                        
                        <div class='detail-item'>
                            <div class='detail-label'>Tiêu thụ TB/ngày</div>
                            <div class='detail-value'>{alert.AverageDailyConsumption:N1} {alert.Unit}</div>
                        </div>
                        
                        <div class='detail-item'>
                            <div class='detail-label'>Số ngày còn lại</div>
                            <div class='detail-value {(alert.DaysRemaining <= 3 ? "urgent-value" : "")}'>{alert.DaysRemaining} ngày</div>
                        </div>
                    </div>
                </div>");
            }

            if (alerts.Count > 12)
            {
                cards.AppendLine($@"
                <div class='alert-card low'>
                    <div class='alert-header'>
                        <div class='ingredient-name'>... và {alerts.Count - 12} mục khác</div>
                        <div class='alert-badge badge-low'>Xem Excel</div>
                    </div>
                    <div class='branch-name'>📎 Vui lòng xem file đính kèm để biết chi tiết đầy đủ</div>
                </div>");
            }

            return cards.ToString();
        }

        private async Task<byte[]> GenerateExcelData(List<StockAlert> alerts)
        {
            var csv = new StringBuilder();

            try
            {
                await _purchaseEnrichment.EnrichAsync(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to enrich alerts with purchase/supplier info; continuing without enrichment");
            }

            csv.AppendLine("STT,Chi Nhánh,Tên Nguyên Liệu,Đơn Vị,Tồn Kho Hiện Tại,Điểm Đặt Hàng,Tồn Kho An Toàn,Tiêu Thụ TB/Ngày,Số Ngày Còn Lại,Mức Độ Cảnh Báo,Thời Gian Cập NHật,Nhà Cung Cấp,Liên Hệ Nhà Cung Cấp,Giá Mua Gần Nhất,Ngày Nhập Gần Nhất");
            
            var sortedAlerts = alerts.OrderBy(a => a.AlertLevel)
                                   .ThenBy(a => a.BranchName)
                                   .ThenBy(a => a.IngredientName)
                                   .ToList();

            for (int i = 0; i < sortedAlerts.Count; i++)
            {
                var alert = sortedAlerts[i];
                var alertLevel = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "HẾT HÀNG",
                    StockAlertLevel.Critical => "KHẨN CẤP",
                    StockAlertLevel.Low => "SẮP HẾT",
                    _ => "THẤP"
                };

                var supplier = string.IsNullOrEmpty(alert.SupplierName) ? "" : alert.SupplierName.Replace("\"", "\"\"");
                var supplierContact = string.IsNullOrEmpty(alert.SupplierContact) ? "" : alert.SupplierContact.Replace("\"", "\"\"");
                var lastPrice = alert.LastPurchasePrice.HasValue ? alert.LastPurchasePrice.Value.ToString("N2") : "";
                var lastRestock = alert.LastRestockDate.HasValue ? alert.LastRestockDate.Value.ToString("dd/MM/yyyy") : "";

                csv.AppendLine($"{i + 1}," +
                             $"\"{alert.BranchName}\"," +
                             $"\"{alert.IngredientName}\"," +
                             $"\"{alert.Unit}\"," +
                             $"{alert.CurrentStock:N2}," +
                             $"{alert.ReorderPoint:N2}," +
                             $"{alert.SafetyStock:N2}," +
                             $"{alert.AverageDailyConsumption:N2}," +
                             $"{alert.DaysRemaining}," +
                             $"\"{alertLevel}\"," +
                             $"\"{DateTime.Now:dd/MM/yyyy HH:mm}\"," +
                             $"\"{supplier}\"," +
                             $"\"{supplierContact}\"," +
                             $"{lastPrice}," +
                             $"\"{lastRestock}\"");
            }
            
            // Convert CSV to bytes (UTF-8 with BOM for Excel compatibility)
            var csvBytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
            return csvBytes;
        }

        private async Task SendEmailAsync(string subject, string htmlBody, byte[]? attachmentData = null)
        {
            var dryRun = _email.DryRun;
            if (dryRun)
            {
                try
                {
                    var fileName = $"stockalert_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}.html";
                    var path = Path.Combine(Path.GetTempPath(), fileName);
                    await File.WriteAllTextAsync(path, $"<h1>Subject: {subject}</h1>\n{htmlBody}");
                    _logger.LogInformation("Email dry-run: written HTML to {Path}", path);

                    if (attachmentData != null)
                    {
                        var csvFileName = $"stock-alert-{DateTime.UtcNow:yyyyMMdd}.csv";
                        var csvPath = Path.Combine(Path.GetTempPath(), csvFileName);
                        await File.WriteAllBytesAsync(csvPath, attachmentData);
                        _logger.LogInformation("Email dry-run: written CSV attachment to {Path}", csvPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to write dry-run email to disk");
                }

                return;
            }

            var emailConfig = ValidateEmailConfiguration();
            if (emailConfig == null)
            {
                _logger.LogWarning("Email configuration is invalid. Skipping email send.");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailConfig.FromName, emailConfig.FromEmail));
            
            foreach (var email in emailConfig.ToEmails)
            {
                if (MailboxAddress.TryParse(email, out var mailboxAddress))
                {
                    message.To.Add(mailboxAddress);
                }
                else
                {
                    _logger.LogWarning("Invalid email address: {Email}", email);
                }
            }

            if (!message.To.Any())
            {
                _logger.LogWarning("No valid recipients found. Email not sent.");
                return;
            }
            
            message.Subject = subject;
            
            // Create multipart message with HTML body and attachment
            var multipart = new Multipart("mixed");
            multipart.Add(new TextPart("html") { Text = htmlBody });
            
            if (attachmentData != null)
            {
                var attachment = new MimePart("text", "csv")
                {
                    Content = new MimeContent(new MemoryStream(attachmentData)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = $"stock-alert-{DateTime.Now:yyyyMMdd}.csv"
                };
                multipart.Add(attachment);
            }
            
            message.Body = multipart;

            using var client = new SmtpClient();
            
            try
            {
                _logger.LogInformation("Connecting to SMTP server {Host}:{Port}", emailConfig.SmtpHost, emailConfig.SmtpPort);
                
                await client.ConnectAsync(emailConfig.SmtpHost, emailConfig.SmtpPort, SecureSocketOptions.StartTls);
                
                if (!string.IsNullOrEmpty(emailConfig.Username) && !string.IsNullOrEmpty(emailConfig.Password))
                {
                    _logger.LogInformation("Authenticating with username: {Username}", emailConfig.Username);
                    await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
                }
                
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Consolidated email sent successfully to {RecipientCount} recipients: {Subject}", 
                    message.To.Count, subject);
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                _logger.LogError(ex, "SMTP Authentication failed. Please check your credentials. For Gmail, use App Password.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send consolidated email: {Subject}", subject);
                throw;
            }
        }

        private EmailConfiguration? ValidateEmailConfiguration()
        {
            // Read from _email options and support both nested and flat smtp config
            var smtpHost = _email.Smtp?.Host ?? _email.SmtpHost;
            var smtpPort = _email.Smtp?.Port != 0 ? _email.Smtp!.Port : (_email.SmtpPort != 0 ? _email.SmtpPort : (int?)null);
            var fromEmail = _email.FromEmail ?? _email.AlertsFromAddress;
            var fromName = _email.FromName ?? "Hệ thống quản lý kho";
            var username = _email.Smtp?.Username ?? _email.Username ?? fromEmail;
            var password = _email.AppPassword ?? _email.Password ?? _email.Smtp?.Password;
            var toEmails = _email.AlertRecipients ?? (_email.AlertsToAddress != null ? new[] { _email.AlertsToAddress } : null);

            var missingConfigs = new List<string>();

            if (string.IsNullOrEmpty(smtpHost))
                missingConfigs.Add("Email:SmtpHost");
            
            if (!smtpPort.HasValue || smtpPort <= 0)
                missingConfigs.Add("Email:SmtpPort");
            
            if (string.IsNullOrEmpty(fromEmail))
                missingConfigs.Add("Email:FromEmail");
            
            if (toEmails == null || !toEmails.Any())
                missingConfigs.Add("Email:AlertRecipients");

            if (missingConfigs.Any())
            {
                _logger.LogError("Missing email configuration: {MissingConfigs}", string.Join(", ", missingConfigs));
                return null;
            }

            return new EmailConfiguration
            {
                SmtpHost = smtpHost!,
                SmtpPort = smtpPort!.Value,
                FromEmail = fromEmail!,
                FromName = fromName,
                Username = username,
                Password = password,
                ToEmails = toEmails!
            };
        }

        private class EmailConfiguration
        {
            public string SmtpHost { get; set; } = null!;
            public int SmtpPort { get; set; }
            public string FromEmail { get; set; } = null!;
            public string FromName { get; set; } = null!;
            public string? Username { get; set; }
            public string? Password { get; set; }
            public string[] ToEmails { get; set; } = null!;
        }
    }
}