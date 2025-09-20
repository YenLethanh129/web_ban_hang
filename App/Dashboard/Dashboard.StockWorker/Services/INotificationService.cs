using Dashboard.StockWorker.Models;

namespace Dashboard.StockWorker.Services
{
    public interface INotificationService
    {
        Task SendStockAlertsAsync(List<StockAlert> alerts);
        Task SendLowStockEmailAsync(StockAlert alert);
        Task SendCriticalStockEmailAsync(StockAlert alert);
        Task SendOutOfStockEmailAsync(StockAlert alert);
        Task SendGenericEmailAsync(string subject, string htmlBody, Dictionary<string, byte[]>? attachments = null);
    }
}
