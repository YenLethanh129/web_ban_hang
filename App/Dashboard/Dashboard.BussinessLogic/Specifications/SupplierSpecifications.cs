using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.DataAccess.Models.Entities.Suppliers;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class SupplierSpecifications
{
    public static Specification<Supplier> WithIncludes()
    {
        var spec = new Specification<Supplier>(s => true);
        spec.IncludeStrings.Add("IngredientPurchaseOrders");
        spec.IncludeStrings.Add("SupplierIngredientPrices");
        spec.IncludeStrings.Add("SupplierPerformances");
        return spec;
    }

    public static Specification<Supplier> BySearchCriteria(GetSuppliersInput input)
    {
        var spec = new Specification<Supplier>(s => true);

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            spec = new Specification<Supplier>(s =>
                s.Name.Contains(input.SearchTerm) ||
                (s.Phone != null && s.Phone.Contains(input.SearchTerm)) ||
                (s.Email != null && s.Email.Contains(input.SearchTerm)) ||
                (s.Address != null && s.Address.Contains(input.SearchTerm)));
        }

        if (input.CreatedAfter.HasValue && input.CreatedBefore.HasValue)
        {
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<Supplier>(s =>
                    (s.Name.Contains(input.SearchTerm) ||
                     (s.Phone != null && s.Phone.Contains(input.SearchTerm)) ||
                     (s.Email != null && s.Email.Contains(input.SearchTerm)) ||
                     (s.Address != null && s.Address.Contains(input.SearchTerm))) &&
                    s.CreatedAt >= input.CreatedAfter.Value &&
                    s.CreatedAt <= input.CreatedBefore.Value);
            }
            else
            {
                spec = new Specification<Supplier>(s =>
                    s.CreatedAt >= input.CreatedAfter.Value &&
                    s.CreatedAt <= input.CreatedBefore.Value);
            }
        }
        else if (input.CreatedAfter.HasValue)
        {
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<Supplier>(s =>
                    (s.Name.Contains(input.SearchTerm) ||
                     (s.Phone != null && s.Phone.Contains(input.SearchTerm)) ||
                     (s.Email != null && s.Email.Contains(input.SearchTerm)) ||
                     (s.Address != null && s.Address.Contains(input.SearchTerm))) &&
                    s.CreatedAt >= input.CreatedAfter.Value);
            }
            else
            {
                spec = new Specification<Supplier>(s => s.CreatedAt >= input.CreatedAfter.Value);
            }
        }
        else if (input.CreatedBefore.HasValue)
        {
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<Supplier>(s =>
                    (s.Name.Contains(input.SearchTerm) ||
                     (s.Phone != null && s.Phone.Contains(input.SearchTerm)) ||
                     (s.Email != null && s.Email.Contains(input.SearchTerm)) ||
                     (s.Address != null && s.Address.Contains(input.SearchTerm))) &&
                    s.CreatedAt <= input.CreatedBefore.Value);
            }
            else
            {
                spec = new Specification<Supplier>(s => s.CreatedAt <= input.CreatedBefore.Value);
            }
        }

        return spec;
    }

    public static Specification<Supplier> ByName(string name, long? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return new Specification<Supplier>(s =>
                s.Name.ToLower() == name.ToLower() && s.Id != excludeId.Value);
        }

        return new Specification<Supplier>(s => s.Name.ToLower() == name.ToLower());
    }

    public static Specification<Supplier> ById(long id)
    {
        var spec = new Specification<Supplier>(s => s.Id == id);
        spec.IncludeStrings.Add("IngredientPurchaseOrders");
        spec.IncludeStrings.Add("SupplierIngredientPrices");
        spec.IncludeStrings.Add("SupplierPerformances");
        return spec;
    }

    public static Specification<Supplier> WithActiveOrders()
    {
        var spec = new Specification<Supplier>(s => s.IngredientPurchaseOrders.Any());
        spec.IncludeStrings.Add("IngredientPurchaseOrders");
        return spec;
    }
}
