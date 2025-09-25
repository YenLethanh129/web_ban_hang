using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Dashboard.StockWorker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dashboard.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            _logger = logger;

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
            sb.AppendLine("<h2>Stock Alerts</h2>");
            sb.AppendLine("<ul>");
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
            return email.AlertsFromAddress ?? email.FromEmail ?? "noreply@example.com";
        }

        private string GetToAddress()
        {
            var email = _emailOptionsMonitor.CurrentValue;
            // Prefer AlertsToAddress (single), fallback to AlertRecipients array if present
            if (!string.IsNullOrWhiteSpace(email.AlertsToAddress))
                return email.AlertsToAddress!;
            if (email.AlertRecipients != null && email.AlertRecipients.Length > 0)
                return string.Join(",", email.AlertRecipients);
            return "admin@example.com";
        }

        private async Task<string> BuildHtmlFromTemplateAsync(string templatePath, StockAlert alert)
        {
            string template = null!;
            try
            {
                if (File.Exists(templatePath))
                {
                    template = await File.ReadAllTextAsync(templatePath);
                }
                else
                {
                    template = "<h2>Low stock alert</h2><p>Ingredient: {{IngredientName}}</p><p>Branch: {{BranchName}}</p><p>Current stock: {{CurrentStock}}</p><p>Safety stock: {{SafetyStock}}</p><p>Date: {{Date}}</p>";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Could not read template {path}", templatePath);
                template = "<h2>Low stock alert</h2><p>Ingredient: {{IngredientName}}</p><p>Branch: {{BranchName}}</p><p>Current stock: {{CurrentStock}}</p><p>Safety stock: {{SafetyStock}}</p><p>Date: {{Date}}</p>";
            }

            template = template.Replace("{{IngredientName}}", WebUtility.HtmlEncode(alert.IngredientName ?? string.Empty))
                .Replace("{{BranchName}}", WebUtility.HtmlEncode(alert.BranchName ?? string.Empty))
                .Replace("{{CurrentStock}}", alert.CurrentStock.ToString())
                .Replace("{{SafetyStock}}", alert.SafetyStock.ToString())
                .Replace("{{Date}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

            return template;
        }

        private async Task SendEmailAsync(string from, string to, string subject, string htmlBody)
        {
            await SendEmailAsync(from, to, subject, htmlBody, null);
        }

        private async Task SendEmailAsync(string from, string to, string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
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
                    sb.AppendLine($"<!-- To: {to} -->");
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
                sb.AppendLine($"<!-- To: {to} -->");
                sb.AppendLine(htmlBody);
                await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
                _logger?.LogInformation("Wrote pickup email and {count} attachments to {dir}", attachments?.Count ?? 0, _pickupDirectory);
                return;
            }

            try
            {
                using var msg = new MailMessage(from, to, subject, htmlBody) { IsBodyHtml = true };

                if (attachments != null)
                {
                    foreach (var kv in attachments)
                    {
                        var ms = new MemoryStream(kv.Value);
                        var attach = new Attachment(ms, kv.Key);
                        msg.Attachments.Add(attach);
                    }
                }

                // Resolve SMTP settings from options
                var smtpHost = emailCfg.Smtp?.Host ?? emailCfg.SmtpHost;
                var smtpPort = emailCfg.Smtp?.Port ?? emailCfg.SmtpPort;
                var enableSsl = emailCfg.Smtp?.EnableSsl ?? false;
                var user = emailCfg.Smtp?.Username ?? emailCfg.Username;
                var pass = emailCfg.Smtp?.Password ?? emailCfg.AppPassword ?? emailCfg.Password;

                using var smtp = new SmtpClient(smtpHost ?? "localhost", smtpPort == 0 ? 25 : smtpPort)
                {
                    EnableSsl = enableSsl
                };

                if (!string.IsNullOrWhiteSpace(user))
                {
                    smtp.Credentials = new NetworkCredential(user, pass);
                }

                // Send synchronously in Task.Run to avoid blocking threads
                await Task.Run(() => smtp.Send(msg));
                _logger?.LogInformation("Sent email to {to} via SMTP host {host}:{port}", to, smtpHost, smtpPort);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send email {subject} to {to}", subject, to);
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
