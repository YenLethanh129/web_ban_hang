namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class UpdateIngredientInput
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal CostPerUnit { get; set; }
    public int IngredientCategoryId { get; set; }
}
