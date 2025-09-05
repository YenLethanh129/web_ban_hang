using Dashboard.BussinessLogic.Dtos;

namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class GetSupplierPricesInput : DefaultInput
{
    public long? SupplierId { get; set; }
    public long? IngredientId { get; set; }
    public string? SearchTerm { get; set; }
    public bool? OnlyActive { get; set; }
    public DateTime? EffectiveAfter { get; set; }
    public DateTime? EffectiveBefore { get; set; }
}
