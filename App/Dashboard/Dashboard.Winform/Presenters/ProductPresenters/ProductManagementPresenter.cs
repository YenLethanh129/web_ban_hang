using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.BussinessLogic.Dtos;
using System.ComponentModel;
using Dashboard.BussinessLogic.Services.ProductServices;

namespace Dashboard.Winform.Presenters.ProductPresenters;

public interface IProductManagementPresenter : IManagementPresenter<ProductManagementModel>
{
    Task LoadDataAsync(long? categoryId = null, string? searchTerm = null,
        int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task FilterByCategoryAsync(long? categoryId);
    Task SortBy(string? sortBy);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
    Task LoadCategoriesAsync();
}

public class ProductManagementPresenter : IProductManagementPresenter
{
    private readonly ILogger<ProductManagementPresenter> _logger;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<ProductViewModel> _allProductsCache = [];
    private List<ProductViewModel> _filteredProducts = [];
    private string _currentSearchTerm = string.Empty;
    private string _currentStatusFilter = "All";
    private long? _currentCategoryFilter = null;
    private string _currentSortBy = string.Empty;
    private bool _sortDescending = false;

    IManagableModel IManagementPresenter<ProductManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

    public event EventHandler? OnDataLoaded;

    public ProductManagementPresenter(
        ILogger<ProductManagementPresenter> logger,
        IUnitOfWork unitOfWork,
        ICategoryService categoryService,
        IProductService productService,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _productService = productService;
        _categoryService = categoryService;
        _mapper = mapper;
        Model = new ProductManagementModel();
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

            if (!_allProductsCache.Any() || forceRefresh)
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
        try
        {
            _logger.LogInformation("Loading all products to cache");

            var input = new GetProductsInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };

            var pagedResult = await _productService.GetProductsAsync(input);
            _allProductsCache = _mapper.Map<List<ProductViewModel>>(pagedResult.Items);

            // Load sold quantities in separate call to avoid connection conflicts
            try
            {
                var soldQuantities = await _productService.GetSoldQuantitiesAsync();
                foreach (var product in _allProductsCache)
                {
                    product.SoldQuantity = soldQuantities.GetValueOrDefault(product.Id, 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not load sold quantities, continuing without them");
            }

            _logger.LogInformation("Loaded {Count} products to cache", _allProductsCache.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading all products to cache");
            throw;
        }
    }

    private async Task LoadCategories()
    {
        try
        {
            _logger.LogInformation("Loading categories");

            var categories = await _categoryService.GetAllCategories();
            var listCategories = categories.Items.ToList();

            var categoryViewModels = _mapper.Map<List<CategoryViewModel>>(listCategories);

            Model.Categories.Clear();

            Model.Categories.Add(new CategoryViewModel
            {
                Id = 0,
                Name = "Tất cả"
            });

            foreach (var cat in categoryViewModels)
                Model.Categories.Add(cat);

            _logger.LogInformation("Loaded {Count} categories", Model.Categories.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories");
            throw;
        }
    }


    private void ApplyFiltersAndSort()
    {
        var query = _allProductsCache.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
        {
            query = query.Where(p =>
                p.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply status filter
        if (_currentStatusFilter != "All")
        {
            bool isActive = _currentStatusFilter == "Active";
            query = query.Where(p => p.IsActive == isActive);
        }

        // Apply category filter
        if (_currentCategoryFilter.HasValue && _currentCategoryFilter > 0)
        {
            query = query.Where(p => p.CategoryId == _currentCategoryFilter.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(_currentSortBy))
        {
            query = _currentSortBy.ToLower() switch
            {
                "id" => _sortDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                "name" => _sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => _sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "category" => _sortDescending ? query.OrderByDescending(p => p.CategoryName) : query.OrderBy(p => p.CategoryName),
                "soldquantity" => _sortDescending ? query.OrderByDescending(p => p.SoldQuantity) : query.OrderBy(p => p.SoldQuantity),
                _ => query.OrderBy(p => p.Id)
            };
        }

        _filteredProducts = query.ToList();

        UpdateModelWithPagination();

        var currentPageProducts = GetCurrentPageProducts();
        OnDataLoaded?.Invoke(this, new ProductsLoadedEventArgs
        {
            Products = currentPageProducts,
            TotalCount = _filteredProducts.Count
        });
    }

    private void UpdateModelWithPagination()
    {
        Model.TotalItems = _filteredProducts.Count;

        if (Model.CurrentPage < 1) Model.CurrentPage = 1;
        if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
            Model.CurrentPage = Model.TotalPages;
    }

    private List<ProductViewModel> GetCurrentPageProducts()
    {
        int skip = (Model.CurrentPage - 1) * Model.PageSize;
        if (skip < 0) skip = 0;
        return _filteredProducts.Skip(skip).Take(Model.PageSize).ToList();
    }

    public async Task SearchAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm ?? string.Empty;
        Model.CurrentPage = 1;
        await LoadDataAsync(_currentCategoryFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, false);
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

    public async Task FilterByCategoryAsync(long? categoryId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentCategoryFilter = categoryId;
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

    public async Task LoadCategoriesAsync()
    {
        // This method is kept for backward compatibility
        // But categories are now loaded automatically in LoadDataAsync
        if (!Model.Categories.Any())
        {
            await LoadCategories();
        }
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}