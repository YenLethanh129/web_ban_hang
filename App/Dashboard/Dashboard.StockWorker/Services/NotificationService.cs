using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dashboard.StockWorker.Models;
using Dashboard.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Dashboard.StockWorker.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IOptionsMonitor<EmailOptions> _emailOptionsMonitor;
        private readonly ILogger<NotificationService>? _logger;

        private readonly bool _usePickup;
        private readonly string? _pickupDirectory;

        public NotificationService(IOptionsMonitor<EmailOptions> emailOptionsMonitor, ILogger<NotificationService>? logger = null)
        {
            _emailOptionsMonitor = emailOptionsMonitor ?? throw new ArgumentNullException(nameof(emailOptionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(emailOptionsMonitor));

            var email = _emailOptionsMonitor.CurrentValue;
            _usePickup = email.UsePickupDirectory;
            _pickupDirectory = email.PickupDirectory;

            if (_usePickup && string.IsNullOrWhiteSpace(_pickupDirectory))
            {
                _pickupDirectory = Path.Combine(Path.GetTempPath(), "Dashboard.StockWorker.EmailPickup");
            }

            if (_usePickup)
            {
                try
                {
                    Directory.CreateDirectory(_pickupDirectory!);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Could not create pickup directory {dir}", _pickupDirectory);
                }
            }
        }

        public async Task SendStockAlertsAsync(List<StockAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0)
                return;

            var subject = $"Stock alerts ({alerts.Count}) - {DateTime.UtcNow:yyyy-MM-dd}";
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Stock Alerts</h2><ul>");
            foreach (var a in alerts)
            {
                sb.AppendLine($"<li><strong>{a.IngredientName}</strong> - Current: {a.CurrentStock} / Safety: {a.SafetyStock} - Branch: {a.BranchName}</li>");
            }
            sb.AppendLine("</ul>");
            var html = sb.ToString();

            await SendEmailAsync(GetFromAddress(), GetToAddress(), subject, html);
        }

        public async Task SendLowStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = File.Exists(templatePath) ? await File.ReadAllTextAsync(templatePath) : $"<h2>Low stock alert</h2><p>{alert.IngredientName}</p>";
            var subject = $"Low stock: {alert.IngredientName} - {alert.BranchName}";
            await SendEmailAsync(GetFromAddress(), GetToAddress(), subject, html);
        }

        public async Task SendCriticalStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = File.Exists(templatePath) ? await File.ReadAllTextAsync(templatePath) : $"<h2>Critical stock alert</h2><p>{alert.IngredientName}</p>";
            var subject = $"CRITICAL stock: {alert.IngredientName} - {alert.BranchName}";
            await SendEmailAsync(GetFromAddress(), GetToAddress(), subject, html);
        }

        public async Task SendOutOfStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = File.Exists(templatePath) ? await File.ReadAllTextAsync(templatePath) : $"<h2>Out of stock alert</h2><p>{alert.IngredientName}</p>";
            var subject = $"OUT OF STOCK: {alert.IngredientName} - {alert.BranchName}";
            await SendEmailAsync(GetFromAddress(), GetToAddress(), subject, html);
        }

        private string GetFromAddress()
        {
            var email = _emailOptionsMonitor.CurrentValue;
            return string.IsNullOrWhiteSpace(email.FromEmail) ? "noreply@kythuat.vn" : email.FromEmail;
        }

        private string GetToAddress()
        {
            var email = _emailOptionsMonitor.CurrentValue;
            if (email.AlertRecipients != null && email.AlertRecipients.Length > 0)
                return string.Join(",", email.AlertRecipients);
            return "admin@example.com";
        }

        private async Task SendEmailAsync(string from, string toCsv, string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            var emailCfg = _emailOptionsMonitor.CurrentValue;

            // Dry-run: write to disk and return
            if (emailCfg.DryRun)
            {
                try
                {
                    var fileName = SanitizeFileName($"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{subject}") + ".html";
                    var path = Path.Combine(Path.GetTempPath(), fileName);
                    var sb = new StringBuilder();
                    sb.AppendLine($"<!-- Subject: {subject} -->");
                    sb.AppendLine($"<!-- From: {from} -->");
                    sb.AppendLine($"<!-- To: {toCsv} -->");
                    sb.AppendLine(htmlBody);
                    await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
                    _logger?.LogInformation("Dry-run email written to {Path}", path);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Failed to write dry-run email");
                }
                return;
            }

            if (_usePickup)
            {
                var fileName = SanitizeFileName($"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{subject}") + ".html";
                var path = Path.Combine(_pickupDirectory!, fileName);
                var sb = new StringBuilder();
                sb.AppendLine($"<!-- Subject: {subject} -->");
                sb.AppendLine($"<!-- From: {from} -->");
                sb.AppendLine($"<!-- To: {toCsv} -->");
                sb.AppendLine(htmlBody);
                await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
                _logger?.LogInformation("Wrote pickup email to {dir}", _pickupDirectory);
                return;
            }

            // Build MimeMessage
            var message = new MimeMessage();
            try
            {
                message.From.Add(MailboxAddress.Parse(from));
            }
            catch
            {
                message.From.Add(new MailboxAddress("", from));
            }

            foreach (var addr in toCsv.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                try { message.To.Add(MailboxAddress.Parse(addr.Trim())); }
                catch { message.To.Add(new MailboxAddress("", addr.Trim())); }
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

            // Determine SMTP config (prefer Smtp nested)
            var smtpHost = emailCfg.Smtp?.Host ?? emailCfg.SmtpHost;
            var smtpPort = emailCfg.Smtp?.Port ?? emailCfg.SmtpPort;
            var enableSsl = emailCfg.Smtp?.EnableSsl ?? true;
            var username = emailCfg.Smtp?.Username ?? emailCfg.Username;
            // prefer AppPassword then Password
            var password = !string.IsNullOrEmpty(emailCfg.AppPassword) ? emailCfg.AppPassword : (emailCfg.Smtp?.Password ?? emailCfg.Password);

            // Redact for logs
            string Redact(string v) => string.IsNullOrEmpty(v) ? "<empty>" : (v.Length > 4 ? v.Substring(0, 2) + "..." + v.Substring(v.Length - 2) : "****");

            _logger!.LogDebug("SMTP connect {Host}:{Port} user={User}", smtpHost, smtpPort, Redact(username));

            // Send using MailKit
            try
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();

                // Accept all certificates? only for local/dev troubleshooting — don't use in production without thought.
                // client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                var socketOptions = enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                await client.ConnectAsync(smtpHost ?? "localhost", smtpPort == 0 ? 25 : smtpPort, socketOptions);

                if (!string.IsNullOrWhiteSpace(username))
                {
                    try
                    {
                        await client.AuthenticateAsync(username, password ?? string.Empty);
                    }
                    catch (MailKit.Security.AuthenticationException aex)
                    {
                        _logger!.LogError(aex, "SMTP Authentication failed. user={UserMasked}", Redact(username));
                        throw; // rethrow for caller to handle/log
                    }
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger!.LogInformation("Sent email to {To} via SMTP {Host}:{Port}", toCsv, smtpHost, smtpPort);
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "Failed to send mail to {To}", toCsv);
                throw;
            }
        }

        public async Task SendGenericEmailAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            await SendEmailAsync(GetFromAddress(), GetToAddress(), subject, htmlBody, attachments);
        }

        private static string SanitizeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input;
        }
    }
}