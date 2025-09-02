namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class UpdateWarehouseInventoryInput
{
    public int IngredientId { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal SafetyStock { get; set; }
    public decimal MaximumThreshold { get; set; }
}