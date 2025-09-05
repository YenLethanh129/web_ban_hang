namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class SupplierIngredientPriceDto
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public long IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public bool IsActive { get; set; }
}
