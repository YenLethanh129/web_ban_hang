namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class GetBranchInventoryInput : DefaultInput
{
    public int BranchId { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsLowStock { get; set; }
    public string? SearchTerm { get; set; }
}
