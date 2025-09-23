using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.BussinessLogic.Services.ProductServices;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.ComponentModel;

namespace Dashboard.Winform.Presenters.RecipePresenters;

public interface IRecipeManagementPresenter : IManagementPresenter<RecipeManagementModel>
{
    Task LoadDataAsync(long? productId = null, string? searchTerm = null,
        int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task FilterByProductAsync(long? productId);
    Task SortBy(string? sortBy);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
    Task LoadProductsAsync();
}

public class RecipeManagementPresenter : IRecipeManagementPresenter
{
    private readonly ILogger<RecipeManagementPresenter> _logger;
    private readonly IRecipeService _recipeService;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public RecipeManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<RecipeViewModel> _allRecipesCache = [];
    private List<RecipeViewModel> _filteredRecipes = [];
    private string _currentSearchTerm = string.Empty;
    private string _currentStatusFilter = "All";
    private long? _currentProductFilter = null;
    private string _currentSortBy = string.Empty;
    private bool _sortDescending = false;

    IManagableModel IManagementPresenter<RecipeManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

    public event EventHandler? OnDataLoaded;

    public RecipeManagementPresenter(
        ILogger<RecipeManagementPresenter> logger,
        IRecipeService recipeService,
        IProductService productService,
        IMapper mapper)
    {
        _logger = logger;
        _recipeService = recipeService;
        _productService = productService;
        _mapper = mapper;
        Model = new RecipeManagementModel();
    }

    public async Task LoadDataAsync(
        long? productId = null,
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

            if (!_allRecipesCache.Any() || forceRefresh)
            {
                await LoadAllDataToCache();
            }

            if (!Model.Products.Any())
            {
                await LoadProducts();
            }

            _currentSearchTerm = searchTerm ?? string.Empty;
            _currentStatusFilter = "All";
            _currentProductFilter = productId;

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
        try
        {
            _logger.LogInformation("Loading all recipes to cache");

            var input = new GetRecipesInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };

            var pagedResult = await _recipeService.GetRecipesAsync(input);
            _allRecipesCache = _mapper.Map<List<RecipeViewModel>>(pagedResult.Items);

            _logger.LogInformation("Loaded {Count} recipes to cache", _allRecipesCache.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading all recipes to cache");
            throw;
        }
    }

    private async Task LoadProducts()
    {
        try
        {
            _logger.LogInformation("Loading products for recipe filtering");

            var input = new GetProductsInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                IsActive = true
            };

            var pagedResult = await _productService.GetProductsAsync(input);
            var productViewModels = _mapper.Map<List<ProductViewModel>>(pagedResult.Items);

            Model.Products.Clear();
            Model.Products.Add(new ProductViewModel
            {
                Id = 0,
                Name = "Tất cả sản phẩm"
            });

            foreach (var product in productViewModels)
                Model.Products.Add(product);

            _logger.LogInformation("Loaded {Count} products for filtering", Model.Products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading products");

            // Fallback data
            Model.Products.Clear();
            Model.Products.Add(new ProductViewModel { Id = 0, Name = "Tất cả sản phẩm" });
            Model.Products.Add(new ProductViewModel { Id = 1, Name = "Cà phê đen" });
            Model.Products.Add(new ProductViewModel { Id = 2, Name = "Cà phê sữa" });
            Model.Products.Add(new ProductViewModel { Id = 3, Name = "Bánh mì thịt" });
        }
    }

    private void ApplyFiltersAndSort()
    {
        var query = _allRecipesCache.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
        {
            query = query.Where(r =>
                r.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrEmpty(r.Description) 
                    && r.Description.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.ProductName.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply status filter
        if (_currentStatusFilter != "All")
        {
            bool isActive = _currentStatusFilter == "ACTIVE";
            query = query.Where(r => r.IsActive == isActive);
        }

        // Apply product filter
        if (_currentProductFilter.HasValue && _currentProductFilter > 0)
        {
            query = query.Where(r => r.ProductId == _currentProductFilter.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(_currentSortBy))
        {
            query = _currentSortBy.ToLower() switch
            {
                "id" => _sortDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                "name" => _sortDescending ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                "product" => _sortDescending ? query.OrderByDescending(r => r.ProductName) : query.OrderBy(r => r.ProductName),
                "servingsize" => _sortDescending ? query.OrderByDescending(r => r.ServingSize) : query.OrderBy(r => r.ServingSize),
                "createdat" => _sortDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                _ => query.OrderByDescending(r => r.Id)
            };
        }
        else
        {
            query = query.OrderByDescending(r => r.CreatedAt);
        }

        _filteredRecipes = query.ToList();

        UpdateModelWithPagination();

        var currentPageRecipes = GetCurrentPageRecipes();
        OnDataLoaded?.Invoke(this, new RecipesLoadedEventArgs
        {
            Recipes = currentPageRecipes,
            TotalCount = _filteredRecipes.Count
        });
    }

    private void UpdateModelWithPagination()
    {
        Model.TotalItems = _filteredRecipes.Count;

        if (Model.CurrentPage < 1) Model.CurrentPage = 1;
        if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
            Model.CurrentPage = Model.TotalPages;
    }

    private List<RecipeViewModel> GetCurrentPageRecipes()
    {
        int skip = (Model.CurrentPage - 1) * Model.PageSize;
        if (skip < 0) skip = 0;
        return _filteredRecipes.Skip(skip).Take(Model.PageSize).ToList();
    }

    public async Task SearchAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm ?? string.Empty;
        Model.CurrentPage = 1;
        await LoadDataAsync(_currentProductFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, false);
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

    public async Task FilterByProductAsync(long? productId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentProductFilter = productId;
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

    public async Task LoadProductsAsync()
    {
        if (!Model.Products.Any())
        {
            await LoadProducts();
        }
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}