namespace Dashboard.Common.Options
{
    public class StockWorkerOptions
    {
        // Interval in minutes for low-stock check
        public int CheckIntervalMinutes { get; set; } = 60;

        public int LowStockThreshold { get; set; } = 10;
        public int CriticalStockThreshold { get; set; } = 5;

        // optional lead-time default if you later want to wire into StockCalculationService
        public int DefaultLeadTimeDays { get; set; } = 7;
    }
}