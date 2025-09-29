using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Common.Options;
using Dashboard.StockWorker.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dashboard.StockWorker.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IOptionsMonitor<EmailOptions> _emailOptionsMonitor;
        private readonly ILogger<NotificationService> _logger;
        private readonly PurchaseEnrichmentService _purchaseEnrichment;

        public NotificationService(
            IOptionsMonitor<EmailOptions> emailOptionsMonitor,
            ILogger<NotificationService> logger,
            PurchaseEnrichmentService purchaseEnrichment)
        {
            _emailOptionsMonitor = emailOptionsMonitor ?? throw new ArgumentNullException(nameof(emailOptionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _purchaseEnrichment = purchaseEnrichment ?? throw new ArgumentNullException(nameof(purchaseEnrichment));
        }

        public async Task SendStockAlertsAsync(List<StockAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0)
            {
                _logger.LogInformation("No stock alerts to send");
                return;
            }

            var emailOptions = _emailOptionsMonitor.CurrentValue;

            if (emailOptions.UseAdvancedNotifications)
            {
                await SendConsolidatedStockAlertsAsync(alerts);
            }
            else
            {
                await SendBasicStockAlertsAsync(alerts);
            }
        }

        public async Task SendLowStockEmailAsync(StockAlert alert)
        {
            await SendStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendCriticalStockEmailAsync(StockAlert alert)
        {
            await SendStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendOutOfStockEmailAsync(StockAlert alert)
        {
            await SendStockAlertsAsync(new List<StockAlert> { alert });
        }

        public async Task SendGenericEmailAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            await SendEmailInternalAsync(subject, htmlBody, attachments);
        }

        #region Private Methods - Stock Alerts

        private async Task SendBasicStockAlertsAsync(List<StockAlert> alerts)
        {
            var subject = $"Stock alerts ({alerts.Count}) - {DateTime.UtcNow:yyyy-MM-dd}";
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Stock Alerts</h2><ul>");
            foreach (var a in alerts)
            {
                sb.AppendLine($"<li><strong>{a.IngredientName}</strong> - Current: {a.CurrentStock} / Safety: {a.SafetyStock} - Branch: {a.BranchName}</li>");
            }
            sb.AppendLine("</ul>");

            await SendEmailInternalAsync(subject, sb.ToString());
        }

        private async Task SendConsolidatedStockAlertsAsync(List<StockAlert> alerts)
        {
            var outOfStockCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.OutOfStock);
            var criticalCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.Critical);
            var lowStockCount = alerts.Count(a => a.AlertLevel == StockAlertLevel.Low);

            var subject = GenerateConsolidatedSubject(outOfStockCount, criticalCount, lowStockCount);
            var htmlBody = GenerateConsolidatedEmailBody(alerts, outOfStockCount, criticalCount, lowStockCount);

            try
            {
                await _purchaseEnrichment.EnrichAsync(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to enrich alerts with purchase/supplier info");
            }

            var excelData = GenerateStockAlertExcel(alerts);
            var attachments = new Dictionary<string, byte[]>
            {
                [$"stock-alert-{DateTime.Now:yyyyMMdd}.csv"] = excelData
            };

            await SendEmailInternalAsync(subject, htmlBody, attachments);
        }

        private string GenerateConsolidatedSubject(int outOfStock, int critical, int lowStock)
        {
            var priorities = new List<string>();

            if (outOfStock > 0) priorities.Add($"{outOfStock} hết hàng");
            if (critical > 0) priorities.Add($"{critical} khẩn cấp");
            if (lowStock > 0) priorities.Add($"{lowStock} sắp hết");

            return $"🚨 [CẢNH BÁO TỒN KHO] {string.Join(" • ", priorities)} - {DateTime.Now:dd/MM/yyyy}";
        }

        private string GenerateConsolidatedEmailBody(List<StockAlert> alerts, int outOfStock, int critical, int lowStock)
        {
            var totalAlerts = alerts.Count;
            var branches = alerts.GroupBy(a => a.BranchName).Count();

            var html = $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Báo cáo tồn kho</title>
    <style>
        body {{ font-family: 'Segoe UI', sans-serif; background: #f5f7fa; margin: 0; padding: 20px; }}
        .container {{ max-width: 800px; margin: 0 auto; background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .header h1 {{ margin: 0 0 10px 0; font-size: 24px; }}
        .stats {{ display: flex; justify-content: space-around; padding: 20px; background: #f8f9fa; }}
        .stat {{ text-align: center; }}
        .stat-number {{ font-size: 32px; font-weight: bold; }}
        .stat-label {{ font-size: 12px; color: #7f8c8d; text-transform: uppercase; }}
        .out-of-stock {{ color: #e74c3c; }}
        .critical {{ color: #f39c12; }}
        .low-stock {{ color: #3498db; }}
        .content {{ padding: 30px; }}
        .alert-card {{ background: #f8f9fa; padding: 15px; margin: 10px 0; border-radius: 8px; border-left: 4px solid #3498db; }}
        .alert-card.out-of-stock {{ border-left-color: #e74c3c; }}
        .alert-card.critical {{ border-left-color: #f39c12; }}
        .footer {{ background: #2c3e50; color: white; padding: 20px; text-align: center; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏪 Báo Cáo Tồn Kho Nguyên Liệu</h1>
            <p>Cần xử lý ngay</p>
        </div>
        
        <div class='stats'>
            {(outOfStock > 0 ? $"<div class='stat'><div class='stat-number out-of-stock'>{outOfStock}</div><div class='stat-label'>Hết hàng</div></div>" : "")}
            {(critical > 0 ? $"<div class='stat'><div class='stat-number critical'>{critical}</div><div class='stat-label'>Khẩn cấp</div></div>" : "")}
            {(lowStock > 0 ? $"<div class='stat'><div class='stat-number low-stock'>{lowStock}</div><div class='stat-label'>Sắp hết</div></div>" : "")}
            <div class='stat'><div class='stat-number'>{totalAlerts}</div><div class='stat-label'>Tổng</div></div>
            <div class='stat'><div class='stat-number'>{branches}</div><div class='stat-label'>Chi nhánh</div></div>
        </div>
        
        <div class='content'>
            <h3>Chi Tiết Cảnh Báo</h3>
            {GenerateAlertCardsHtml(alerts.OrderBy(a => a.AlertLevel).ThenBy(a => a.DaysRemaining).Take(10).ToList())}
            {(alerts.Count > 10 ? $"<p><em>... và {alerts.Count - 10} mục khác. Xem file đính kèm.</em></p>" : "")}
            
            <div style='background: #e3f2fd; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                <h4>📎 File Báo Cáo Chi Tiết</h4>
                <p>File Excel đính kèm: <strong>stock-alert-{DateTime.Now:yyyyMMdd}.csv</strong></p>
            </div>
        </div>
        
        <div class='footer'>
            <p>Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
            <p>Đây là email tự động, vui lòng không trả lời</p>
        </div>
    </div>
</body>
</html>";

            return html;
        }

        private string GenerateAlertCardsHtml(List<StockAlert> alerts)
        {
            var sb = new StringBuilder();
            foreach (var alert in alerts)
            {
                var alertClass = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "out-of-stock",
                    StockAlertLevel.Critical => "critical",
                    _ => ""
                };

                var badge = alert.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "Hết hàng",
                    StockAlertLevel.Critical => "Khẩn cấp",
                    _ => "Sắp hết"
                };

                sb.AppendLine($@"
                <div class='alert-card {alertClass}'>
                    <strong>{alert.IngredientName}</strong> <span style='float:right;'>{badge}</span>
                    <div>📍 {alert.BranchName}</div>
                    <div>Tồn kho: {alert.CurrentStock:N1} {alert.Unit} | Điểm đặt hàng: {alert.ReorderPoint:N1} {alert.Unit}</div>
                    <div>Tiêu thụ TB: {alert.AverageDailyConsumption:N1} {alert.Unit}/ngày | Còn lại: {alert.DaysRemaining} ngày</div>
                </div>");
            }
            return sb.ToString();
        }

        private byte[] GenerateStockAlertExcel(List<StockAlert> alerts)
        {
            var csv = new StringBuilder();
            csv.AppendLine("STT,Chi Nhánh,Nguyên Liệu,Đơn Vị,Tồn Kho,Điểm Đặt Hàng,Tồn An Toàn,Tiêu Thụ TB/Ngày,Còn Lại (ngày),Mức Độ,NCC,Liên Hệ NCC,Giá Gần Nhất,Ngày Nhập Gần Nhất");

            var sorted = alerts.OrderBy(a => a.AlertLevel).ThenBy(a => a.BranchName).ThenBy(a => a.IngredientName).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                var a = sorted[i];
                var level = a.AlertLevel switch
                {
                    StockAlertLevel.OutOfStock => "HẾT HÀNG",
                    StockAlertLevel.Critical => "KHẨN CẤP",
                    _ => "SẮP HẾT"
                };

                csv.AppendLine($"{i + 1}," +
                    $"\"{a.BranchName}\"," +
                    $"\"{a.IngredientName}\"," +
                    $"\"{a.Unit}\"," +
                    $"{a.CurrentStock:N2}," +
                    $"{a.ReorderPoint:N2}," +
                    $"{a.SafetyStock:N2}," +
                    $"{a.AverageDailyConsumption:N2}," +
                    $"{a.DaysRemaining}," +
                    $"\"{level}\"," +
                    $"\"{a.SupplierName ?? ""}\"," +
                    $"\"{a.SupplierContact ?? ""}\"," +
                    $"{(a.LastPurchasePrice.HasValue ? a.LastPurchasePrice.Value.ToString("N2") : "")}," +
                    $"\"{(a.LastRestockDate.HasValue ? a.LastRestockDate.Value.ToString("dd/MM/yyyy") : "")}\"");
            }

            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
        }

        #endregion

        #region Private Methods - Email Sending

        private async Task SendEmailInternalAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            var emailCfg = _emailOptionsMonitor.CurrentValue;

            if (emailCfg.DryRun)
            {
                await WriteDryRunEmail(subject, htmlBody, attachments);
                return;
            }

            if (emailCfg.UsePickupDirectory)
            {
                await WritePickupEmail(subject, htmlBody, emailCfg.PickupDirectory);
                return;
            }

            await SendViaSmtpAsync(subject, htmlBody, attachments, emailCfg);
        }

        private async Task WriteDryRunEmail(string subject, string htmlBody, Dictionary<string, byte[]>? attachments)
        {
            try
            {
                var fileName = SanitizeFileName($"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{subject}") + ".html";
                var path = Path.Combine(Path.GetTempPath(), fileName);
                await File.WriteAllTextAsync(path, $"<!-- Subject: {subject} -->\n{htmlBody}", Encoding.UTF8);
                _logger.LogInformation("Dry-run email written to {Path}", path);

                if (attachments != null)
                {
                    foreach (var att in attachments)
                    {
                        var attPath = Path.Combine(Path.GetTempPath(), att.Key);
                        await File.WriteAllBytesAsync(attPath, att.Value);
                        _logger.LogInformation("Dry-run attachment written to {Path}", attPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write dry-run email");
            }
        }

        private async Task WritePickupEmail(string subject, string htmlBody, string? pickupDir)
        {
            var directory = string.IsNullOrWhiteSpace(pickupDir)
                ? Path.Combine(Path.GetTempPath(), "Dashboard.EmailPickup")
                : pickupDir;

            try
            {
                Directory.CreateDirectory(directory);
                var fileName = SanitizeFileName($"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{subject}") + ".html";
                var path = Path.Combine(directory, fileName);
                await File.WriteAllTextAsync(path, $"<!-- Subject: {subject} -->\n{htmlBody}", Encoding.UTF8);
                _logger.LogInformation("Pickup email written to {Path}", path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write pickup email to {Dir}", directory);
            }
        }

        private async Task SendViaSmtpAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments, EmailOptions emailCfg)
        {
            var message = BuildMimeMessage(subject, htmlBody, attachments, emailCfg);

            var smtpHost = emailCfg.Smtp?.Host ?? emailCfg.SmtpHost ?? "localhost";
            var smtpPort = emailCfg.Smtp?.Port ?? emailCfg.SmtpPort;
            if (smtpPort == 0) smtpPort = 587; 

            var enableSsl = emailCfg.Smtp?.EnableSsl ?? true;
            var username = emailCfg.Smtp?.Username ?? emailCfg.Username ?? emailCfg.FromEmail;

            var password = !string.IsNullOrEmpty(emailCfg.AppPassword)
                ? emailCfg.AppPassword
                : (emailCfg.Smtp?.Password ?? emailCfg.Password ?? string.Empty);

            _logger.LogInformation("Sending email via SMTP {Host}:{Port} (SSL={SSL}) as {User}",
                smtpHost, smtpPort, enableSsl, MaskSensitive(username));

            try
            {
                using var client = new SmtpClient();

                var socketOptions = enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                await client.ConnectAsync(smtpHost, smtpPort, socketOptions);

                if (!string.IsNullOrWhiteSpace(username))
                {
                    try
                    {
                        await client.AuthenticateAsync(username, password);
                        _logger.LogInformation("SMTP authentication successful");
                    }
                    catch (MailKit.Security.AuthenticationException aex)
                    {
                        _logger.LogError(aex, "SMTP Authentication FAILED for user {User}. " +
                            "For Gmail, ensure you're using App Password (not regular password). " +
                            "Generate at: https://myaccount.google.com/apppasswords",
                            MaskSensitive(username));
                        throw;
                    }
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully: {Subject}", subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via SMTP {Host}:{Port}", smtpHost, smtpPort);
                throw;
            }
        }

        private MimeMessage BuildMimeMessage(string subject, string htmlBody, Dictionary<string, byte[]>? attachments, EmailOptions emailCfg)
        {
            var message = new MimeMessage();

            var fromEmail = emailCfg.FromEmail ?? "noreply@example.com";
            var fromName = emailCfg.FromName ?? "Dashboard System";
            message.From.Add(new MailboxAddress(fromName, fromEmail));

            var recipients = emailCfg.AlertRecipients ?? new[] { "admin@example.com" };
            foreach (var addr in recipients)
            {
                if (!string.IsNullOrWhiteSpace(addr))
                {
                    try { message.To.Add(MailboxAddress.Parse(addr.Trim())); }
                    catch { message.To.Add(new MailboxAddress("", addr.Trim())); }
                }
            }

            if (!message.To.Any())
            {
                _logger.LogWarning("No valid recipients found, using fallback");
                message.To.Add(new MailboxAddress("", "admin@example.com"));
            }

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

            if (attachments != null)
            {
                foreach (var kv in attachments)
                {
                    bodyBuilder.Attachments.Add(kv.Key, kv.Value);
                }
            }

            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }

        private static string SanitizeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input;
        }

        private static string MaskSensitive(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "<empty>";
            if (value.Length <= 4) return "****";
            return value.Substring(0, 2) + "***" + value.Substring(value.Length - 2);
        }

        #endregion
    }
}