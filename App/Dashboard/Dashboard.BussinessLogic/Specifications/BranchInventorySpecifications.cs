using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Specifications;

public static class BranchInventorySpecifications
{
    public static Specification<BranchIngredientInventory> WithIncludes()
    {
        var spec = new Specification<BranchIngredientInventory>(bi => true);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchIngredientInventory> ByBranch(long branchId)
    {
        var spec = new Specification<BranchIngredientInventory>(bi => bi.BranchId == branchId);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchIngredientInventory> ByBranchAndIngredient(long branchId, long ingredientId)
    {
        var spec = new Specification<BranchIngredientInventory>(bi => 
            bi.BranchId == branchId && bi.IngredientId == ingredientId);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchIngredientInventory> LowStockByBranch(long branchId)
    {
        var spec = new Specification<BranchIngredientInventory>(bi => 
            bi.BranchId == branchId && bi.Quantity <= bi.SafetyStock);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Branch");
        return spec;
    }

    public static Specification<BranchIngredientInventory> LowStock()
    {
        var spec = new Specification<BranchIngredientInventory>(bi => 
            bi.Quantity <= bi.SafetyStock);
        spec.IncludeStrings.Add("Ingredient");
        spec.IncludeStrings.Add("Ingredient.Category");
        spec.IncludeStrings.Add("Branch");
        return spec;
    }
}
