namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class CreateIngredientInput
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal CostPerUnit { get; set; }
    public int IngredientCategoryId { get; set; }
}
