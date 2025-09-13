using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;

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
}

public class IngredientManagementPresenter : IIngredientManagementPresenter
{
    // TODO: Inject actual services when backend is ready
    // private readonly IIngredientManagementService _ingredientManagementService;
    // private readonly IIngredientCategoryService _categoryService;
    // private readonly IUnitOfWork _unitOfWork;

    public IngredientManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<IngredientViewModel> _allIngredientsCache = [];
    private List<IngredientViewModel> _filteredIngredients = [];
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
        // TODO: Add service dependencies when backend is ready
        // IIngredientManagementService ingredientManagementService,
        // IUnitOfWork unitOfWork,
        // IIngredientCategoryService categoryService
        )
    {
        // TODO: Initialize services when backend is ready
        // _ingredientManagementService = ingredientManagementService;
        // _unitOfWork = unitOfWork;
        // _categoryService = categoryService;

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

            if (!_allIngredientsCache.Any() || forceRefresh)
            {
                await LoadAllDataToCache();
            }

            if (!Model.Categories.Any())
            {
                await LoadCategories();
            }

            _currentSearchTerm = searchTerm ?? string.Empty;
            _currentStatusFilter = "All";
            _currentCategoryFilter = categoryId;

            Model.PageSize = pageSize ?? 10;
            Model.CurrentPage = page ?? 1;

            ApplyFiltersAndSort();
        }
        finally
        {
            _isLoading = false;
            _semaphore.Release();
        }
    }

    public async Task LoadDataAsync()
    {
        await LoadDataAsync(null, null, null, null);
    }

    private async Task LoadAllDataToCache()
    {
        // TODO: Replace with actual service call
        // var input = new GetIngredientsInput
        // {
        //     PageNumber = 1,
        //     PageSize = int.MaxValue
        // };

        // var ingredients = await _ingredientManagementService.GetIngredientsAsync(input);

        // Mock data for now
        await Task.Delay(100); // Simulate async call
        _allIngredientsCache = GenerateMockIngredients();
    }

    private async Task LoadCategories()
    {
        // TODO: Replace with actual service call
        // var categories = await _categoryService.GetAllCategoriesAsync();

        // Mock data for now
        await Task.Delay(50); // Simulate async call
        Model.Categories.Clear();

        var mockCategories = new List<IngredientCategoryViewModel>
        {
            new() { Id = 0, Name = "Tất cả", Description = "Tất cả danh mục" },
            new() { Id = 1, Name = "Thịt", Description = "Các loại thịt tươi sống" },
            new() { Id = 2, Name = "Rau củ", Description = "Rau xanh và củ quả" },
            new() { Id = 3, Name = "Gia vị", Description = "Các loại gia vị, bột nêm" },
            new() { Id = 4, Name = "Hải sản", Description = "Cá, tôm, cua, ghẹ" },
            new() { Id = 5, Name = "Đồ khô", Description = "Miến, bún, bánh tráng" }
        };

        foreach (var category in mockCategories)
        {
            Model.Categories.Add(category);
        }
    }

    private List<IngredientViewModel> GenerateMockIngredients()
    {
        var random = new Random();
        var ingredients = new List<IngredientViewModel>();

        var mockData = new[]
        {
            new { Name = "Thịt heo ba chỉ", Unit = "kg", CategoryId = 1L },
            new { Name = "Thịt bò thăn", Unit = "kg", CategoryId = 1L },
            new { Name = "Gà ta", Unit = "con", CategoryId = 1L },
            new { Name = "Cà rót", Unit = "kg", CategoryId = 2L },
            new { Name = "Cà chua", Unit = "kg", CategoryId = 2L },
            new { Name = "Hành tây", Unit = "kg", CategoryId = 2L },
            new { Name = "Tỏi", Unit = "kg", CategoryId = 2L },
            new { Name = "Muối", Unit = "gói", CategoryId = 3L },
            new { Name = "Đường cát", Unit = "kg", CategoryId = 3L },
            new { Name = "Nước mắm", Unit = "chai", CategoryId = 3L },
            new { Name = "Tôm sú", Unit = "kg", CategoryId = 4L },
            new { Name = "Cá tra", Unit = "kg", CategoryId = 4L },
            new { Name = "Bún tươi", Unit = "kg", CategoryId = 5L },
            new { Name = "Bánh tráng", Unit = "gói", CategoryId = 5L },
        };

        for (int i = 0; i < mockData.Length; i++)
        {
            var item = mockData[i];
            ingredients.Add(new IngredientViewModel
            {
                Id = i + 1,
                Name = item.Name,
                Unit = item.Unit,
                CategoryId = item.CategoryId,
                CategoryName = GetCategoryName(item.CategoryId),
                Description = $"Mô tả cho {item.Name}",
                IsActive = random.NextDouble() > 0.1, // 90% active
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 365)),
                UpdatedAt = DateTime.Now.AddDays(-random.Next(1, 30))
            });
        }

        return ingredients;
    }

    private string GetCategoryName(long categoryId)
    {
        return categoryId switch
        {
            1 => "Thịt",
            2 => "Rau củ",
            3 => "Gia vị",
            4 => "Hải sản",
            5 => "Đồ khô",
            _ => "Khác"
        };
    }

    private void ApplyFiltersAndSort()
    {
        var query = _allIngredientsCache.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
        {
            query = query.Where(ing =>
                ing.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                ing.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                ing.Unit.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply status filter
        if (_currentStatusFilter != "All")
        {
            bool isActive = _currentStatusFilter == "Active";
            query = query.Where(ing => ing.IsActive == isActive);
        }

        // Apply category filter
        if (_currentCategoryFilter.HasValue && _currentCategoryFilter > 0)
        {
            query = query.Where(ing => ing.CategoryId == _currentCategoryFilter.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(_currentSortBy))
        {
            query = _currentSortBy.ToLower() switch
            {
                "id" => _sortDescending ? query.OrderByDescending(i => i.Id) : query.OrderBy(i => i.Id),
                "name" => _sortDescending ? query.OrderByDescending(i => i.Name) : query.OrderBy(i => i.Name),
                "category" => _sortDescending ? query.OrderByDescending(i => i.CategoryName) : query.OrderBy(i => i.CategoryName),
                "unit" => _sortDescending ? query.OrderByDescending(i => i.Unit) : query.OrderBy(i => i.Unit),
                "status" => _sortDescending ? query.OrderByDescending(i => i.IsActive) : query.OrderBy(i => i.IsActive),
                "created date" => _sortDescending ? query.OrderByDescending(i => i.CreatedAt) : query.OrderBy(i => i.CreatedAt),
                _ => query.OrderBy(i => i.Id)
            };
        }

        _filteredIngredients = query.ToList();

        UpdateModelWithPagination();

        var currentPageIngredients = GetCurrentPageIngredients();
        OnDataLoaded?.Invoke(this, new IngredientsLoadedEventArgs(currentPageIngredients));
    }

    private void UpdateModelWithPagination()
    {
        Model.TotalItems = _filteredIngredients.Count;

        if (Model.CurrentPage < 1) Model.CurrentPage = 1;
        if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
            Model.CurrentPage = Model.TotalPages;
    }

    private List<IngredientViewModel> GetCurrentPageIngredients()
    {
        int skip = (Model.CurrentPage - 1) * Model.PageSize;
        return [.. _filteredIngredients.Skip(skip).Take(Model.PageSize)];
    }

    public async Task SearchAsync(string searchTerm)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentSearchTerm = searchTerm ?? string.Empty;
            Model.SearchText = _currentSearchTerm;
            Model.CurrentPage = 1;
            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task FilterByStatusAsync(string status)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentStatusFilter = status ?? "All";
            Model.CurrentPage = 1;
            ApplyFiltersAndSort();
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
            _currentCategoryFilter = categoryId > 0 ? categoryId : null;
            Model.CurrentPage = 1;
            ApplyFiltersAndSort();
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
            if (_currentSortBy == sortBy)
            {
                _sortDescending = !_sortDescending;
            }
            else
            {
                _currentSortBy = sortBy ?? string.Empty;
                _sortDescending = false;
            }

            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task GoToNextPageAsync()
    {
        if (Model.CurrentPage >= Model.TotalPages)
            return;

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage++;
            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task GoToPreviousPageAsync()
    {
        if (Model.CurrentPage <= 1)
            return;

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage--;
            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ChangePageSizeAsync(int pageSize)
    {
        await _semaphore.WaitAsync();
        try
        {
            Model.PageSize = pageSize;
            Model.CurrentPage = 1;
            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task RefreshCacheAsync()
    {
        await LoadDataAsync(forceRefresh: true);
    }

    // TODO: Implement when backend services are ready
    public async Task AddIngredientAsync(string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        // TODO: Replace with actual service call
        // var input = new CreateIngredientInput
        // {
        //     Name = name,
        //     Unit = unit,
        //     CategoryId = categoryId,
        //     Description = description,
        //     IsActive = isActive,
        //     TaxId = taxId
        // };

        // await _ingredientManagementService.AddIngredientAsync(input);

        // Mock implementation for now
        await Task.Delay(100);
        var newIngredient = new IngredientViewModel
        {
            Id = _allIngredientsCache.Max(i => i.Id) + 1,
            Name = name,
            Unit = unit,
            CategoryId = categoryId,
            CategoryName = GetCategoryName(categoryId),
            Description = description,
            IsActive = isActive,
            TaxId = taxId,
            CreatedAt = DateTime.Now
        };

        _allIngredientsCache.Add(newIngredient);
        await RefreshCacheAsync();
    }

    public async Task UpdateIngredientAsync(long id, string name, string unit, long categoryId,
        string? description, bool isActive, long? taxId)
    {
        // TODO: Replace with actual service call
        // var input = new UpdateIngredientInput
        // {
        //     Id = id,
        //     Name = name,
        //     Unit = unit,
        //     CategoryId = categoryId,
        //     Description = description,
        //     IsActive = isActive,
        //     TaxId = taxId
        // };

        // await _ingredientManagementService.UpdateIngredientAsync(input);

        // Mock implementation for now
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

        await RefreshCacheAsync();
    }

    public async Task DeleteIngredientAsync(long id)
    {
        // TODO: Replace with actual service call
        // await _ingredientManagementService.DeleteIngredientAsync(id);

        // Mock implementation for now
        await Task.Delay(100);
        var ingredientToRemove = _allIngredientsCache.FirstOrDefault(i => i.Id == id);
        if (ingredientToRemove != null)
        {
            _allIngredientsCache.Remove(ingredientToRemove);
        }

        await RefreshCacheAsync();
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}

// Event args for ingredient data loaded
public class IngredientsLoadedEventArgs : EventArgs
{
    public List<IngredientViewModel> Ingredients { get; }

    public IngredientsLoadedEventArgs(List<IngredientViewModel> ingredients)
    {
        Ingredients = ingredients;
    }
}