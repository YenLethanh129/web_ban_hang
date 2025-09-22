using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters;

public interface IIngredientManagementPresenter : IManagementPresenter<IngredientManagementModel>
{
    Task LoadDataAsync(long? categoryId = null, string? searchTerm = null,
        int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task AddIngredientAsync(string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId);
    Task UpdateIngredientAsync(long id, string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId);
    Task DeleteIngredientAsync(long id);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task FilterByCategoryAsync(long categoryId);
    Task SortBy(string? sortBy);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
    Task AddCategoryAsync(string name, string? description);
    Task UpdateCategoryAsync(long id, string name, string? description);
    Task DeleteCategoryAsync(long id);
}

public class IngredientManagementPresenter : IIngredientManagementPresenter
{
    private readonly IIngredientManagementService _ingredientManagementService;
    private readonly IUnitOfWork _unitOfWork;

    public IngredientManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<IngredientViewModel> _allIngredientsCache = new();
    private List<IngredientViewModel> _filteredIngredients = new();
    private string _currentSearchTerm = string.Empty;
    private string _currentStatusFilter = "All";
    private long? _currentCategoryFilter = null;
    private string _currentSortBy = string.Empty;
    private bool _sortDescending = false;

    IManagableModel IManagementPresenter<IngredientManagementModel>.Model
    {
        get => Model;
        set => throw new NotImplementedException();
    }

    public event EventHandler? OnDataLoaded;

    public IngredientManagementPresenter(
         IIngredientManagementService ingredientManagementService,
         IUnitOfWork unitOfWork
        )
    {
        _ingredientManagementService = ingredientManagementService ?? throw new ArgumentNullException(nameof(ingredientManagementService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        Model = new IngredientManagementModel();
    }

    public async Task LoadDataAsync(
        long? categoryId = null,
        string? searchTerm = null,
        int? pageSize = 10,
        int? page = 1,
        bool forceRefresh = false)
    {
        if (_isLoading && !forceRefresh) return;

        await _semaphore.WaitAsync();
        try
        {
            _isLoading = true;

            _currentCategoryFilter = categoryId ?? _currentCategoryFilter;
            _currentSearchTerm = searchTerm ?? _currentSearchTerm;

            if (!Model.Categories.Any())
            {
                await LoadCategoriesInternal();
            }

            if (!_allIngredientsCache.Any() || forceRefresh)
            {
                await LoadAllDataToCacheInternal();
            }

            if (pageSize.HasValue) Model.PageSize = pageSize.Value;
            if (page.HasValue) Model.CurrentPage = page.Value;

            await ApplyFiltersAndPaging();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            throw;
        }
        finally
        {
            _isLoading = false;
            _semaphore.Release();
        }
    }

    private async Task LoadAllDataToCacheInternal()
    {
        _allIngredientsCache.Clear();

        try
        {
            var input = new GetIngredientsInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };

            var paged = await _ingredientManagementService.GetIngredientsAsync(input);

            if (paged?.Items != null)
            {
                _allIngredientsCache = [.. paged.Items.Select(dto => new IngredientViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name ?? string.Empty,
                    Unit = dto.Unit ?? string.Empty,
                    CategoryId = dto.CategoryId,
                    CategoryName = dto.CategoryName ?? "Không rõ",
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    CreatedAt = dto.CreatedAt,
                    UpdatedAt = dto.UpdatedAt,
                })];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading from service: {ex.Message}");
            _allIngredientsCache = GenerateMockData();
        }

        if (Model.Categories != null && Model.Categories.Count > 0)
        {
            foreach (var item in _allIngredientsCache)
            {
                var cat = Model.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                item.CategoryName = cat?.Name ?? item.CategoryName ?? "Không rõ";
            }
        }
    }

    private async Task LoadCategoriesInternal()
    {
        Model.Categories.Clear();

        try
        {
            var repo = _unitOfWork.Repository<IngredientCategory>();
            var categories = await repo.GetAllAsync(true);

            foreach (var c in categories)
            {
                Model.Categories.Add(new IngredientCategoryViewModel
                {
                    Id = (long)c.Id,
                    Name = c.Name ?? string.Empty,
                    Description = c.Description
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");
            Model.Categories.Add(new IngredientCategoryViewModel { Id = 1, Name = "Rau củ", Description = "Các loại rau củ quả" });
            Model.Categories.Add(new IngredientCategoryViewModel { Id = 2, Name = "Thịt cá", Description = "Thịt và hải sản" });
            Model.Categories.Add(new IngredientCategoryViewModel { Id = 3, Name = "Gia vị", Description = "Các loại gia vị" });
        }
    }
    private async Task LoadCategories()
    {
        await _semaphore.WaitAsync();
        try
        {
            await LoadCategoriesInternal();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task LoadDataAsync()
    {
        await LoadDataAsync(null, null, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task SearchAsync(string searchTerm)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentSearchTerm = searchTerm ?? string.Empty;
            Model.CurrentPage = 1;

            if (!_allIngredientsCache.Any())
            {
                await LoadAllDataToCacheInternal();
            }

            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private Task ApplyFiltersAndPaging()
    {
        try
        {
            var query = _allIngredientsCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
            {
                query = query.Where(x =>
                    x.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    x.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (x.Description ?? string.Empty).Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_currentStatusFilter) && _currentStatusFilter != "All")
            {
                bool active = _currentStatusFilter.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                             _currentStatusFilter.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase);
                query = query.Where(x => x.IsActive == active);
            }

            if (_currentCategoryFilter.HasValue && _currentCategoryFilter > 0)
            {
                query = query.Where(x => x.CategoryId == _currentCategoryFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(_currentSortBy))
            {
                query = _currentSortBy.ToLower() switch
                {
                    "id" => _sortDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                    "name" => _sortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    "category" => _sortDescending ? query.OrderByDescending(x => x.CategoryName) : query.OrderBy(x => x.CategoryName),
                    "categoryname" => _sortDescending ? query.OrderByDescending(x => x.CategoryName) : query.OrderBy(x => x.CategoryName),
                    "unit" => _sortDescending ? query.OrderByDescending(x => x.Unit) : query.OrderBy(x => x.Unit),
                    "description" => _sortDescending ? query.OrderByDescending(x => x.Description ?? "") : query.OrderBy(x => x.Description ?? ""),
                    "isactive" => _sortDescending ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
                    "status" => _sortDescending ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
                    "createdat" => _sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    "created date" => _sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                    _ => query.OrderBy(x => x.Id)
                };
            }

            _filteredIngredients = query.ToList();

            Model.TotalItems = _filteredIngredients.Count;

            if (Model.CurrentPage < 1) Model.CurrentPage = 1;
            if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
                Model.CurrentPage = Model.TotalPages;

            var items = _filteredIngredients
                .Skip((Model.CurrentPage - 1) * Model.PageSize)
                .Take(Model.PageSize)
                .ToList();

            Model.Ingredients.Clear();
            foreach (var item in items)
            {
                Model.Ingredients.Add(item);
            }

            if (items.Any())
            {
                OnDataLoaded?.Invoke(this, new IngredientsLoadedEventArgs(items));
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in ApplyFiltersAndPaging: {ex.Message}");
            return Task.CompletedTask;
        }
    }

    private List<IngredientViewModel> GenerateMockData()
    {
        var mockData = new List<IngredientViewModel>();

        for (int i = 1; i <= 20; i++)
        {
            mockData.Add(new IngredientViewModel
            {
                Id = i,
                Name = $"Nguyên liệu {i}",
                Unit = i % 2 == 0 ? "kg" : "lít",
                CategoryId = (i % 3) + 1,
                CategoryName = (i % 3) switch
                {
                    0 => "Rau củ",
                    1 => "Thịt cá",
                    _ => "Gia vị"
                },
                Description = $"Mô tả cho nguyên liệu {i}",
                IsActive = i % 4 != 0,
                CostPerUnit = (10000 + (i * 500)),
                CreatedAt = DateTime.Now.AddDays(-i),
                UpdatedAt = i % 3 == 0 ? DateTime.Now.AddHours(-i) : null
            });
        }

        return mockData;
    }

    public async Task AddCategoryAsync(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name is required", nameof(name));

        await _semaphore.WaitAsync();
        try
        {
            var repo = _unitOfWork.Repository<IngredientCategory>();

            var exists = await repo.GetContainString("Name", name, true);
            if (exists != null)
            {
                throw new InvalidOperationException($"Category with name '{name}' already exists.");
            }

            var entity = new IngredientCategory
            {
                Name = name.Trim(),
                Description = description?.Trim(),
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            await LoadCategories();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task UpdateCategoryAsync(long id, string name, string? description)
    {
        if (id <= 0) throw new ArgumentException("Invalid category id", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name is required", nameof(name));

        await _semaphore.WaitAsync();
        try
        {
            var repo = _unitOfWork.Repository<IngredientCategory>();
            var existing = await repo.GetAsync(id) ?? throw new InvalidOperationException($"Category id {id} not found.");
            if (await repo.GetContainString("Name", name, true) is IngredientCategory dup && dup.Id != existing.Id)
                throw new InvalidOperationException($"Another category with name '{name}' exists.");

            existing.Name = name.Trim();
            existing.Description = description?.Trim();
            existing.LastModified = DateTime.UtcNow;

            repo.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            await LoadCategories();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DeleteCategoryAsync(long id)
    {
        if (id <= 0) throw new ArgumentException("Invalid category id", nameof(id));

        await _semaphore.WaitAsync();
        try
        {
            var repo = _unitOfWork.Repository<IngredientCategory>();
            IngredientCategory? existing = await repo.GetAsync(id) ?? throw new InvalidOperationException($"Category id {id} not found.");
            repo.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            await LoadCategories();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task AddIngredientAsync(string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        await Task.Delay(100);

        var newIngredient = new IngredientViewModel
        {
            Id = _allIngredientsCache.Any() ? _allIngredientsCache.Max(i => i.Id) + 1 : 1,
            Name = name,
            Unit = unit,
            CategoryId = categoryId,
            CategoryName = GetCategoryName(categoryId),
            Description = description,
            IsActive = isActive,
            TaxId = taxId,
            CreatedAt = DateTime.Now,
            CostPerUnit = 0 
        };

        _allIngredientsCache.Add(newIngredient);
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task UpdateIngredientAsync(long id, string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        await Task.Delay(100);

        var existingIngredient = _allIngredientsCache.FirstOrDefault(i => i.Id == id);
        if (existingIngredient != null)
        {
            existingIngredient.Name = name;
            existingIngredient.Unit = unit;
            existingIngredient.CategoryId = categoryId;
            existingIngredient.CategoryName = GetCategoryName(categoryId);
            existingIngredient.Description = description;
            existingIngredient.IsActive = isActive;
            existingIngredient.TaxId = taxId;
            existingIngredient.UpdatedAt = DateTime.Now;
        }

        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task DeleteIngredientAsync(long id)
    {
        await Task.Delay(100);

        var ingredientToRemove = _allIngredientsCache.FirstOrDefault(i => i.Id == id);
        if (ingredientToRemove != null)
        {
            _allIngredientsCache.Remove(ingredientToRemove);
        }

        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task FilterByStatusAsync(string status)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentStatusFilter = string.IsNullOrWhiteSpace(status) ? "All" : status;
            Model.CurrentPage = 1;
            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }
    public async Task FilterByCategoryAsync(long categoryId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentCategoryFilter = categoryId <= 0 ? null : categoryId;
            Model.CurrentPage = 1;
            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SortBy(string? sortBy)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (_currentSortBy == (sortBy ?? string.Empty))
                {
                    _sortDescending = !_sortDescending;
                }
                else
                {
                    _currentSortBy = sortBy ?? string.Empty;
                    _sortDescending = false;
                }
            }

            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task GoToNextPageAsync()
    {
        if (Model.CurrentPage >= Model.TotalPages) return;

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage++;
            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }


    public async Task GoToPreviousPageAsync()
    {
        if (Model.CurrentPage <= 1) return;

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage--;
            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ChangePageSizeAsync(int pageSize)
    {
        if (pageSize <= 0) return;

        await _semaphore.WaitAsync();
        try
        {
            Model.PageSize = pageSize;
            Model.CurrentPage = 1;
            await ApplyFiltersAndPaging();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task RefreshCacheAsync()
    {
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    private string GetCategoryName(long? categoryId)
    {
        if (!categoryId.HasValue) return string.Empty;
        var cat = Model.Categories.FirstOrDefault(c => c.Id == categoryId.Value);
        return cat?.Name ?? string.Empty;
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}

public class IngredientsLoadedEventArgs : EventArgs
{
    public List<IngredientViewModel> Ingredients { get; }

    public IngredientsLoadedEventArgs(List<IngredientViewModel> ingredients)
    {
        Ingredients = ingredients;
    }
}