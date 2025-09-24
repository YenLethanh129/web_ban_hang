namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class GetIngredientsInput : DefaultInput
{
    public int? CategoryId { get; set; }
    public bool? IsLowStock { get; set; }
    public string? SearchTerm { get; set; }
}
