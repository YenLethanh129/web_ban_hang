using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BussinessLogic.Services;

public interface IIngredientService
{
    Task<IngredientDto> CreateIngredientAsync(CreateIngredientInput input);
    Task<IngredientDto> UpdateIngredientAsync(UpdateIngredientInput input);
    Task<bool> DeleteIngredientAsync(long id);

    // CRUD operations for Branch Inventory
    Task<BranchIngredientInventoryDto> CreateBranchInventoryAsync(CreateBranchInventoryInput input);
    Task<BranchIngredientInventoryDto> UpdateBranchInventoryAsync(UpdateBranchInventoryInput input);
    Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId);

    // CRUD operations for Warehouse Inventory
    Task<WarehouseIngredientInventoryDto> CreateWarehouseInventoryAsync(CreateWarehouseInventoryInput input);
    Task<WarehouseIngredientInventoryDto> UpdateWarehouseInventoryAsync(UpdateWarehouseInventoryInput input);
    Task<bool> DeleteWarehouseInventoryAsync(long ingredientId);

    // Validation
    Task<bool> ValidateIngredientAsync(long ingredientId);
    Task<bool> ValidateBranchAsync(long branchId);

    Task<PagedList<IngredientDto>> GetIngredientsAsync(GetIngredientsInput input);
    Task<PagedList<BranchIngredientInventoryDto>> GetBranchInventoryAsync(GetBranchInventoryInput input);
    Task<PagedList<WarehouseIngredientInventoryDto>> GetWarehouseInventoryAsync(GetWarehouseInventoryInput input);
    Task<IEnumerable<BranchIngredientInventoryDto>> GetAllBranchInventoriesAsync();
    Task<BranchIngredientInventoryDto?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId);
    Task<WarehouseIngredientInventoryDto?> GetWarehouseInventoryByIngredientAsync(long ingredientId);
    Task<IEnumerable<IngredientDto>> GetLowStockIngredientsAsync(long branchId);
    Task<IEnumerable<IngredientDto>> GetLowStockWarehouseIngredientsAsync();
    Task<IngredientDto?> GetIngredientByIdAsync(long id);
}

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public IngredientService(IUnitOfWork unitOfWork, IIngredientRepository ingredientRepository, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<IngredientDto>> GetIngredientsAsync(GetIngredientsInput input)
    {
        var ingredients = await _ingredientRepository.GetIngredientsWithCategoryAsync();
        if (input.CategoryId.HasValue)
        {
            ingredients = ingredients.Where(i => i.CategoryId == input.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            ingredients = ingredients.Where(i =>
                i.Name.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (i.Description != null && i.Description.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        var totalCount = ingredients.Count();
        var pagedIngredients = ingredients
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var ingredientDtos = _mapper.Map<List<IngredientDto>>(pagedIngredients);

        return new PagedList<IngredientDto> { 
            Items = ingredientDtos,
            TotalRecords = totalCount, 
            PageNumber = input.PageNumber, 
            PageSize = input.PageSize };
    }

    public async Task<PagedList<BranchIngredientInventoryDto>> GetBranchInventoryAsync(GetBranchInventoryInput input)
    {
        var branchInventories = await _ingredientRepository.GetBranchInventoryAsync(input.BranchId);

        if (input.CategoryId.HasValue)
        {
            branchInventories = branchInventories.Where(bi =>
                bi.Ingredient.CategoryId == input.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            branchInventories = branchInventories.Where(bi =>
                bi.Ingredient.Name.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (input.IsLowStock == true)
        {
            var lowStock = await _ingredientRepository.GetLowStockIngredientsAsync(input.BranchId);
            var lowStockIds = lowStock.Select(i => i.Id).ToHashSet();
            branchInventories = branchInventories.Where(bi => lowStockIds.Contains(bi.IngredientId));
        }

        var totalCount = branchInventories.Count();
        var pagedInventories = branchInventories
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var inventoryDtos = _mapper.Map<List<BranchIngredientInventoryDto>>(pagedInventories);

        return new PagedList<BranchIngredientInventoryDto>
        {
            Items = inventoryDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<PagedList<WarehouseIngredientInventoryDto>> GetWarehouseInventoryAsync(GetWarehouseInventoryInput input)
    {
        var warehouseInventories = await _ingredientRepository.GetWarehouseInventoryAsync();

        if (input.CategoryId.HasValue)
        {
            warehouseInventories = warehouseInventories.Where(wi =>
                wi.Ingredient.CategoryId == input.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            warehouseInventories = warehouseInventories.Where(wi =>
                wi.Ingredient.Name.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (input.IsLowStock == true)
        {
            var lowStock = await _ingredientRepository.GetLowStockWarehouseIngredientsAsync();
            var lowStockIds = lowStock.Select(i => i.Id).ToHashSet();
            warehouseInventories = warehouseInventories.Where(bi => lowStockIds.Contains(bi.IngredientId));
        }

        var totalCount = warehouseInventories.Count();
        var pagedInventories = warehouseInventories
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var inventoryDtos = _mapper.Map<List<WarehouseIngredientInventoryDto>>(pagedInventories);

        return new PagedList<WarehouseIngredientInventoryDto>
        {
            Items = inventoryDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<IEnumerable<BranchIngredientInventoryDto>> GetAllBranchInventoriesAsync()
    {
        var branchInventories = await _ingredientRepository.GetAllBranchInventoriesAsync();

        return _mapper.Map<IEnumerable<BranchIngredientInventoryDto>>(branchInventories);
    }

    public async Task<BranchIngredientInventoryDto?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId)
    {
        var branchInventory = await _ingredientRepository.GetBranchInventoryByIngredientAsync(branchId, ingredientId);

        return branchInventory != null ? _mapper.Map<BranchIngredientInventoryDto>(branchInventory) : null;
    }

    public async Task<WarehouseIngredientInventoryDto?> GetWarehouseInventoryByIngredientAsync(long ingredientId)
    {
        var warehouseInventory = await _ingredientRepository.GetWarehouseInventoryByIngredientAsync(ingredientId);

        return warehouseInventory != null ? _mapper.Map<WarehouseIngredientInventoryDto>(warehouseInventory) : null;
    }

    public async Task<IEnumerable<IngredientDto>> GetLowStockIngredientsAsync(long branchId)
    {
        var lowStockIngredients = await _ingredientRepository.GetLowStockIngredientsAsync(branchId);

        return _mapper.Map<IEnumerable<IngredientDto>>(lowStockIngredients);
    }

    public async Task<IEnumerable<IngredientDto>> GetLowStockWarehouseIngredientsAsync()
    {
        var lowStockIngredients = await _ingredientRepository.GetLowStockWarehouseIngredientsAsync();

        return _mapper.Map<IEnumerable<IngredientDto>>(lowStockIngredients);
    }

    public async Task<IngredientDto?> GetIngredientByIdAsync(long id)
    {
        var ingredient = await _ingredientRepository.GetAsync(id);

        return ingredient != null ? _mapper.Map<IngredientDto>(ingredient) : null;
    }

    public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientInput input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new ArgumentException("Ingredient name is required");

            if (input.CostPerUnit < 0)
                throw new ArgumentException("Cost per unit must be greater than or equal to 0");

            var nameExists = await _ingredientRepository.IngredientNameExistsAsync(input.Name);

            if (nameExists)
                throw new InvalidOperationException($"Ingredient with name '{input.Name}' already exists");

            var ingredient = _mapper.Map<Ingredient>(input);
            var createdIngredient = await _ingredientRepository
                .CreateIngredientAsync(ingredient);

            return _mapper.Map<IngredientDto>(createdIngredient);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating ingredient: {ex.Message}", ex);
        }
    }

    public async Task<IngredientDto> UpdateIngredientAsync(UpdateIngredientInput input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new ArgumentException("Ingredient name is required");

            if (input.CostPerUnit < 0)
                throw new ArgumentException("Cost per unit must be greater than or equal to 0");

            var existingIngredient = await _ingredientRepository.GetAsync(input.Id);

            if (existingIngredient == null)
                throw new InvalidOperationException($"Ingredient with ID {input.Id} not found");

            var nameExists = await _ingredientRepository
                .IngredientNameExistsAsync(input.Name, input.Id);

            if (nameExists)
                throw new InvalidOperationException($"Another ingredient with name '{input.Name}' already exists");

            _mapper.Map(input, existingIngredient);
            var updatedIngredient = await _ingredientRepository
                .UpdateIngredientAsync(existingIngredient);

            return _mapper.Map<IngredientDto>(updatedIngredient);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating ingredient: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteIngredientAsync(long id)
    {
        try
        {
            var exists = await _ingredientRepository
                .IngredientExistsAsync(id);

            if (!exists)
                throw new InvalidOperationException($"Ingredient with ID {id} not found");

            // TODO: Add business logic to check if ingredient is being used in recipes, orders, etc.
            // For now, I'll just delete it

            return await _ingredientRepository
                .DeleteIngredientAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting ingredient: {ex.Message}", ex);
        }
    }

    public async Task<BranchIngredientInventoryDto> CreateBranchInventoryAsync(CreateBranchInventoryInput input)
    {
        try
        {
            if (input.CurrentStock < 0)
                throw new ArgumentException("Current stock cannot be negative");

            if (input.MinimumThreshold < 0)
                throw new ArgumentException("Minimum threshold cannot be negative");

            if (input.MaximumThreshold < input.MinimumThreshold)
                throw new ArgumentException("Maximum threshold must be greater than or equal to minimum threshold");

            var existingInventory = await _ingredientRepository
                .GetBranchInventoryByIngredientAsync(input.BranchId, input.IngredientId);

            if (existingInventory != null)
                throw new InvalidOperationException($"Branch inventory for ingredient ID {input.IngredientId} and branch ID {input.BranchId} already exists");

            await ValidateIngredientAsync(input.IngredientId);
            await ValidateBranchAsync(input.BranchId);

            var inventory = _mapper.Map<BranchIngredientInventory>(input);
            var createdInventory = await _ingredientRepository
                .CreateBranchInventoryAsync(inventory);

            return _mapper.Map<BranchIngredientInventoryDto>(createdInventory);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating branch inventory: {ex.Message}", ex);
        }
    }

    public async Task<BranchIngredientInventoryDto> UpdateBranchInventoryAsync(UpdateBranchInventoryInput input)
    {
        try
        {
            if (input.CurrentStock < 0)
                throw new ArgumentException("Current stock cannot be negative");

            if (input.MinimumThreshold < 0)
                throw new ArgumentException("Minimum threshold cannot be negative");

            if (input.MaximumThreshold < input.MinimumThreshold)
                throw new ArgumentException("Maximum threshold must be greater than or equal to minimum threshold");

            var existingInventory = await _ingredientRepository
                .GetBranchInventoryByIngredientAsync(input.BranchId, input.IngredientId);

            if (existingInventory == null)
                throw new InvalidOperationException($"Branch inventory for ingredient ID {input.IngredientId} and branch ID {input.BranchId} not found");

            _mapper.Map(input, existingInventory);
            var updatedInventory = await _ingredientRepository
                .UpdateBranchInventoryAsync(existingInventory);

            return _mapper.Map<BranchIngredientInventoryDto>(updatedInventory);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating branch inventory: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId)
    {
        try
        {
            var existingInventory = await _ingredientRepository
                .GetBranchInventoryByIngredientAsync(branchId, ingredientId);

            if (existingInventory == null)
                throw new InvalidOperationException($"Branch inventory for ingredient ID {ingredientId} and branch ID {branchId} not found");

            return await _ingredientRepository
                .DeleteBranchInventoryAsync(branchId, ingredientId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting branch inventory: {ex.Message}", ex);
        }
    }
    public async Task<WarehouseIngredientInventoryDto> CreateWarehouseInventoryAsync(CreateWarehouseInventoryInput input)
    {
        try
        {
            if (input.CurrentStock < 0)
                throw new ArgumentException("Current stock cannot be negative");

            if (input.MinimumThreshold < 0)
                throw new ArgumentException("Minimum threshold cannot be negative");

            if (input.MaximumThreshold < input.MinimumThreshold)
                throw new ArgumentException("Maximum threshold must be greater than or equal to minimum threshold");

            var existingInventory = await _ingredientRepository
                .GetWarehouseInventoryByIngredientAsync(input.IngredientId);

            if (existingInventory != null)
                throw new InvalidOperationException($"Warehouse inventory for ingredient ID {input.IngredientId} already exists");

            await ValidateIngredientAsync(input.IngredientId);

            var inventory = _mapper.Map<IngredientWarehouse>(input);
            var createdInventory = await _ingredientRepository
                .CreateWarehouseInventoryAsync(inventory);

            return _mapper.Map<WarehouseIngredientInventoryDto>(createdInventory);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating warehouse inventory: {ex.Message}", ex);
        }
    }

    public async Task<WarehouseIngredientInventoryDto> UpdateWarehouseInventoryAsync(UpdateWarehouseInventoryInput input)
    {
        try
        {
            // Validate input
            if (input.CurrentStock < 0)
                throw new ArgumentException("Current stock cannot be negative");

            if (input.MinimumThreshold < 0)
                throw new ArgumentException("Minimum threshold cannot be negative");

            if (input.MaximumThreshold < input.MinimumThreshold)
                throw new ArgumentException("Maximum threshold must be greater than or equal to minimum threshold");

            // Check if warehouse inventory exists
            var existingInventory = await _ingredientRepository
                .GetWarehouseInventoryByIngredientAsync(input.IngredientId);

            if (existingInventory == null)
                throw new InvalidOperationException($"Warehouse inventory for ingredient ID {input.IngredientId} not found");

            // Update inventory
            _mapper.Map(input, existingInventory);
            var updatedInventory = await _ingredientRepository
                .UpdateWarehouseInventoryAsync(existingInventory);

            return _mapper.Map<WarehouseIngredientInventoryDto>(updatedInventory);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating warehouse inventory: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteWarehouseInventoryAsync(long ingredientId)
    {
        try
        {
            // Check if warehouse inventory exists
            var existingInventory = await _ingredientRepository
                .GetWarehouseInventoryByIngredientAsync(ingredientId);

            if (existingInventory == null)
                throw new InvalidOperationException($"Warehouse inventory for ingredient ID {ingredientId} not found");

            return await _ingredientRepository
                .DeleteWarehouseInventoryAsync(ingredientId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting warehouse inventory: {ex.Message}", ex);
        }
    }

    public async Task<bool> ValidateIngredientAsync(long ingredientId)
    {
        var exists = await _ingredientRepository
            .IngredientExistsAsync(ingredientId);

        if (!exists)
            throw new InvalidOperationException($"Ingredient with ID {ingredientId} not found");

        return true;
    }

    public async Task<bool> ValidateBranchAsync(long branchId)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetAsync(branchId);

        if (branch == null)
            throw new InvalidOperationException($"Branch with ID {branchId} not found");

        return true;
    }

}