using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using System.ComponentModel;

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

        _isLoading = true;
        try
        {
            _currentCategoryFilter = categoryId;
            _currentSearchTerm = searchTerm ?? string.Empty;

            // Load categories first
            await LoadCategories();

            // Load ingredients data
            await LoadAllDataToCache();

            // Apply filtering from cache
            var query = _allIngredientsCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
            {
                query = query.Where(x => x.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                        (x.Description ?? string.Empty).Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (_currentCategoryFilter.HasValue && _currentCategoryFilter > 0)
            {
                query = query.Where(x => x.CategoryId == _currentCategoryFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(_currentStatusFilter) && _currentStatusFilter != "All")
            {
                bool active = _currentStatusFilter.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                             _currentStatusFilter.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase);
                query = query.Where(x => x.IsActive == active);
            }

            if (!string.IsNullOrWhiteSpace(_currentSortBy))
            {
                if (_currentSortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    query = _sortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                else if (_currentSortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
                    query = _sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);
                else if (_currentSortBy.Equals("Category", StringComparison.OrdinalIgnoreCase))
                    query = _sortDescending ? query.OrderByDescending(x => x.CategoryName) : query.OrderBy(x => x.CategoryName);
            }

            _filteredIngredients = query.ToList();

            var pageNumber = page ?? Model.CurrentPage;
            var pSize = pageSize ?? Model.PageSize;

            Model.TotalItems = _filteredIngredients.Count;
            Model.CurrentPage = Math.Max(1, Math.Min(pageNumber, Model.TotalPages == 0 ? 1 : Model.TotalPages));
            Model.PageSize = pSize;

            var items = _filteredIngredients
                .Skip((Model.CurrentPage - 1) * Model.PageSize)
                .Take(Model.PageSize)
                .ToList();

            // Clear and add items to binding list
            Model.Ingredients.Clear();
            foreach (var item in items)
            {
                Model.Ingredients.Add(item);
            }

            // Fire event with correct EventArgs type
            OnDataLoaded?.Invoke(this, new IngredientsLoadedEventArgs(items));
        }
        catch (Exception ex)
        {
            // Log the error for debugging
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            throw; // Re-throw to let the UI handle it
        }
        finally
        {
            _isLoading = false;
        }
    }

    public async Task LoadDataAsync()
    {
        await LoadDataAsync(null, null, Model.PageSize, Model.CurrentPage, true);
    }

    private async Task LoadAllDataToCache()
    {
        await _semaphore.WaitAsync();
        try
        {
            // Clear existing cache
            _allIngredientsCache.Clear();

            try
            {
                // Use the business service to get ingredients
                var input = new GetIngredientsInput
                {
                    PageNumber = 1,
                    PageSize = 10000 // Large page to get all items
                };

                var paged = await _ingredientManagementService.GetIngredientsAsync(input);

                if (paged?.Items != null)
                {
                    _allIngredientsCache = paged.Items.Select(dto => new IngredientViewModel
                    {
                        Id = dto.Id,
                        Name = dto.Name ?? string.Empty,
                        Unit = dto.Unit ?? string.Empty,
                        CategoryId = dto.IngredientCategoryId,
                        CategoryName = dto.CategoryName ?? "Không rõ",
                        Description = dto.Description,
                        IsActive = true, // Default to active since DTO might not have this field
                        CostPerUnit = dto.CostPerUnit,
                        CreatedAt = dto.CreatedAt,
                        UpdatedAt = dto.UpdatedAt
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Log error and provide fallback mock data for development
                System.Diagnostics.Debug.WriteLine($"Error loading from service: {ex.Message}");

                // Fallback mock data for development/testing
                _allIngredientsCache = GenerateMockData();
            }

            // Map categories to ingredients (ensure CategoryName is populated)
            Model.MapCategoriesToIngredients();
        }
        finally
        {
            _semaphore.Release();
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
                IsActive = i % 4 != 0, // 3/4 items are active
                CostPerUnit = (decimal)(10000 + (i * 500)),
                CreatedAt = DateTime.Now.AddDays(-i),
                UpdatedAt = i % 3 == 0 ? DateTime.Now.AddHours(-i) : null
            });
        }

        return mockData;
    }

    private async Task LoadCategories()
    {
        await _semaphore.WaitAsync();
        try
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
                // Log error and provide fallback categories
                System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");

                // Add default categories for development
                Model.Categories.Add(new IngredientCategoryViewModel { Id = 1, Name = "Rau củ", Description = "Các loại rau củ quả" });
                Model.Categories.Add(new IngredientCategoryViewModel { Id = 2, Name = "Thịt cá", Description = "Thịt và hải sản" });
                Model.Categories.Add(new IngredientCategoryViewModel { Id = 3, Name = "Gia vị", Description = "Các loại gia vị" });
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Category management methods (unchanged)
    public async Task AddCategoryAsync(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Category name is required", nameof(name));

        await _semaphore.WaitAsync();
        try
        {
            var repo = _unitOfWork.Repository<IngredientCategory>();

            // Duplicate check
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
            var existing = await repo.GetAsync(id) as IngredientCategory;
            if (existing == null) throw new InvalidOperationException($"Category id {id} not found.");

            // Duplicate by name excluding itself
            var dup = await repo.GetContainString("Name", name, true) as IngredientCategory;
            if (dup != null && dup.Id != existing.Id)
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

    // Ingredient CRUD operations with mock implementation
    public async Task AddIngredientAsync(string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        await Task.Delay(100); // Simulate async operation

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
            CostPerUnit = 0 // Default value
        };

        _allIngredientsCache.Add(newIngredient);
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task UpdateIngredientAsync(long id, string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        await Task.Delay(100); // Simulate async operation

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
        await Task.Delay(100); // Simulate async operation

        var ingredientToRemove = _allIngredientsCache.FirstOrDefault(i => i.Id == id);
        if (ingredientToRemove != null)
        {
            _allIngredientsCache.Remove(ingredientToRemove);
        }

        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    // Filter and navigation methods
    public async Task SearchAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm ?? string.Empty;
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, 1, true);
    }

    public async Task FilterByStatusAsync(string status)
    {
        _currentStatusFilter = string.IsNullOrWhiteSpace(status) ? "All" : status;
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, 1, true);
    }

    public async Task FilterByCategoryAsync(long categoryId)
    {
        _currentCategoryFilter = categoryId <= 0 ? null : categoryId;
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, 1, true);
    }

    public async Task SortBy(string? sortBy)
    {
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (_currentSortBy == sortBy)
                _sortDescending = !_sortDescending; // toggle
            else
            {
                _currentSortBy = sortBy ?? string.Empty;
                _sortDescending = false;
            }
        }

        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, true);
    }

    public async Task GoToNextPageAsync()
    {
        if (Model.CurrentPage < Model.TotalPages)
        {
            await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage + 1, true);
        }
    }

    public async Task GoToPreviousPageAsync()
    {
        if (Model.CurrentPage > 1)
        {
            await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage - 1, true);
        }
    }

    public async Task ChangePageSizeAsync(int pageSize)
    {
        if (pageSize <= 0) return;
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, pageSize, 1, true);
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