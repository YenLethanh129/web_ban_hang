namespace Dashboard.BussinessLogic.Dtos.IngredientDtos;

public class CreateBranchInventoryInput
{
    public int BranchId { get; set; }
    public int IngredientId { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal MinimumThreshold { get; set; }
    public decimal MaximumThreshold { get; set; }
}
