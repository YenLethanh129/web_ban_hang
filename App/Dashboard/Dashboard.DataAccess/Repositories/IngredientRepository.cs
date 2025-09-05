using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IIngredientRepository : IRepository<Ingredient>
{

    Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
    Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient);
    Task<bool> DeleteIngredientAsync(long id);
    Task<bool> IngredientExistsAsync(long id);
    Task<bool> IngredientNameExistsAsync(string name, long? excludeId = null);
    
    Task<BranchIngredientInventory> CreateBranchInventoryAsync(BranchIngredientInventory inventory);
    Task<BranchIngredientInventory> UpdateBranchInventoryAsync(BranchIngredientInventory inventory);
    Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId);
    
    Task<IngredientWarehouse> CreateWarehouseInventoryAsync(IngredientWarehouse inventory);
    Task<IngredientWarehouse> UpdateWarehouseInventoryAsync(IngredientWarehouse inventory);
    Task<bool> DeleteWarehouseInventoryAsync(long ingredientId);

    Task<IEnumerable<Ingredient>> GetIngredientsWithCategoryAsync();
    Task<IEnumerable<BranchIngredientInventory>> GetBranchInventoryAsync(long branchId);
    Task<IEnumerable<BranchIngredientInventory>> GetAllBranchInventoriesAsync();
    Task<IEnumerable<IngredientWarehouse>> GetWarehouseInventoryAsync();
    Task<BranchIngredientInventory?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId);
    Task<IngredientWarehouse?> GetWarehouseInventoryByIngredientAsync(long ingredientId);
    Task<IEnumerable<Ingredient>> GetLowStockIngredientsAsync(long branchId);
    Task<IEnumerable<Ingredient>> GetLowStockWarehouseIngredientsAsync();
}

public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ingredient>> GetIngredientsWithCategoryAsync()
    {
        return await _context.Ingredients
            .Include(i => i.Category)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<BranchIngredientInventory>> GetBranchInventoryAsync(long branchId)
    {
        return await _context.BranchIngredientInventories
            .Include(bi => bi.Ingredient)
                .ThenInclude(i => i.Category)
            .Include(bi => bi.Branch)
            .Where(bi => bi.BranchId == branchId)
            .OrderBy(bi => bi.Ingredient.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<BranchIngredientInventory>> GetAllBranchInventoriesAsync()
    {
        return await _context.BranchIngredientInventories
            .Include(bi => bi.Ingredient)
                .ThenInclude(i => i.Category)
            .Include(bi => bi.Branch)
            .OrderBy(bi => bi.Branch.Name)
                .ThenBy(bi => bi.Ingredient.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<IngredientWarehouse>> GetWarehouseInventoryAsync()
    {
        return await _context.IngredientWarehouses
            .Include(iw => iw.Ingredient)
                .ThenInclude(i => i.Category)
            .OrderBy(iw => iw.Ingredient.Name)
            .ToListAsync();
    }

    public async Task<BranchIngredientInventory?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId)
    {
        return await _context.BranchIngredientInventories
            .Include(bi => bi.Ingredient)
                .ThenInclude(i => i.Category)
            .Include(bi => bi.Branch)
            .FirstOrDefaultAsync(bi => bi.BranchId == branchId && bi.IngredientId == ingredientId);
    }

    public async Task<IngredientWarehouse?> GetWarehouseInventoryByIngredientAsync(long ingredientId)
    {
        return await _context.IngredientWarehouses
            .Include(iw => iw.Ingredient)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(iw => iw.IngredientId == ingredientId);
    }

    public async Task<IEnumerable<Ingredient>> GetLowStockIngredientsAsync(long branchId)
    {
        // Simplify the query to avoid complex joins
        var ingredients = await _context.Ingredients
            .Include(i => i.Category)
            .Include(i => i.BranchIngredientInventories.Where(bi => bi.BranchId == branchId))
            .Where(i => i.BranchIngredientInventories.Any(bi => bi.BranchId == branchId))
            .ToListAsync();
            
        // Filter in memory to avoid EF complex query generation
        var lowStockIngredients = new List<Ingredient>();
        foreach (var ingredient in ingredients)
        {
            var branchInventory = ingredient.BranchIngredientInventories.FirstOrDefault(bi => bi.BranchId == branchId);
            if (branchInventory != null)
            {
                // Simple quantity check - consider low stock if quantity < 10 for demo
                if (branchInventory.Quantity < 10)
                {
                    lowStockIngredients.Add(ingredient);
                }
            }
        }
        
        return lowStockIngredients;
    }

    public async Task<IEnumerable<Ingredient>> GetLowStockWarehouseIngredientsAsync()
    {
        var ingredients = await _context.Ingredients
            .Include(i => i.Category)
            .Include(i => i.IngredientWarehouse)
            .Where(i => i.IngredientWarehouse != null)
            .ToListAsync();
            
        var lowStockIngredients = new List<Ingredient>();
        foreach (var ingredient in ingredients)
        {
            if (ingredient.IngredientWarehouse != null)
            {
                if (ingredient.IngredientWarehouse.Quantity < 10)
                {
                    lowStockIngredients.Add(ingredient);
                }
            }
        }
        
        return lowStockIngredients;
    }
        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
    {
        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
        await _context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<bool> DeleteIngredientAsync(long id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return false;

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IngredientExistsAsync(long id)
    {
        return await _context.Ingredients.AnyAsync(i => i.Id == id);
    }

    public async Task<bool> IngredientNameExistsAsync(string name, long? excludeId = null)
    {
        return await _context.Ingredients
            .AnyAsync(i => i.Name.ToLower() == name.ToLower() && 
                          (!excludeId.HasValue || i.Id != excludeId.Value));
    }

    // Branch inventory CRUD
    public async Task<BranchIngredientInventory> CreateBranchInventoryAsync(BranchIngredientInventory inventory)
    {
        await _context.BranchIngredientInventories.AddAsync(inventory);
        await _context.SaveChangesAsync();
        
        return await _context.BranchIngredientInventories
            .Include(bi => bi.Ingredient)
                .ThenInclude(i => i.Category)
            .Include(bi => bi.Branch)
            .FirstAsync(bi => bi.Id == inventory.Id);
    }

    public async Task<BranchIngredientInventory> UpdateBranchInventoryAsync(BranchIngredientInventory inventory)
    {
        _context.BranchIngredientInventories.Update(inventory);
        await _context.SaveChangesAsync();
        
        return await _context.BranchIngredientInventories
            .Include(bi => bi.Ingredient)
                .ThenInclude(i => i.Category)
            .Include(bi => bi.Branch)
            .FirstAsync(bi => bi.Id == inventory.Id);
    }

    public async Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId)
    {
        var inventory = await _context.BranchIngredientInventories
            .FirstOrDefaultAsync(bi => bi.BranchId == branchId && bi.IngredientId == ingredientId);
        
        if (inventory == null) return false;

        _context.BranchIngredientInventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return true;
    }

    // Warehouse inventory CRUD
    public async Task<IngredientWarehouse> CreateWarehouseInventoryAsync(IngredientWarehouse inventory)
    {
        await _context.IngredientWarehouses.AddAsync(inventory);
        await _context.SaveChangesAsync();
        
        return await _context.IngredientWarehouses
            .Include(iw => iw.Ingredient)
                .ThenInclude(i => i.Category)
            .FirstAsync(iw => iw.Id == inventory.Id);
    }

    public async Task<IngredientWarehouse> UpdateWarehouseInventoryAsync(IngredientWarehouse inventory)
    {
        _context.IngredientWarehouses.Update(inventory);
        await _context.SaveChangesAsync();
        
        return await _context.IngredientWarehouses
            .Include(iw => iw.Ingredient)
                .ThenInclude(i => i.Category)
            .FirstAsync(iw => iw.Id == inventory.Id);
    }

    public async Task<bool> DeleteWarehouseInventoryAsync(long ingredientId)
    {
        var inventory = await _context.IngredientWarehouses
            .FirstOrDefaultAsync(iw => iw.IngredientId == ingredientId);
        
        if (inventory == null) return false;

        _context.IngredientWarehouses.Remove(inventory);
        await _context.SaveChangesAsync();
        return true;
    }

}