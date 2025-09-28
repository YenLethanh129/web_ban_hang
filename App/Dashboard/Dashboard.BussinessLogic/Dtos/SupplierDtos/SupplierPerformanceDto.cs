namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class SupplierPerformanceDto
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string EvaluationPeriod { get; set; } = string.Empty;
    public string PeriodValue { get; set; } = string.Empty;
    public int? TotalOrders { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? AverageOrderValue { get; set; }
    public int? OnTimeDeliveries { get; set; }
    public decimal? OnTimeDeliveryRate { get; set; }
    public int? QualityScore { get; set; }
    public decimal? OverallRating { get; set; }
    public DateTime CreatedAt { get; set; }
}
