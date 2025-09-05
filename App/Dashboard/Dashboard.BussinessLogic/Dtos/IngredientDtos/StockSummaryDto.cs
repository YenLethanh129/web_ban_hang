namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class StockSummaryDto
{
    public long IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalBranchStock { get; set; }
    public decimal TotalWarehouseStock { get; set; }
    public decimal TotalStock { get; set; }
    public bool IsLowStockOverall { get; set; }
    public List<string> BranchesWithLowStock { get; set; } = new();
    public List<string> WarehousesWithLowStock { get; set; } = new();
}
