namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class SupplierSummaryDto
{
    public long SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int TotalIngredients { get; set; }
    public int ActivePrices { get; set; }
    public int TotalPurchaseOrders { get; set; }
    public decimal TotalPurchaseAmount { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public decimal? AverageDeliveryTime { get; set; }
    public decimal? OverallRating { get; set; }
    public bool IsActive { get; set; }
}
