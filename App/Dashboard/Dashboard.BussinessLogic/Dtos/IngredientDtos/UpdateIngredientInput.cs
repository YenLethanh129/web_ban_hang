namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class UpdateIngredientInput
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public bool IsActive { get; set; } 
}
