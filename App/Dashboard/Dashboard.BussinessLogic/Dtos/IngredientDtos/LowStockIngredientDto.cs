namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class LowStockIngredientDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int InStockQuantity { get; set; }
    public decimal CostPerUnit { get; set; }
    public long IngredientCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
