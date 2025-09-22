using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Dashboard.StockWorker.Services
{
    /// <summary>
    /// Background worker that calculates ingredient consumption from product sales in the last N days
    /// (default 7), compares that need to branch & warehouse stocks and flags low-stock ingredients.
    /// It updates safety_stock in ingredient_warehouse and inventory_thresholds (per branch), and
    /// sends an email alert (if SMTP configured) with the results.
    ///
    /// Design notes:
    /// - Uses direct SQL queries so the worker can run independently of repository abstractions.
    /// - Be conservative with updates: set safety_stock to the calculated consumption value.
    /// - The schedule and lookback window are configurable via appsettings (LowStockAlert section).
    /// </summary>
    public class LowStockAlertWorker : BackgroundService
    {
        private readonly ILogger<LowStockAlertWorker> _logger;
        private readonly IConfiguration _config;
        private readonly TimeSpan _interval;
        private readonly StockCalculationService _stockCalc;
        private readonly INotificationService _notifier;

        public LowStockAlertWorker(ILogger<LowStockAlertWorker> logger, IConfiguration config, StockCalculationService stockCalc, INotificationService notifier)
        {
            _logger = logger;
            _config = config;
            _stockCalc = stockCalc;
            _notifier = notifier;

            var minutes = _config.GetValue<int?>("LowStockAlert:IntervalMinutes") ?? 60;
            _interval = TimeSpan.FromMinutes(Math.Max(1, minutes));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LowStockAlertWorker starting. Interval: {interval} min", _interval.TotalMinutes);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var alerts = await _stockCalc.GetLowStockAlertsAsync();
                    _logger.LogInformation("LowStockAlertWorker found {count} low-stock alerts", alerts.Count);

                    if (alerts.Any())
                    {
                        // Delegate email / notification to the notification service
                        await _notifier.SendStockAlertsAsync(alerts);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // shutting down
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while running low-stock check");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("LowStockAlertWorker stopping.");
        }
    }
}
