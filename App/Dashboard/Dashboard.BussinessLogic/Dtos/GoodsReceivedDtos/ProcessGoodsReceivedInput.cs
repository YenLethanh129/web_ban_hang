using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;

namespace Dashboard.BussinessLogic.Dtos.GoodsReceivedDtos;

public class ProcessGoodsReceivedInput
{
    public string GrnCode { get; set; } = null!;
    public long SupplierId { get; set; }
    public long BranchId { get; set; }
    public long WarehouseStaffId { get; set; }

    public List<GoodsReceivedDetail> GoodsReceivedDetails { get; set; } = new List<GoodsReceivedDetail>();
}
