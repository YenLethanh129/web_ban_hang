using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class GoodsReceivedNoteSpecification
{
    public static Specification<GoodsReceivedNote> ByBranchAndDate(long branchId, DateTime startOfMonth, DateTime endOfMonth)
    {
        var spec = new Specification<GoodsReceivedNote>(x => x.BranchId == branchId && x.ReceivedDate >= startOfMonth && x.ReceivedDate < endOfMonth);
        spec.IncludeStrings.Add("GoodsReceivedDetails");
        return spec;
    }
}
