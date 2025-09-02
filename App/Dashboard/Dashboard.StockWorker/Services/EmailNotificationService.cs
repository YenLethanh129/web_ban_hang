using Dashboard.StockWorker.Models;
using Dashboard.StockWorker.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Dashboard.StockWorker.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfiguration configuration, ILogger<EmailNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendStockAlertsAsync(List<StockAlert> alerts)
        {
            if (!alerts.Any())
            {
                _logger.LogInformation("No stock alerts to send");
                return;
            }

            var groupedAlerts = alerts.GroupBy(a => a.AlertLevel);

            foreach (var group in groupedAlerts)
            {
                foreach (var alert in group)
                {
                    switch (alert.AlertLevel)
                    {
                        case StockAlertLevel.Low:
                            await SendLowStockEmailAsync(alert);
                            break;
                        case StockAlertLevel.Critical:
                            await SendCriticalStockEmailAsync(alert);
                            break;
                        case StockAlertLevel.OutOfStock:
                            await SendOutOfStockEmailAsync(alert);
                            break;
                    }
                }
            }
        }

        public async Task SendLowStockEmailAsync(StockAlert alert)
        {
            var subject = $"[CẢNH BÁO] Nguyên liệu sắp hết - {alert.IngredientName}";
            var body = GenerateEmailBody(alert, "Nguyên liệu đang ở mức thấp và cần đặt hàng bổ sung");
            
            await SendEmailAsync(subject, body);
        }

        public async Task SendCriticalStockEmailAsync(StockAlert alert)
        {
            var subject = $"[KHẨN CẤP] Nguyên liệu cần nhập gấp - {alert.IngredientName}";
            var body = GenerateEmailBody(alert, "Nguyên liệu đang ở mức rất thấp, cần nhập hàng ngay");
            
            await SendEmailAsync(subject, body);
        }

        public async Task SendOutOfStockEmailAsync(StockAlert alert)
        {
            var subject = $"[HẾT HÀNG] Nguyên liệu đã hết - {alert.IngredientName}";
            var body = GenerateEmailBody(alert, "Nguyên liệu đã hết hàng hoàn toàn");
            
            await SendEmailAsync(subject, body);
        }

        private string GenerateEmailBody(StockAlert alert, string message)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        .alert-container {{
            font-family: Arial, sans-serif;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
        }}
        .alert-header {{
            background-color: {GetAlertColor(alert.AlertLevel)};
            color: white;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }}
        .alert-details {{
            background-color: #f9f9f9;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 15px;
        }}
        .detail-row {{
            margin-bottom: 8px;
        }}
        .label {{
            font-weight: bold;
            color: #333;
        }}
        .value {{
            color: #666;
        }}
        .urgent {{
            color: #e74c3c;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <div class='alert-container'>
        <div class='alert-header'>
            <h2>Cảnh báo tồn kho nguyên liệu</h2>
            <p>{message}</p>
        </div>
        
        <div class='alert-details'>
            <div class='detail-row'>
                <span class='label'>Chi nhánh:</span>
                <span class='value'>{alert.BranchName}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Nguyên liệu:</span>
                <span class='value'>{alert.IngredientName}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Tồn kho hiện tại:</span>
                <span class='value {(alert.AlertLevel == StockAlertLevel.OutOfStock ? "urgent" : "")}'>{alert.CurrentStock:N2} {alert.Unit}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Điểm đặt hàng:</span>
                <span class='value'>{alert.ReorderPoint:N2} {alert.Unit}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Tồn kho tối thiểu:</span>
                <span class='value'>{alert.SafetyStock:N2} {alert.Unit}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Tiêu thụ trung bình/ngày:</span>
                <span class='value'>{alert.AverageDailyConsumption:N2} {alert.Unit}</span>
            </div>
            <div class='detail-row'>
                <span class='label'>Số ngày còn lại:</span>
                <span class='value {(alert.DaysRemaining <= 3 ? "urgent" : "")}'>{alert.DaysRemaining} ngày</span>
            </div>
        </div>
        
        <div style='margin-top: 20px; padding: 10px; background-color: #fff3cd; border-radius: 5px;'>
            <p><strong>Khuyến nghị:</strong></p>
            <ul>
                <li>Liên hệ nhà cung cấp để đặt hàng ngay</li>
                <li>Kiểm tra lại dự báo nhu cầu</li>
                <li>Xem xét điều chuyển từ chi nhánh khác (nếu có)</li>
            </ul>
        </div>
        
        <div style='margin-top: 15px; font-size: 12px; color: #999;'>
            <p>Email này được gửi tự động bởi hệ thống quản lý kho vào lúc {DateTime.Now:dd/MM/yyyy HH:mm}</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GetAlertColor(StockAlertLevel level)
        {
            return level switch
            {
                StockAlertLevel.Low => "#f39c12",      // Orange
                StockAlertLevel.Critical => "#e74c3c", // Red
                StockAlertLevel.OutOfStock => "#c0392b", // Dark Red
                _ => "#3498db"                          // Blue
            };
        }

        private async Task SendEmailAsync(string subject, string htmlBody)
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = _configuration.GetValue<int>("Email:SmtpPort");
            var username = _configuration["Email:Username"];
            var password = _configuration["Email:Password"];
            var fromEmail = _configuration["Email:FromEmail"];
            var fromName = _configuration["Email:FromName"];
            var toEmails = _configuration.GetSection("Email:AlertRecipients").Get<string[]>();

            if (string.IsNullOrEmpty(smtpHost) || toEmails == null || !toEmails.Any())
            {
                _logger.LogWarning("Email configuration is missing or incomplete");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName ?? "Stock Alert System", fromEmail));
            
            foreach (var email in toEmails)
            {
                message.To.Add(MailboxAddress.Parse(email));
            }
            
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await client.AuthenticateAsync(username, password);
            }
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully: {Subject}", subject);
        }
    }
}
