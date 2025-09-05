using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Services;

public interface IWarehouseInventoryService
{
    Task<PagedList<WarehouseIngredientInventoryDto>> GetWarehouseInventoryAsync(GetWarehouseInventoryInput input);
    Task<IEnumerable<WarehouseIngredientInventoryDto>> GetAllWarehouseInventoriesAsync();
    Task<WarehouseIngredientInventoryDto?> GetWarehouseInventoryByIngredientAsync(long ingredientId);
    Task<WarehouseIngredientInventoryDto> CreateWarehouseInventoryAsync(CreateWarehouseInventoryInput input);
    Task<WarehouseIngredientInventoryDto> UpdateWarehouseInventoryAsync(UpdateWarehouseInventoryInput input);
    Task<bool> DeleteWarehouseInventoryAsync(long ingredientId);
    Task<IEnumerable<LowStockIngredientDto>> GetLowStockIngredientsAsync();
}

public class WarehouseInventoryService : BaseTransactionalService, IWarehouseInventoryService
{
    private readonly IMapper _mapper;

    public WarehouseInventoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<WarehouseIngredientInventoryDto>> GetWarehouseInventoryAsync(GetWarehouseInventoryInput input)
    {
        // Get all warehouse inventories with includes
        var specification = WarehouseInventorySpecifications.BySearchCriteria(
            input.SearchTerm, input.CategoryId, input.IsLowStock);
        
        // Get total count for pagination
        var allInventories = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetAllWithSpecAsync(specification, true);
        var totalCount = allInventories.Count();

        // Get paged data using skip and take parameters
        var pagedInventories = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetAllWithSpecAsync(specification, true,
                skip: (input.PageNumber - 1) * input.PageSize,
                take: input.PageSize);

        var inventoryDtos = _mapper.Map<List<WarehouseIngredientInventoryDto>>(pagedInventories);

        return new PagedList<WarehouseIngredientInventoryDto>
        {
            Items = inventoryDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<IEnumerable<WarehouseIngredientInventoryDto>> GetAllWarehouseInventoriesAsync()
    {
        var specification = WarehouseInventorySpecifications.WithIncludes();
        var warehouseInventories = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<WarehouseIngredientInventoryDto>>(warehouseInventories);
    }

    public async Task<WarehouseIngredientInventoryDto?> GetWarehouseInventoryByIngredientAsync(long ingredientId)
    {
        var specification = WarehouseInventorySpecifications.ByIngredient(ingredientId);
        var warehouseInventory = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetWithSpecAsync(specification);

        return warehouseInventory != null ? _mapper.Map<WarehouseIngredientInventoryDto>(warehouseInventory) : null;
    }

    public async Task<WarehouseIngredientInventoryDto> CreateWarehouseInventoryAsync(CreateWarehouseInventoryInput input)
    {
        if (input.CurrentStock < 0)
            throw new ArgumentException("Current stock cannot be negative");

        if (input.SafetyStock < 0)
            throw new ArgumentException("Safety stock cannot be negative");

        if (input.MaximumThreshold < input.SafetyStock)
            throw new ArgumentException("Maximum threshold must be greater than or equal to safety stock");

        // Check if inventory already exists
        var existingSpec = WarehouseInventorySpecifications.ByIngredient(input.IngredientId);
        var existingInventory = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory != null)
            throw new InvalidOperationException($"Warehouse inventory for ingredient ID {input.IngredientId} already exists");

        // Validate ingredient exists
        await ValidateIngredientAsync(input.IngredientId);

        var inventory = _mapper.Map<IngredientWarehouse>(input);
        await _unitOfWork.Repository<IngredientWarehouse>().AddAsync(inventory);
        await _unitOfWork.SaveChangesAsync();

        return await GetWarehouseInventoryByIngredientAsync(input.IngredientId)
            ?? throw new InvalidOperationException("Failed to retrieve created inventory");
    }

    public async Task<WarehouseIngredientInventoryDto> UpdateWarehouseInventoryAsync(UpdateWarehouseInventoryInput input)
    {
        if (input.CurrentStock < 0)
            throw new ArgumentException("Current stock cannot be negative");

        if (input.SafetyStock < 0)
            throw new ArgumentException("Safety stock cannot be negative");

        if (input.MaximumThreshold < input.SafetyStock)
            throw new ArgumentException("Maximum threshold must be greater than or equal to safety stock");

        var existingSpec = WarehouseInventorySpecifications.ByIngredient(input.IngredientId);
        var existingInventory = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory == null)
            throw new InvalidOperationException($"Warehouse inventory for ingredient ID {input.IngredientId} not found");

        _mapper.Map(input, existingInventory);
        _unitOfWork.Repository<IngredientWarehouse>().Remove(existingInventory);
        _unitOfWork.Repository<IngredientWarehouse>().Add(existingInventory);
        await _unitOfWork.SaveChangesAsync();

        return await GetWarehouseInventoryByIngredientAsync(input.IngredientId)
            ?? throw new InvalidOperationException("Failed to retrieve updated inventory");
    }

    public async Task<bool> DeleteWarehouseInventoryAsync(long ingredientId)
    {
        var existingSpec = WarehouseInventorySpecifications.ByIngredient(ingredientId);
        var existingInventory = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory == null)
            throw new InvalidOperationException($"Warehouse inventory for ingredient ID {ingredientId} not found");

        _unitOfWork.Repository<IngredientWarehouse>().Remove(existingInventory);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LowStockIngredientDto>> GetLowStockIngredientsAsync()
    {
        var specification = WarehouseInventorySpecifications.LowStock();
        var lowStockIngredients = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<LowStockIngredientDto>>(lowStockIngredients);
    }

    private async Task<bool> ValidateIngredientAsync(long ingredientId)
    {
        var ingredient = await _unitOfWork.Repository<Ingredient>().GetAsync(ingredientId);
        if (ingredient == null)
            throw new InvalidOperationException($"Ingredient with ID {ingredientId} not found");

        return true;
    }
}
