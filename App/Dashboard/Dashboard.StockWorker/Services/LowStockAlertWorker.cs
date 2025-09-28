using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dashboard.Common.Options;

namespace Dashboard.StockWorker.Services
{
    public class LowStockAlertWorker : BackgroundService
    {
        private readonly ILogger<LowStockAlertWorker> _logger;
        private readonly TimeSpan _interval;
        private readonly StockCalculationService _stockCalc;
        private readonly INotificationService _notifier;

        public LowStockAlertWorker(ILogger<LowStockAlertWorker> logger, IOptions<StockWorkerOptions> stockOptions, StockCalculationService stockCalc, INotificationService notifier)
        {
            _logger = logger;
            _stockCalc = stockCalc;
            _notifier = notifier;

            var minutes = stockOptions?.Value?.CheckIntervalMinutes ?? 60;
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
                        await _notifier.SendStockAlertsAsync(alerts);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
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