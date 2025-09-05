namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class CreateSupplierPriceInput
{
    public long SupplierId { get; set; }
    public long IngredientId { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
}
