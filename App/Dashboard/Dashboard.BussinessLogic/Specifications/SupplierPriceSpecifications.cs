using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class SupplierPriceSpecifications
{
    public static Specification<SupplierIngredientPrice> ById(long id)
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => sp.Id == id);
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> WithIncludes()
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => true);
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> BySupplier(long supplierId)
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => sp.SupplierId == supplierId);
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> ByIngredient(long ingredientId)
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => sp.IngredientId == ingredientId);
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> BySupplierAndIngredient(long supplierId, long ingredientId)
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => 
            sp.SupplierId == supplierId && sp.IngredientId == ingredientId);
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> ActivePrices()
    {
        var now = DateTime.Now;
        var spec = new Specification<SupplierIngredientPrice>(sp => 
            (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
            (sp.ExpiredDate == null || sp.ExpiredDate > now));
        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<SupplierIngredientPrice> BySearchCriteria(GetSupplierPricesInput input)
    {
        var spec = new Specification<SupplierIngredientPrice>(sp => true);

        if (input.SupplierId.HasValue && input.IngredientId.HasValue)
        {
            spec = new Specification<SupplierIngredientPrice>(sp =>
                sp.SupplierId == input.SupplierId.Value &&
                sp.IngredientId == input.IngredientId.Value);
        }
        else if (input.SupplierId.HasValue)
        {
            spec = new Specification<SupplierIngredientPrice>(sp =>
                sp.SupplierId == input.SupplierId.Value);
        }
        else if (input.IngredientId.HasValue)
        {
            spec = new Specification<SupplierIngredientPrice>(sp =>
                sp.IngredientId == input.IngredientId.Value);
        }

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            if (input.SupplierId.HasValue || input.IngredientId.HasValue)
            {
                // Complex combination - need to rebuild spec
                if (input.SupplierId.HasValue && input.IngredientId.HasValue)
                {
                    spec = new Specification<SupplierIngredientPrice>(sp =>
                        sp.SupplierId == input.SupplierId.Value &&
                        sp.IngredientId == input.IngredientId.Value &&
                        (sp.Supplier.Name.Contains(input.SearchTerm) ||
                         sp.Ingredient.Name.Contains(input.SearchTerm)));
                }
                else if (input.SupplierId.HasValue)
                {
                    spec = new Specification<SupplierIngredientPrice>(sp =>
                        sp.SupplierId == input.SupplierId.Value &&
                        (sp.Supplier.Name.Contains(input.SearchTerm) ||
                         sp.Ingredient.Name.Contains(input.SearchTerm)));
                }
                else if (input.IngredientId.HasValue)
                {
                    spec = new Specification<SupplierIngredientPrice>(sp =>
                        sp.IngredientId == input.IngredientId.Value &&
                        (sp.Supplier.Name.Contains(input.SearchTerm) ||
                         sp.Ingredient.Name.Contains(input.SearchTerm)));
                }
            }
            else
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.Supplier.Name.Contains(input.SearchTerm) ||
                    sp.Ingredient.Name.Contains(input.SearchTerm));
            }
        }

        if (input.OnlyActive == true)
        {
            var now = DateTime.Now;
            // Rebuild spec to include active filter
            var currentSpec = spec;
            spec = new Specification<SupplierIngredientPrice>(sp => true);
            
            // Apply all previous filters plus active filter
            if (input.SupplierId.HasValue && input.IngredientId.HasValue && !string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.SupplierId == input.SupplierId.Value &&
                    sp.IngredientId == input.IngredientId.Value &&
                    (sp.Supplier.Name.Contains(input.SearchTerm) ||
                     sp.Ingredient.Name.Contains(input.SearchTerm)) &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else if (input.SupplierId.HasValue && !string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.SupplierId == input.SupplierId.Value &&
                    (sp.Supplier.Name.Contains(input.SearchTerm) ||
                     sp.Ingredient.Name.Contains(input.SearchTerm)) &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else if (input.IngredientId.HasValue && !string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.IngredientId == input.IngredientId.Value &&
                    (sp.Supplier.Name.Contains(input.SearchTerm) ||
                     sp.Ingredient.Name.Contains(input.SearchTerm)) &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else if (input.SupplierId.HasValue)
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.SupplierId == input.SupplierId.Value &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else if (input.IngredientId.HasValue)
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    sp.IngredientId == input.IngredientId.Value &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    (sp.Supplier.Name.Contains(input.SearchTerm) ||
                     sp.Ingredient.Name.Contains(input.SearchTerm)) &&
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
            else
            {
                spec = new Specification<SupplierIngredientPrice>(sp =>
                    (sp.EffectiveDate == null || sp.EffectiveDate <= now) &&
                    (sp.ExpiredDate == null || sp.ExpiredDate > now));
            }
        }

        spec.IncludeStrings.Add("Supplier");
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }
}
