using Dashboard.DataAccess.Models.Entities.Suppliers;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class SupplierPerformanceSpecifications
{
    public static Specification<SupplierPerformance> WithIncludes()
    {
        var spec = new Specification<SupplierPerformance>(sp => true);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }

    public static Specification<SupplierPerformance> BySupplier(long supplierId)
    {
        var spec = new Specification<SupplierPerformance>(sp => sp.SupplierId == supplierId);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }

    public static Specification<SupplierPerformance> ByPeriod(string evaluationPeriod, string periodValue)
    {
        var spec = new Specification<SupplierPerformance>(sp => 
            sp.EvaluationPeriod == evaluationPeriod && sp.PeriodValue == periodValue);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }

    public static Specification<SupplierPerformance> BySupplierAndPeriod(long supplierId, string evaluationPeriod, string periodValue)
    {
        var spec = new Specification<SupplierPerformance>(sp => 
            sp.SupplierId == supplierId && 
            sp.EvaluationPeriod == evaluationPeriod && 
            sp.PeriodValue == periodValue);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }

    public static Specification<SupplierPerformance> TopPerformers(int topCount = 10)
    {
        var spec = new Specification<SupplierPerformance>(sp => sp.OverallRating != null);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }

    public static Specification<SupplierPerformance> RecentPerformance(DateTime fromDate)
    {
        var spec = new Specification<SupplierPerformance>(sp => sp.CreatedAt >= fromDate);
        spec.IncludeStrings.Add("Supplier");
        return spec;
    }
}
