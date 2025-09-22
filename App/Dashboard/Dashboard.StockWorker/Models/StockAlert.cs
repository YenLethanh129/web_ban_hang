namespace Dashboard.StockWorker.Models
{
    public enum StockAlertLevel
    {
        Low = 1,
        Critical = 2,
        OutOfStock = 3
    }

    public class StockAlert
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal SafetyStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal AverageDailyConsumption { get; set; }
        public int DaysRemaining { get; set; }
        public StockAlertLevel AlertLevel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Additional properties for enhanced reporting
        public decimal MaximumStock { get; set; }
        public decimal ReservedQuantity { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierContact { get; set; }
        public decimal? LastPurchasePrice { get; set; }
        public DateTime? LastRestockDate { get; set; }
        public string? Note { get; set; }
    }
}