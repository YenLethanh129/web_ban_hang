using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class WarehouseInventorySpecifications
{
    public static Specification<IngredientWarehouse> WithIncludes()
    {
        var spec = new Specification<IngredientWarehouse>(iw => true);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<IngredientWarehouse> ByIngredient(long ingredientId)
    {
        var spec = new Specification<IngredientWarehouse>(iw => iw.IngredientId == ingredientId);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<IngredientWarehouse> LowStock()
    {
        var spec = new Specification<IngredientWarehouse>(iw => iw.Quantity <= iw.SafetyStock);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }

    public static Specification<IngredientWarehouse> BySearchCriteria(
        string? searchTerm = null,
        long? categoryId = null,
        bool? isLowStock = null)
    {
        var spec = new Specification<IngredientWarehouse>(iw => true);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            spec = new Specification<IngredientWarehouse>(iw =>
                iw.Ingredient.Name.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Ingredient.Name.Contains(searchTerm) && 
                    iw.Ingredient.CategoryId == categoryId.Value);
            }
            else
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Ingredient.CategoryId == categoryId.Value);
            }
        }

        if (isLowStock == true)
        {
            if (!string.IsNullOrEmpty(searchTerm) && categoryId.HasValue)
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Ingredient.Name.Contains(searchTerm) && 
                    iw.Ingredient.CategoryId == categoryId.Value &&
                    iw.Quantity <= iw.SafetyStock);
            }
            else if (!string.IsNullOrEmpty(searchTerm))
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Ingredient.Name.Contains(searchTerm) && 
                    iw.Quantity <= iw.SafetyStock);
            }
            else if (categoryId.HasValue)
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Ingredient.CategoryId == categoryId.Value &&
                    iw.Quantity <= iw.SafetyStock);
            }
            else
            {
                spec = new Specification<IngredientWarehouse>(iw =>
                    iw.Quantity <= iw.SafetyStock);
            }
        }

        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        return spec;
    }
}
