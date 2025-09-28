namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class WarehouseIngredientInventoryDto
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public LowStockIngredientDto Ingredient { get; set; } = new();
    public decimal SafetyStock { get; set; }
    public decimal MaximumThreshold { get; set; }
    public DateTime LastUpdated { get; set; }
}
