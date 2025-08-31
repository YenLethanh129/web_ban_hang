using Dashboard.StockWorker.Services;

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
            try
            {
                await ProcessStockMonitoringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during stock monitoring process");
            }

            _logger.LogInformation("Next stock monitoring check in {Minutes} minutes", intervalMinutes);
            await Task.Delay(checkInterval, stoppingToken);
        }
    }

    private async Task ProcessStockMonitoringAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var stockCalculationService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<EmailNotificationService>();

        _logger.LogInformation("Starting stock monitoring process at {Time}", DateTime.UtcNow);

        try
        {
            // 1. Cập nhật tất cả threshold và tồn kho
            _logger.LogInformation("Updating all thresholds and current stock levels");
            await stockCalculationService.CalculateAndUpdateReorderPointsAsync();

            // 2. Kiểm tra các cảnh báo tồn kho
            _logger.LogInformation("Checking for stock alerts");
            var lowStockAlerts = await stockCalculationService.GetLowStockAlertsAsync();
            var outOfStockAlerts = await stockCalculationService.GetOutOfStockAlertsAsync();
            
            var allAlerts = new List<Dashboard.DataAccess.Models.Entities.InventoryThreshold>();
            allAlerts.AddRange(lowStockAlerts);
            allAlerts.AddRange(outOfStockAlerts);

            if (allAlerts.Any())
            {
                _logger.LogWarning("Found {Count} stock alerts", allAlerts.Count);
                
                foreach (var alert in allAlerts)
                {
                    _logger.LogWarning("Stock Alert - {IngredientName} in Branch {BranchId} - Current Stock vs ROP: {ROP}", 
                        alert.Ingredient?.Name ?? "Unknown", alert.BranchId, alert.ReorderPoint);
                }

                // 3. Gửi thông báo nếu có alerts (tạm thời comment để tránh lỗi)
                _logger.LogInformation("Stock alerts found but email notifications temporarily disabled");
                // TODO: Convert InventoryThreshold to StockAlert for email notifications
                // await notificationService.SendStockAlertsAsync(alerts);
            }
            else
            {
                _logger.LogInformation("No stock alerts found - all inventory levels are healthy");
            }

            _logger.LogInformation("Stock monitoring process completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during stock monitoring process");
        }
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
