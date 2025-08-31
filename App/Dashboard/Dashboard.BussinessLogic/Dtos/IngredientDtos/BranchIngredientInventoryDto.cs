namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class BranchIngredientInventoryDto
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public long IngredientId { get; set; }
    public LowStockIngredientDto Ingredient { get; set; } = new();
    public decimal CurrentStock { get; set; }
    public decimal MinimumThreshold { get; set; }
    public decimal MaximumThreshold { get; set; }
    public DateTime LastUpdated { get; set; }
}
