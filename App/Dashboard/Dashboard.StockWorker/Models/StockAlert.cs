namespace Dashboard.StockWorker.Models
{
    public enum StockAlertLevel
    {
        Low,
        Critical,
        OutOfStock
    }

    public class StockAlert
    {
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal MaximumStock { get; set; }
        public StockAlertLevel AlertLevel { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public DateTime AlertTime { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public decimal AverageDailyConsumption { get; set; }
        public int DaysRemaining { get; set; }
    }
}
