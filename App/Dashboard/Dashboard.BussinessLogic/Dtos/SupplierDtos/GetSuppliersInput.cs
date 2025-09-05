using Dashboard.BussinessLogic.Dtos;

namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class GetSuppliersInput : DefaultInput
{
    public string? SearchTerm { get; set; }
    public bool? HasActiveOrders { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}
