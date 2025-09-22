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

namespace Dashboard.StockWorker.Services
{
    /// <summary>
    /// Simple notification service that supports two modes:
    /// - PickupDirectory (for testing / environments without SMTP): write HTML files to a directory
    /// - SMTP: send real emails using SmtpClient (configured via appsettings: Email section)
    ///
    /// The HTML templates live under Templates/ and can contain placeholders like
    /// {{IngredientName}}, {{CurrentStock}}, {{SafetyStock}}, {{BranchName}}, {{Date}}
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationService>? _logger;

        private readonly bool _usePickup;
        private readonly string? _pickupDirectory;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService>? logger = null)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;

            _usePickup = _configuration.GetValue<bool>("Email:UsePickupDirectory", true);
            _pickupDirectory = _configuration.GetValue<string>("Email:PickupDirectory");

            if (_usePickup && string.IsNullOrWhiteSpace(_pickupDirectory))
            {
                // default to a temp folder inside the project
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

            await SendEmailAsync(_configuration.GetValue<string>("Email:AlertsFromAddress") ?? "noreply@example.com",
                _configuration.GetValue<string>("Email:AlertsToAddress") ?? "admin@example.com",
                subject, html);
        }

        public async Task SendLowStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;

            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = await BuildHtmlFromTemplateAsync(templatePath, alert);
            var subject = $"Low stock: {alert.IngredientName} - {alert.BranchName}";

            await SendEmailAsync(_configuration.GetValue<string>("Email:AlertsFromAddress") ?? "noreply@example.com",
                _configuration.GetValue<string>("Email:AlertsToAddress") ?? "admin@example.com",
                subject, html);
        }

        public async Task SendCriticalStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = await BuildHtmlFromTemplateAsync(templatePath, alert);
            var subject = $"CRITICAL stock: {alert.IngredientName} - {alert.BranchName}";

            await SendEmailAsync(_configuration.GetValue<string>("Email:AlertsFromAddress") ?? "noreply@example.com",
                _configuration.GetValue<string>("Email:AlertsToAddress") ?? "admin@example.com",
                subject, html);
        }

        public async Task SendOutOfStockEmailAsync(StockAlert alert)
        {
            if (alert == null) return;
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "low-stock.html");
            var html = await BuildHtmlFromTemplateAsync(templatePath, alert);
            var subject = $"OUT OF STOCK: {alert.IngredientName} - {alert.BranchName}";

            await SendEmailAsync(_configuration.GetValue<string>("Email:AlertsFromAddress") ?? "noreply@example.com",
                _configuration.GetValue<string>("Email:AlertsToAddress") ?? "admin@example.com",
                subject, html);
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
                    // fallback simple inline html (use same wording as templates/tests)
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

        private async Task SendEmailAsync(string from, string to, string subject, string htmlBody, Dictionary<string, byte[]>? attachments)
        {
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

                if (attachments != null)
                {
                    foreach (var kv in attachments)
                    {
                        try
                        {
                            var attPath = Path.Combine(_pickupDirectory!, SanitizeFileName(kv.Key));
                            await File.WriteAllBytesAsync(attPath, kv.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "Failed to write attachment {name} to pickup dir", kv.Key);
                        }
                    }
                }

                _logger?.LogInformation("Wrote pickup email and {count} attachments to {dir}", attachments?.Count ?? 0, _pickupDirectory);
                return;
            }

            // SMTP path with attachments
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

                var host = _configuration.GetValue<string>("Email:Smtp:Host");
                var port = _configuration.GetValue<int?>("Email:Smtp:Port") ?? 25;
                var enableSsl = _configuration.GetValue<bool>("Email:Smtp:EnableSsl", false);
                var user = _configuration.GetValue<string>("Email:Smtp:Username");
                var pass = _configuration.GetValue<string>("Email:Smtp:Password");

                using var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                };

                if (!string.IsNullOrWhiteSpace(user))
                {
                    smtp.Credentials = new NetworkCredential(user, pass);
                }

                await Task.Run(() => smtp.Send(msg));
                _logger?.LogInformation("Sent email to {to} via SMTP host {host}:{port}", to, host, port);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send email {subject} to {to}", subject, to);
                throw;
            }
        }

        public async Task SendGenericEmailAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null)
        {
            var from = _configuration.GetValue<string>("Email:AlertsFromAddress") ?? "noreply@example.com";
            var to = _configuration.GetValue<string>("Email:AlertsToAddress") ?? "admin@example.com";

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

                if (attachments != null)
                {
                    foreach (var kv in attachments)
                    {
                        try
                        {
                            var attPath = Path.Combine(_pickupDirectory!, SanitizeFileName(kv.Key));
                            await File.WriteAllBytesAsync(attPath, kv.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "Failed to write attachment {name} to pickup dir", kv.Key);
                        }
                    }
                }

                _logger?.LogInformation("Wrote pickup email and {count} attachments to {dir}", attachments?.Count ?? 0, _pickupDirectory);
                return;
            }

            // SMTP path with attachments
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

                var host = _configuration.GetValue<string>("Email:Smtp:Host");
                var port = _configuration.GetValue<int?>("Email:Smtp:Port") ?? 25;
                var enableSsl = _configuration.GetValue<bool>("Email:Smtp:EnableSsl", false);
                var user = _configuration.GetValue<string>("Email:Smtp:Username");
                var pass = _configuration.GetValue<string>("Email:Smtp:Password");

                using var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                };

                if (!string.IsNullOrWhiteSpace(user))
                {
                    smtp.Credentials = new NetworkCredential(user, pass);
                }

                await Task.Run(() => smtp.Send(msg));
                _logger?.LogInformation("Sent email to {to} via SMTP host {host}:{port}", to, host, port);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to send email {subject} to {to}", subject, to);
                throw;
            }
        }

        private static string SanitizeFileName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');
            return input;
        }
    }
}
