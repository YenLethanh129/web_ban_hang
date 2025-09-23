using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.Suppliers;
using Dashboard.BussinessLogic.Dtos;
using System.ComponentModel;
using Dashboard.BussinessLogic.Services.SupplierServices;

namespace Dashboard.Winform.Presenters.SupplierPresenters;

public interface ISupplierManagementPresenter : IManagementPresenter<SupplierManagementModel>
{
    Task LoadDataAsync(long? supplierTypeId = null, string? searchTerm = null,
        int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task FilterBySupplierTypeAsync(long? supplierTypeId);
    Task SortBy(string? sortBy);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
    Task LoadSupplierTypesAsync();
}

public class SupplierManagementPresenter : ISupplierManagementPresenter
{
    private readonly ILogger<SupplierManagementPresenter> _logger;
    private readonly ISupplierManagementService _supplierService;
    //private readonly ISupplierTypeService _supplierTypeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SupplierManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<SupplierViewModel> _allSuppliersCache = [];
    private List<SupplierViewModel> _filteredSuppliers = [];
    private string _currentSearchTerm = string.Empty;
    private string _currentStatusFilter = "All";
    private long? _currentSupplierTypeFilter = null;
    private string _currentSortBy = string.Empty;
    private bool _sortDescending = false;

    IManagableModel IManagementPresenter<SupplierManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

    public event EventHandler? OnDataLoaded;

    public SupplierManagementPresenter(
        ILogger<SupplierManagementPresenter> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
,
        ISupplierManagementService supplierService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        Model = new SupplierManagementModel();
        _supplierService = supplierService;
    }

    public async Task LoadDataAsync(
        long? supplierTypeId = null,
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

            if (!_allSuppliersCache.Any() || forceRefresh)
            {
                await LoadAllDataToCache();
            }

            //if (!Model.SupplierTypes.Any())
            //{
            //    await LoadSupplierTypes();
            //}

            _currentSearchTerm = searchTerm ?? string.Empty;
            _currentStatusFilter = "All";
            _currentSupplierTypeFilter = supplierTypeId;

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
            _logger.LogInformation("Loading all suppliers to cache");

            var input = new GetSuppliersInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            };

            var pagedResult = await _supplierService.GetSuppliersAsync(input);
            _allSuppliersCache = _mapper.Map<List<SupplierViewModel>>(pagedResult.Items);

            _logger.LogInformation("Loaded {Count} suppliers to cache", _allSuppliersCache.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading all suppliers to cache");
            throw;
        }
    }

    //private async Task LoadSupplierTypes()
    //{
    //    try
    //    {
    //        _logger.LogInformation("Loading supplier types");

    //        var supplierTypes = await _supplierTypeService.GetAllSupplierTypes();
    //        var listSupplierTypes = supplierTypes.Items.ToList();

    //        var supplierTypeViewModels = _mapper.Map<List<SupplierTypeViewModel>>(listSupplierTypes);

    //        Model.SupplierTypes.Clear();

    //        Model.SupplierTypes.Add(new SupplierTypeViewModel
    //        {
    //            Id = 0,
    //            Name = "Tất cả"
    //        });

    //        foreach (var type in supplierTypeViewModels)
    //            Model.SupplierTypes.Add(type);

    //        _logger.LogInformation("Loaded {Count} supplier types", Model.SupplierTypes.Count);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error loading supplier types");
    //        throw;
    //    }
    //}

    private void ApplyFiltersAndSort()
    {
        var query = _allSuppliersCache.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
        {
            query = query.Where(s =>
                s.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.Name.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.Email!.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply status filter
        if (_currentStatusFilter != "All")
        {
            bool isActive = _currentStatusFilter == "ACTIVE";
            query = query.Where(s => s.IsActive == isActive);
        }

        //// Apply supplier type filter
        //if (_currentSupplierTypeFilter.HasValue && _currentSupplierTypeFilter > 0)
        //{
        //    query = query.Where(s => s.SupplierTypeId == _currentSupplierTypeFilter.Value);
        //}

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(_currentSortBy))
        {
            query = _currentSortBy.ToLower() switch
            {
                "id" => _sortDescending ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
                "name" => _sortDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "email" => _sortDescending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                "phone" => _sortDescending ? query.OrderByDescending(s => s.Phone) : query.OrderBy(s => s.Phone),
                //"contactperson" => _sortDescending ? query.OrderByDescending(s => s.ContactPerson) : query.OrderBy(s => s.ContactPerson),
                "createdat" => _sortDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                _ => query.OrderBy(s => s.Id)
            };
        }

        _filteredSuppliers = query.ToList();

        UpdateModelWithPagination();

        var currentPageSuppliers = GetCurrentPageSuppliers();
        OnDataLoaded?.Invoke(this, new SuppliersLoadedEventArgs
        {
            Suppliers = currentPageSuppliers,
            TotalCount = _filteredSuppliers.Count
        });
    }

    private void UpdateModelWithPagination()
    {
        Model.TotalItems = _filteredSuppliers.Count;

        if (Model.CurrentPage < 1) Model.CurrentPage = 1;
        if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
            Model.CurrentPage = Model.TotalPages;
    }

    private List<SupplierViewModel> GetCurrentPageSuppliers()
    {
        int skip = (Model.CurrentPage - 1) * Model.PageSize;
        if (skip < 0) skip = 0;
        return _filteredSuppliers.Skip(skip).Take(Model.PageSize).ToList();
    }

    public async Task SearchAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm ?? string.Empty;
        Model.CurrentPage = 1;
        await LoadDataAsync(_currentSupplierTypeFilter, _currentSearchTerm, Model.PageSize, Model.CurrentPage, false);
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

    public async Task FilterBySupplierTypeAsync(long? supplierTypeId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentSupplierTypeFilter = supplierTypeId;
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

    //public async Task LoadSupplierTypesAsync()
    //{
    //    // This method is kept for backward compatibility
    //    // But supplier types are now loaded automatically in LoadDataAsync
    //    if (!Model.SupplierTypes.Any())
    //    {
    //        await LoadSupplierTypes();
    //    }
    //}

    public void Dispose()
    {
        _semaphore?.Dispose();
    }

    public Task LoadSupplierTypesAsync()
    {
        throw new NotImplementedException();
    }
}