using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dashboard.StockWorker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dashboard.StockWorker.Workers
{
    public class StockAlertWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<StockAlertWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _checkInterval;

        public StockAlertWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<StockAlertWorker> logger,
            IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var intervalMinutes = _configuration.GetValue("StockWorker:CheckIntervalMinutes", 60);
            _checkInterval = TimeSpan.FromMinutes(Math.Max(1, intervalMinutes));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UnifiedStockAlertWorker started with interval: {Interval} minutes", _checkInterval.TotalMinutes);

            await ProcessStockMonitoringAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_checkInterval, stoppingToken);
                    await ProcessStockMonitoringAsync();
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("UnifiedStockAlertWorker is stopping (cancellation requested)");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in stock monitoring loop - will retry after interval");
                }
            }

            _logger.LogInformation("UnifiedStockAlertWorker stopped");
        }

        private async Task ProcessStockMonitoringAsync()
        {
            _logger.LogInformation("Starting stock monitoring process at {Time}", DateTime.UtcNow);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var stockCalculationService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                _logger.LogInformation("Updating reorder points and stock levels");
                await stockCalculationService.CalculateAndUpdateReorderPointsAsync();

                _logger.LogInformation("Checking for stock alerts");
                var lowStockAlerts = await stockCalculationService.GetLowStockAlertsAsync();
                var outOfStockAlerts = await stockCalculationService.GetOutOfStockAlertsAsync();

                var allAlerts = lowStockAlerts
                    .Concat(outOfStockAlerts)
                    .GroupBy(a => (a.IngredientId, a.BranchId))
                    .Select(g => g.OrderByDescending(x => x.AlertLevel).First())
                    .ToList();

                if (allAlerts.Any())
                {
                    _logger.LogWarning("Found {Count} stock alerts across {Branches} branches",
                        allAlerts.Count,
                        allAlerts.Select(a => a.BranchId).Distinct().Count());

                    var alertsByLevel = allAlerts.GroupBy(a => a.AlertLevel);
                    foreach (var group in alertsByLevel)
                    {
                        _logger.LogWarning("  - {Level}: {Count} items", group.Key, group.Count());
                    }

                    await notificationService.SendStockAlertsAsync(allAlerts);
                    _logger.LogInformation("Stock alerts sent successfully");
                }
                else
                {
                    _logger.LogInformation("No stock alerts found - all inventory levels are healthy");
                }

                _logger.LogInformation("Stock monitoring process completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during stock monitoring process");
                throw;
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UnifiedStockAlertWorker is starting");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UnifiedStockAlertWorker is stopping");
            return base.StopAsync(cancellationToken);
        }
    }
}