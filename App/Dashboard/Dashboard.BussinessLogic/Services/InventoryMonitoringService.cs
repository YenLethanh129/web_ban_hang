using AutoMapper;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Services;

public interface IInventoryMonitoringService
{
    Task<IEnumerable<LowStockIngredientDto>> GetAllLowStockBranchIngredientsAsync();
    Task<IEnumerable<LowStockIngredientDto>> GetAllLowStockWarehouseIngredientsAsync();
    Task<IEnumerable<LowStockIngredientDto>> GetLowStockBranchIngredientsByBranchAsync(long branchId);
    Task<IEnumerable<StockSummaryDto>> GetStockSummaryAsync();
    Task<bool> CheckAndUpdateStockThresholdsAsync();
}

public class InventoryMonitoringService : BaseTransactionalService, IInventoryMonitoringService
{
    private readonly IMapper _mapper;

    public InventoryMonitoringService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<LowStockIngredientDto>> GetAllLowStockBranchIngredientsAsync()
    {
        var specification = BranchInventorySpecifications.LowStock();
        var lowStockIngredients = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<LowStockIngredientDto>>(lowStockIngredients);
    }

    public async Task<IEnumerable<LowStockIngredientDto>> GetAllLowStockWarehouseIngredientsAsync()
    {
        var specification = WarehouseInventorySpecifications.LowStock();
        var lowStockIngredients = await _unitOfWork.Repository<IngredientWarehouse>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<LowStockIngredientDto>>(lowStockIngredients);
    }

    public async Task<IEnumerable<LowStockIngredientDto>> GetLowStockBranchIngredientsByBranchAsync(long branchId)
    {
        var specification = BranchInventorySpecifications.LowStockByBranch(branchId);
        var lowStockIngredients = await _unitOfWork.Repository<BranchIngredientInventory>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<LowStockIngredientDto>>(lowStockIngredients);
    }

    public async Task<IEnumerable<StockSummaryDto>> GetStockSummaryAsync()
    {
        var stockSummaries = new List<StockSummaryDto>();

        // Get all ingredients with their branch and warehouse inventories
        var ingredientSpec = IngredientSpecifications.WithIncludes();
        var ingredients = await _unitOfWork.Repository<Ingredient>()
            .GetAllWithSpecAsync(ingredientSpec, true);

        foreach (var ingredient in ingredients)
        {
            var summary = new StockSummaryDto
            {
                IngredientId = ingredient.Id,
                IngredientName = ingredient.Name,
                Unit = ingredient.Unit,
                CategoryName = ingredient.Category?.Name ?? "Unknown",
                TotalBranchStock = ingredient.BranchIngredientInventories?.Sum(bi => bi.Quantity) ?? 0,
                TotalWarehouseStock = ingredient.IngredientWarehouse?.Quantity ?? 0,
                BranchesWithLowStock = ingredient.BranchIngredientInventories
                    ?.Where(bi => bi.Quantity <= bi.SafetyStock)
                    .Select(bi => bi.Branch?.Name ?? "Unknown")
                    .ToList() ?? new List<string>(),
                WarehousesWithLowStock = ingredient.IngredientWarehouse != null && 
                    ingredient.IngredientWarehouse.Quantity <= ingredient.IngredientWarehouse.SafetyStock
                    ? new List<string> { "Main Warehouse" }
                    : new List<string>()
            };

            summary.TotalStock = summary.TotalBranchStock + summary.TotalWarehouseStock;
            summary.IsLowStockOverall = summary.BranchesWithLowStock.Any() || summary.WarehousesWithLowStock.Any();

            stockSummaries.Add(summary);
        }

        return stockSummaries.OrderBy(s => s.IngredientName);
    }

    public async Task<bool> CheckAndUpdateStockThresholdsAsync()
    {
        try
        {
            // This method could implement automatic threshold adjustments based on usage patterns
            // For now, it performs a health check on all inventory records

            var branchInventories = await _unitOfWork.Repository<BranchIngredientInventory>()
                .GetAllAsync();

            var warehouseInventories = await _unitOfWork.Repository<IngredientWarehouse>()
                .GetAllAsync();

            var hasUpdates = false;

            // Check branch inventories for negative stocks or invalid thresholds
            foreach (var branchInventory in branchInventories)
            {
                if (branchInventory.Quantity < 0)
                {
                    branchInventory.Quantity = 0;
                    hasUpdates = true;
                }

                if (branchInventory.SafetyStock < 0)
                {
                    branchInventory.SafetyStock = 0;
                    hasUpdates = true;
                }
            }

            // Check warehouse inventories for negative stocks or invalid thresholds
            foreach (var warehouseInventory in warehouseInventories)
            {
                if (warehouseInventory.Quantity < 0)
                {
                    warehouseInventory.Quantity = 0;
                    hasUpdates = true;
                }

                if (warehouseInventory.SafetyStock < 0)
                {
                    warehouseInventory.SafetyStock = 0;
                    hasUpdates = true;
                }

                if (warehouseInventory.MaximumStock.HasValue && 
                    warehouseInventory.MaximumStock < warehouseInventory.SafetyStock)
                {
                    warehouseInventory.MaximumStock = warehouseInventory.SafetyStock * 3;
                    hasUpdates = true;
                }
            }

            if (hasUpdates)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return hasUpdates;
        }
        catch
        {
            return false;
        }
    }
}
