using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.StockWorker.Services;
using Dashboard.StockWorker.Models;

namespace Dashboard.StockWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stock Worker Service started at: {time}", DateTimeOffset.Now);

        var intervalMinutes = _configuration.GetValue<int>("StockMonitoring:IntervalMinutes", 30);
        var checkInterval = TimeSpan.FromMinutes(intervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessStockMonitoringAsync();

            _logger.LogInformation("Next stock monitoring check in {Minutes} minutes", intervalMinutes);
            await Task.Delay(checkInterval, stoppingToken);
        }
    }

    private async Task ProcessStockMonitoringAsync()
    {
        using var scope = _serviceProvider.CreateScope();
    var stockCalculationService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        _logger.LogInformation("Starting stock monitoring process at {Time}", DateTime.UtcNow);

        // 1. Lưu tất cả threshold và tồn kho
        _logger.LogInformation("Updating all thresholds and current stock levels");
        await stockCalculationService.CalculateAndUpdateReorderPointsAsync();

        // 2. Kiểm tra các cảnh báo tồn kho
        _logger.LogInformation("Checking for stock alerts");
        var lowStockAlerts = await stockCalculationService.GetLowStockAlertsAsync();
        var outOfStockAlerts = await stockCalculationService.GetOutOfStockAlertsAsync();

        // Combine alerts but avoid duplicates (outOfStockAlerts are a subset of lowStockAlerts)
        var allAlerts = lowStockAlerts
            .Concat(outOfStockAlerts)
            .GroupBy(a => (a.IngredientId, a.BranchId))
            .Select(g => g.OrderByDescending(x => x.AlertLevel).First())
            .ToList();

        if (allAlerts.Any())
        {
            _logger.LogWarning("Found {Count} stock alerts", allAlerts.Count);

            foreach (var alert in allAlerts)
            {
                _logger.LogWarning("Stock Alert - {IngredientName} in Branch {BranchId} - Current Stock: {Current} ROP: {ROP}",
                    alert.IngredientName ?? "Unknown", alert.BranchId, alert.CurrentStock, alert.ReorderPoint);
            }

            // 3. Gửi thông báo nếu có alerts
            await notificationService.SendStockAlertsAsync(allAlerts);
        }
        else
        {
            _logger.LogInformation("No stock alerts found - all inventory levels are healthy");
        }

        _logger.LogInformation("Stock monitoring process completed successfully");
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock Worker Service is starting");
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock Worker Service is stopping");
        await base.StopAsync(cancellationToken);
    }
}
