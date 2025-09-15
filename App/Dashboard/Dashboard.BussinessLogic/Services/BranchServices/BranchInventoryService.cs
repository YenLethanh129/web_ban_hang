using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;

namespace Dashboard.BussinessLogic.Services.BranchServices;

public interface IBranchInventoryService
{
    Task<PagedList<BranchIngredientInventoryDto>> GetBranchInventoryAsync(GetBranchInventoryInput input);
    Task<IEnumerable<BranchIngredientInventoryDto>> GetAllBranchInventoriesAsync();
    Task<BranchIngredientInventoryDto?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId);
    Task<BranchIngredientInventoryDto> CreateBranchInventoryAsync(CreateBranchInventoryInput input);
    Task<BranchIngredientInventoryDto> UpdateBranchInventoryAsync(UpdateBranchInventoryInput input);
    Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId);
    Task<IEnumerable<LowStockIngredientDto>> GetLowStockIngredientsAsync(long branchId);
}

public class BranchInventoryService : BaseTransactionalService, IBranchInventoryService
{
    private readonly IMapper _mapper;

    public BranchInventoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<BranchIngredientInventoryDto>> GetBranchInventoryAsync(GetBranchInventoryInput input)
    {
        // Get all branch inventories for the branch with includes
        var specification = BranchInventorySpecifications.ByBranch(input.BranchId);
        var allInventories = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetAllWithSpecAsync(specification, true);

        // Apply additional filters in memory (since complex navigation property queries can be challenging in EF)
        var filteredInventories = allInventories.AsEnumerable();

        if (input.CategoryId.HasValue)
        {
            filteredInventories = filteredInventories.Where(bi =>
                bi.Ingredient.CategoryId == input.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(input.SearchTerm))
        {
            filteredInventories = filteredInventories.Where(bi =>
                bi.Ingredient.Name.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (input.IsLowStock == true)
        {
            filteredInventories = filteredInventories.Where(bi =>
                bi.Quantity <= bi.SafetyStock);
        }

        var totalCount = filteredInventories.Count();
        var pagedInventories = filteredInventories
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

    public async Task<IEnumerable<BranchIngredientInventoryDto>> GetAllBranchInventoriesAsync()
    {
        var specification = BranchInventorySpecifications.WithIncludes();
        var branchInventories = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<BranchIngredientInventoryDto>>(branchInventories);
    }

    public async Task<BranchIngredientInventoryDto?> GetBranchInventoryByIngredientAsync(long branchId, long ingredientId)
    {
        var specification = BranchInventorySpecifications.ByBranchAndIngredient(branchId, ingredientId);
        var branchInventory = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetWithSpecAsync(specification);

        return branchInventory != null ? _mapper.Map<BranchIngredientInventoryDto>(branchInventory) : null;
    }

    public async Task<BranchIngredientInventoryDto> CreateBranchInventoryAsync(CreateBranchInventoryInput input)
    {
        if (input.CurrentStock < 0)
            throw new ArgumentException("Current stock cannot be negative");

        if (input.SafetyStock < 0)
            throw new ArgumentException("Safety stock cannot be negative");

        if (input.MaximumThreshold < input.SafetyStock)
            throw new ArgumentException("Maximum threshold must be greater than or equal to safety stock");

        // Check if inventory already exists
        var existingSpec = BranchInventorySpecifications.ByBranchAndIngredient(input.BranchId, input.IngredientId);
        var existingInventory = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory != null)
            throw new InvalidOperationException($"Branch inventory for ingredient ID {input.IngredientId} and branch ID {input.BranchId} already exists");

        // Validate ingredient exists
        await ValidateIngredientAsync(input.IngredientId);
        await ValidateBranchAsync(input.BranchId);

        var inventory = _mapper.Map<BranchIngredientInventory>(input);
        await _unitOfWork.Repository<BranchIngredientInventory>().AddAsync(inventory);
        await _unitOfWork.SaveChangesAsync();

        return await GetBranchInventoryByIngredientAsync(input.BranchId, input.IngredientId) 
            ?? throw new InvalidOperationException("Failed to retrieve created inventory");
    }

    public async Task<BranchIngredientInventoryDto> UpdateBranchInventoryAsync(UpdateBranchInventoryInput input)
    {
        if (input.CurrentStock < 0)
            throw new ArgumentException("Current stock cannot be negative");

        if (input.SafetyStock < 0)
            throw new ArgumentException("Safety stock cannot be negative");

        if (input.MaximumThreshold < input.SafetyStock)
            throw new ArgumentException("Maximum threshold must be greater than or equal to safety stock");

        var existingSpec = BranchInventorySpecifications.ByBranchAndIngredient(input.BranchId, input.IngredientId);
        var existingInventory = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory == null)
            throw new InvalidOperationException($"Branch inventory for ingredient ID {input.IngredientId} and branch ID {input.BranchId} not found");

        _mapper.Map(input, existingInventory);
        _unitOfWork.Repository<BranchIngredientInventory>().Remove(existingInventory);
        _unitOfWork.Repository<BranchIngredientInventory>().Add(existingInventory);
        await _unitOfWork.SaveChangesAsync();

        return await GetBranchInventoryByIngredientAsync(input.BranchId, input.IngredientId)
            ?? throw new InvalidOperationException("Failed to retrieve updated inventory");
    }

    public async Task<bool> DeleteBranchInventoryAsync(long branchId, long ingredientId)
    {
        var existingSpec = BranchInventorySpecifications.ByBranchAndIngredient(branchId, ingredientId);
        var existingInventory = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetWithSpecAsync(existingSpec);

        if (existingInventory == null)
            throw new InvalidOperationException($"Branch inventory for ingredient ID {ingredientId} and branch ID {branchId} not found");

        _unitOfWork.Repository<BranchIngredientInventory>().Remove(existingInventory);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<LowStockIngredientDto>> GetLowStockIngredientsAsync(long branchId)
    {
        var specification = BranchInventorySpecifications.LowStockByBranch(branchId);
        var lowStockIngredients = await _unitOfWork.Repository<BranchIngredientInventory>()
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

    private async Task<bool> ValidateBranchAsync(long branchId)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetAsync(branchId);
        if (branch == null)
            throw new InvalidOperationException($"Branch with ID {branchId} not found");

        return true;
    }
}
