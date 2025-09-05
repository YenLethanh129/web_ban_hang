using Dashboard.StockWorker.Models;

namespace Dashboard.StockWorker.Services
{
    public interface INotificationService
    {
        Task SendStockAlertsAsync(List<StockAlert> alerts);
        Task SendLowStockEmailAsync(StockAlert alert);
        Task SendCriticalStockEmailAsync(StockAlert alert);
        Task SendOutOfStockEmailAsync(StockAlert alert);
    }
}
