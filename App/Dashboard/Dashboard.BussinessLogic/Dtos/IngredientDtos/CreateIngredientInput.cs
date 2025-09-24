namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class CreateIngredientInput
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public bool IsActive { get; set; } = true; 
}
