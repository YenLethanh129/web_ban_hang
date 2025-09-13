using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class IngredientSpecifications
{
    public static Specification<Ingredient> WithCategory()
    {
        var spec = new Specification<Ingredient>(i => true);
        spec.IncludeStrings.Add("Category");
        return spec;
    }

    public static Specification<Ingredient> BySearchCriteria(GetIngredientsInput input)
    {
        var spec = new Specification<Ingredient>(i => true);
        
        if (input.CategoryId.HasValue && !string.IsNullOrEmpty(input.SearchTerm))
        {
            spec = new Specification<Ingredient>(i => 
                i.CategoryId == input.CategoryId.Value &&
                (i.Name.Contains(input.SearchTerm) ||
                 (i.Description != null && i.Description.Contains(input.SearchTerm))));
        }
        else if (input.CategoryId.HasValue)
        {
            spec = new Specification<Ingredient>(i => i.CategoryId == input.CategoryId.Value);
        }
        else if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            spec = new Specification<Ingredient>(i => 
                i.Name.Contains(input.SearchTerm) ||
                (i.Description != null && i.Description.Contains(input.SearchTerm)));
        }

        spec.IncludeStrings.Add("Category");
        return spec;
    }

    public static Specification<Ingredient> ByName(string name, long? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return new Specification<Ingredient>(i => 
                i.Name.ToLower() == name.ToLower() && i.Id != excludeId.Value);
        }
        
        return new Specification<Ingredient>(i => i.Name.ToLower() == name.ToLower());
    }

    public static Specification<Ingredient> ById(long id)
    {
        var spec = new Specification<Ingredient>(i => i.Id == id);
        spec.IncludeStrings.Add("Category");
        return spec;
    }

    public static Specification<Ingredient> WithIncludes()
    {
        var spec = new Specification<Ingredient>(i => true);
        spec.IncludeStrings.Add("Category");
        spec.IncludeStrings.Add("BranchIngredientInventories");
        spec.IncludeStrings.Add("BranchIngredientInventories.Branch");
        spec.IncludeStrings.Add("IngredientWarehouse");
        return spec;
    }
}
