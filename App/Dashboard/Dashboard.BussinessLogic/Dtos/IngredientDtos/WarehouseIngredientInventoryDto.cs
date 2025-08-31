namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class WarehouseIngredientInventoryDto
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public IngredientDto Ingredient { get; set; } = new();
    public decimal CurrentStock { get; set; }
    public decimal MinimumThreshold { get; set; }
    public decimal MaximumThreshold { get; set; }
    public DateTime LastUpdated { get; set; }
}
