// using AutoMapper;
// using Dashboard.BussinessLogic.Dtos.IngredientDtos;
// using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
// using Dashboard.DataAccess.Data;
// using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
// using Dashboard.Winform.ViewModels;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace Dashboard.Winform.Presenters
// {
//     public interface IIngredientDetailPresenter
//     {
//         Task<IngredientDetailViewModel?> LoadIngredientAsync(long id);
//         Task<IngredientDetailViewModel?> CreateIngredientAsync(IngredientDetailViewModel model);
//         Task<IngredientDetailViewModel?> UpdateIngredientAsync(IngredientDetailViewModel model);

//         Task<bool> AddBranchInventoryAsync(long ingredientId, long branchId, decimal currentStock,
//             decimal safetyStock, decimal maximumThreshold);
//         Task<bool> UpdateBranchInventoryAsync(long ingredientId, long branchId, decimal currentStock,
//             decimal safetyStock, decimal maximumThreshold);
//         Task<bool> DeleteBranchInventoryAsync(long ingredientId, long branchId);

//         Task<bool> AddWarehouseInventoryAsync(long ingredientId, decimal currentStock,
//             decimal safetyStock, decimal maximumThreshold);
//         Task<bool> UpdateWarehouseInventoryAsync(long ingredientId, decimal currentStock,
//             decimal safetyStock, decimal maximumThreshold);
//         Task<bool> DeleteWarehouseInventoryAsync(long ingredientId);

//         // Load dropdown data
//         Task<List<IngredientCategoryViewModel>> LoadCategoriesAsync();
//         Task<List<BranchViewModel>> LoadBranchesAsync();

//         event EventHandler<IngredientDetailViewModel?>? OnIngredientSaved;
//     }

//     public class IngredientDetailPresenter : IIngredientDetailPresenter
//     {
//         private readonly ILogger<IngredientDetailPresenter> _logger;
//         private readonly IIngredientManagementService _ingredientService;
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IMapper _mapper;
//         private readonly IServiceProvider _serviceProvider;

//         public event EventHandler<IngredientDetailViewModel?>? OnIngredientSaved;

//         public IngredientDetailPresenter(
//             IServiceProvider serviceProvider,
//             ILogger<IngredientDetailPresenter> logger,
//             IIngredientManagementService ingredientService,
//             IUnitOfWork unitOfWork,
//             IMapper mapper)
//         {
//             _serviceProvider = serviceProvider;
//             _logger = logger;
//             _ingredientService = ingredientService;
//             _unitOfWork = unitOfWork;
//             _mapper = mapper;
//         }

//         public async Task<IngredientDetailViewModel?> LoadIngredientAsync(long id)
//         {
//             try
//             {
//                 var dto = await _ingredientService.GetIngredientByIdAsync(id);
//                 if (dto == null)
//                 {
//                     return null;
//                 }

//                 var vm = _mapper.Map<IngredientDetailViewModel>(dto);

//                 // Load additional inventory data if needed
//                 await LoadInventoryData(vm);

//                 return vm;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error loading ingredient {IngredientId}", id);
//                 return null;
//             }
//         }

//         public async Task<IngredientDetailViewModel?> CreateIngredientAsync(IngredientDetailViewModel model)
//         {
//             try
//             {
//                 var input = _mapper.Map<CreateIngredientInput>(model);

//                 var created = await _ingredientService.CreateIngredientAsync(input);
//                 var vm = _mapper.Map<IngredientDetailViewModel>(created);

//                 OnIngredientSaved?.Invoke(this, vm);
//                 return vm;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error creating ingredient {IngredientName}", model?.Name);
//                 throw;
//             }
//         }

//         public async Task<IngredientDetailViewModel?> UpdateIngredientAsync(IngredientDetailViewModel model)
//         {
//             try
//             {
//                 using var scope = _serviceProvider.CreateScope();
//                 var ingredientService = scope.ServiceProvider.GetRequiredService<IIngredientManagementService>();
//                 var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

//                 var input = mapper.Map<UpdateIngredientInput>(model);

//                 var updated = await ingredientService.UpdateIngredientAsync(input);
//                 var vm = mapper.Map<IngredientDetailViewModel>(updated);

//                 OnIngredientSaved?.BeginInvoke(this, vm, null, null);
//                 return vm;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error updating ingredient {IngredientId}", model?.Id);
//                 throw;
//             }
//         }

//         public async Task<bool> AddBranchInventoryAsync(long ingredientId, long branchId,
//             decimal currentStock, decimal safetyStock, decimal maximumThreshold)
//         {
//             try
//             {
//                 // Create input for branch inventory
//                 var input = new CreateBranchInventoryInput
//                 {
//                     IngredientId = ingredientId,
//                     BranchId = branchId,
//                     CurrentStock = currentStock,
//                     SafetyStock = safetyStock,
//                     MaximumThreshold = maximumThreshold
//                 };

//                 // Use ingredient service that has inventory management
//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 var result = await inventoryService.CreateBranchInventoryAsync(input);
//                 return result != null;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error adding branch inventory for ingredient {IngredientId}, branch {BranchId}",
//                     ingredientId, branchId);
//                 return false;
//             }
//         }

//         public async Task<bool> UpdateBranchInventoryAsync(long ingredientId, long branchId,
//             decimal currentStock, decimal safetyStock, decimal maximumThreshold)
//         {
//             try
//             {
//                 var input = new UpdateBranchInventoryInput
//                 {
//                     IngredientId = ingredientId,
//                     BranchId = branchId,
//                     CurrentStock = currentStock,
//                     SafetyStock = safetyStock,
//                     MaximumThreshold = maximumThreshold
//                 };

//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 var result = await inventoryService.UpdateBranchInventoryAsync(input);
//                 return result != null;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error updating branch inventory for ingredient {IngredientId}, branch {BranchId}",
//                     ingredientId, branchId);
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteBranchInventoryAsync(long ingredientId, long branchId)
//         {
//             try
//             {
//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 return await inventoryService.DeleteBranchInventoryAsync(branchId, ingredientId);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error deleting branch inventory for ingredient {IngredientId}, branch {BranchId}",
//                     ingredientId, branchId);
//                 return false;
//             }
//         }

//         public async Task<bool> AddWarehouseInventoryAsync(long ingredientId,
//             decimal currentStock, decimal safetyStock, decimal maximumThreshold)
//         {
//             try
//             {
//                 var input = new CreateWarehouseInventoryInput
//                 {
//                     IngredientId = ingredientId,
//                     CurrentStock = currentStock,
//                     SafetyStock = safetyStock,
//                     MaximumThreshold = maximumThreshold
//                 };

//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 var result = await inventoryService.CreateWarehouseInventoryAsync(input);
//                 return result != null;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error adding warehouse inventory for ingredient {IngredientId}", ingredientId);
//                 return false;
//             }
//         }

//         public async Task<bool> UpdateWarehouseInventoryAsync(long ingredientId,
//             decimal currentStock, decimal safetyStock, decimal maximumThreshold)
//         {
//             try
//             {
//                 var input = new UpdateWarehouseInventoryInput
//                 {
//                     IngredientId = ingredientId,
//                     CurrentStock = currentStock,
//                     SafetyStock = safetyStock,
//                     MaximumThreshold = maximumThreshold
//                 };

//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 var result = await inventoryService.UpdateWarehouseInventoryAsync(input);
//                 return result != null;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error updating warehouse inventory for ingredient {IngredientId}", ingredientId);
//                 return false;
//             }
//         }

//         public async Task<bool> DeleteWarehouseInventoryAsync(long ingredientId)
//         {
//             try
//             {
//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 return await inventoryService.DeleteWarehouseInventoryAsync(ingredientId);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error deleting warehouse inventory for ingredient {IngredientId}", ingredientId);
//                 return false;
//             }
//         }

//         public async Task<List<IngredientCategoryViewModel>> LoadCategoriesAsync()
//         {
//             try
//             {
//                 var repo = _unitOfWork.Repository<IngredientCategory>();
//                 var categories = await repo.GetAllAsync(true);
//                 return categories.Select(c => new IngredientCategoryViewModel
//                 {
//                     Id = c.Id,
//                     Name = c.Name ?? string.Empty,
//                     Description = c.Description
//                 }).ToList();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error loading categories");
//                 return GetFallbackCategories();
//             }
//         }

//         public async Task<List<BranchViewModel>> LoadBranchesAsync()
//         {
//             try
//             {
//                 var repo = _unitOfWork.Repository<Dashboard.DataAccess.Models.Entities.Branches.Branch>();
//                 var branches = await repo.GetAllAsync(true);
//                 return branches.Select(b => new BranchViewModel
//                 {
//                     Id = b.Id,
//                     Name = b.Name ?? string.Empty,
//                     Address = b.Address,
//                     IsActive = true // Assume branches are active
//                 }).ToList();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error loading branches");
//                 return GetFallbackBranches();
//             }
//         }

//         private async Task LoadInventoryData(IngredientDetailViewModel vm)
//         {
//             try
//             {
//                 using var scope = _serviceProvider.CreateScope();
//                 var inventoryService = scope.ServiceProvider.GetRequiredService<IIngredientService>();

//                 // Load branch inventories
//                 var branchInventories = await inventoryService.GetAllBranchInventoriesAsync();
//                 var ingredientBranchInventories = branchInventories
//                     .Where(bi => bi.IngredientId == vm.Id)
//                     .Select(bi => new BranchInventoryViewModel
//                     {
//                         Id = bi.Id,
//                         BranchId = bi.BranchId,
//                         BranchName = bi.BranchName ?? "Unknown",
//                         IngredientId = bi.IngredientId,
//                         CurrentStock = bi.CurrentStock,
//                         SafetyStock = bi.SafetyStock,
//                         MaximumThreshold = bi.MaximumThreshold,
//                         LastUpdated = bi.LastUpdated
//                     }).ToList();

//                 vm.BranchInventories.Clear();
//                 foreach (var inventory in ingredientBranchInventories)
//                 {
//                     vm.BranchInventories.Add(inventory);
//                 }

//                 // Load warehouse inventory
//                 var warehouseInventory = await inventoryService.GetWarehouseInventoryByIngredientAsync(vm.Id);
//                 if (warehouseInventory != null)
//                 {
//                     vm.WarehouseInventory = new WarehouseInventoryViewModel
//                     {
//                         Id = warehouseInventory.Id,
//                         IngredientId = warehouseInventory.IngredientId,
//                         CurrentStock = warehouseInventory.CurrentStock,
//                         SafetyStock = warehouseInventory.SafetyStock,
//                         MaximumThreshold = warehouseInventory.MaximumThreshold,
//                         LastUpdated = warehouseInventory.LastUpdated
//                     };
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogWarning(ex, "Error loading inventory data for ingredient {IngredientId}", vm.Id);
//             }
//         }

//         private List<IngredientCategoryViewModel> GetFallbackCategories()
//         {
//             return new List<IngredientCategoryViewModel>
//             {
//                 new() { Id = 1, Name = "Rau củ", Description = "Các loại rau củ quả" },
//                 new() { Id = 2, Name = "Thịt cá", Description = "Thịt và hải sản" },
//                 new() { Id = 3, Name = "Gia vị", Description = "Các loại gia vị" },
//                 new() { Id = 4, Name = "Ngũ cốc", Description = "Gạo, bột, ngũ cốc" }
//             };
//         }

//         private List<BranchViewModel> GetFallbackBranches()
//         {
//             return new List<BranchViewModel>
//             {
//                 new() { Id = 1, Name = "Chi nhánh trung tâm", Address = "123 Nguyễn Huệ, Q1, TP.HCM", IsActive = true },
//                 new() { Id = 2, Name = "Chi nhánh quận 7", Address = "456 Nguyễn Thị Thập, Q7, TP.HCM", IsActive = true },
//                 new() { Id = 3, Name = "Chi nhánh Thủ Đức", Address = "789 Võ Văn Ngân, TP. Thủ Đức, TP.HCM", IsActive = true }
//             };
//         }
//     }
// }