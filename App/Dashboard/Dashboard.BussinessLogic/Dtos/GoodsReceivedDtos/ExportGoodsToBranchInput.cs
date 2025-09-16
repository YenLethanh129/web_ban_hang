namespace Dashboard.BussinessLogic.Dtos.GoodsReceivedDtos;

public class ExportGoodsToBranchInput
{
    public int IngredientId { get; set; }
    public int BranchId { get; set; }
    public decimal QuantityToExport { get; set; }
    public decimal SafetyStock { get; set; }
    public decimal MaximumThreshold { get; set; }
}
