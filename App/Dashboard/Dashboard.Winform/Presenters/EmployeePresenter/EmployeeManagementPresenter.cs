using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.EmployeeModels;
using ReaLTaiizor.Controls;
using Sprache;
using System.ComponentModel;
using System.Threading;

namespace Dashboard.Winform.Presenters.EmployeePresenter;

public interface IEmployeeManagementPresenter : IManagementPresenter<EmployeeManagementModel>
{
    Task LoadDataAsync(long? position = null, long? branchId = null,
        string? branchName = null, string? searchTerm = null, int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task AddEmployeeAsync(string name, long positionId, string email, string phone,
        DateTime hireDate, DateTime resignDate, string status, decimal baseSalary, string salaryType);
    Task UpdateUserAsync(long id, string name, long positionId, string email, string phone,
        DateTime hireDate, DateTime resignDate, string status, decimal baseSalary, string salaryType);
    Task DeleteUserAsync(long id);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task FilterByPositionAsync(long positionId);
    Task FilterByBranchAsync(long? branchId);
    Task SortBy(string? sortBy);
    Task<PagedList<EmployeeManagementModel>> GoToNextPageAsync();
    Task<PagedList<EmployeeManagementModel>> GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
}

public class EmployeeManagementPresenter : IEmployeeManagementPresenter
{
    private readonly IEmployeeManagementService _employeeManagementService;
    private readonly IRoleManagementService _roleManagementService;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isLoading = false;

    // Cache properties
    private List<EmployeeViewModel> _allEmployeesCache = [];
    private List<EmployeeViewModel> _filteredEmployees = [];
    private string _currentSearchTerm = string.Empty;
    private string _currentStatusFilter = "All";
    private long? _currentPositionFilter = null;
    private long? _currentBranchFilter = null;
    private string _currentSortBy = string.Empty;
    private bool _sortDescending = false;

    IManagableModel IManagementPresenter<EmployeeManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

    public event EventHandler? OnDataLoaded;

    public EmployeeManagementPresenter(
        IEmployeeManagementService employeeManagementService,
        IUnitOfWork unitOfWork,
        IRoleManagementService roleManagementService)
    {
        _employeeManagementService = employeeManagementService;
        _unitOfWork = unitOfWork;
        _roleManagementService = roleManagementService;

        Model = new EmployeeManagementModel();
    }

    public async Task LoadDataAsync(
        long? position = null,
        long? branchId = null,
        string? branchName = null,
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

            if (!_allEmployeesCache.Any() || forceRefresh)
            {
                await LoadAllDataToCache();
            }

            if (!Model.Positions.Any())
            {
                await LoadPositions();
            }

            _currentSearchTerm = searchTerm ?? _currentSearchTerm ?? string.Empty;
            _currentStatusFilter = _currentStatusFilter ?? "All";
            _currentPositionFilter = position ?? _currentPositionFilter;
            _currentBranchFilter = branchId ?? _currentBranchFilter;

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
        var input = new GetEmployeesInput
        {
            PageNumber = 1,
            PageSize = int.MaxValue
        };

        var employees = await _employeeManagementService.GetEmployeesAsync(input);

        _allEmployeesCache = [.. employees.Items
            .Select(emp => new EmployeeViewModel
            {
                Id = emp.Id,
                FullName = emp.FullName,
                PositionId = emp.PositionId,
                PositionName = emp.PositionName,
                Email = emp.Email,
                PhoneNumber = emp.Phone,
                HireDate = emp.HireDate,
                IsActive = emp.Status == "ACTIVE",
                BranchId = emp.BranchId,
            })];
    }

    private async Task LoadPositions()
    {
        var positions = await _unitOfWork.Repository<EmployeePosition>().GetAllAsync(asNoTracking: true);
        Model.Positions.Clear();
        foreach (var pos in positions)
        {
            Model.Positions.Add(new PositionViewModel
            {
                Id = pos.Id,
                Name = pos.Name
            });
        }
        Model.Positions.Add(new PositionViewModel
        {
            Id = 0,
            Name = "Tất cả"
        });
    }

    private string GetPositionName(long positionId)
    {
        return Model.Positions.FirstOrDefault(p => p.Id == positionId)?.Name ?? "Unknown";
    }

    private void ApplyFiltersAndSort()
    {
        var query = _allEmployeesCache.AsQueryable();

        if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
        {
            query = query.Where(emp =>
                emp.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.FullName.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (_currentStatusFilter != "All")
        {
            bool isActive = _currentStatusFilter == "Active";
            query = query.Where(emp => emp.IsActive == isActive);
        }

        if (_currentPositionFilter.HasValue && _currentPositionFilter > 0)
        {
            query = query.Where(emp => emp.PositionId == _currentPositionFilter.Value);
        }

        if (_currentBranchFilter.HasValue && _currentBranchFilter > 0)
        {
            query = query.Where(emp => emp.BranchId == _currentBranchFilter.Value);
        }

        if (!string.IsNullOrWhiteSpace(_currentSortBy))
        {
            query = _currentSortBy.ToLower() switch
            {
                "id" => _sortDescending ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id),
                "fullname" => _sortDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName),
                "position" => _sortDescending ? query.OrderByDescending(e => e.PositionName) : query.OrderBy(e => e.PositionName),
                "positionname" => _sortDescending ? query.OrderByDescending(e => e.PositionName) : query.OrderBy(e => e.PositionName),
                "hiredate" => _sortDescending ? query.OrderByDescending(e => e.HireDate) : query.OrderBy(e => e.HireDate),
                "branchid" => _sortDescending ? query.OrderByDescending(e => e.BranchId) : query.OrderBy(e => e.BranchId),
                "phonenumber" => _sortDescending ? query.OrderByDescending(e => e.PhoneNumber) : query.OrderBy(e => e.PhoneNumber),
                "email" => _sortDescending ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                "isactive" => _sortDescending ? query.OrderByDescending(e => e.IsActive) : query.OrderBy(e => e.IsActive),
                _ => query.OrderBy(e => e.Id)
            };
        }

        _filteredEmployees = query.ToList();

        UpdateModelWithPagination();

        var currentPageEmployees = GetCurrentPageEmployees();
        OnDataLoaded?.Invoke(this, new EmployeesLoadedEventArgs(currentPageEmployees));
    }

    private void UpdateModelWithPagination()
    {
        Model.TotalItems = _filteredEmployees.Count;

        if (Model.CurrentPage < 1) Model.CurrentPage = 1;
        if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
            Model.CurrentPage = Model.TotalPages;
    }

    private List<EmployeeViewModel> GetCurrentPageEmployees()
    {
        int skip = (Model.CurrentPage - 1) * Model.PageSize;
        if (skip < 0) skip = 0;
        return _filteredEmployees.Skip(skip).Take(Model.PageSize).ToList();
    }

    public async Task SearchAsync(string searchTerm)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentSearchTerm = searchTerm ?? string.Empty;
            Model.CurrentPage = 1;

            if (!_allEmployeesCache.Any())
            {
                await LoadAllDataToCache();
            }

            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }

        //_currentSearchTerm = searchTerm ?? string.Empty;
        //Model.CurrentPage = 1;

        //await LoadAllDataToCache();

        //ApplyFiltersAndSort();

        //OnDataLoaded?.Invoke(this, new EmployeesLoadedEventArgs(GetCurrentPageEmployees()));
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

    public async Task FilterByPositionAsync(long positionId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentPositionFilter = positionId > 0 ? positionId : null;
            Model.CurrentPage = 1; 
            ApplyFiltersAndSort();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task FilterByBranchAsync(long? branchId)
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentBranchFilter = branchId;
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

    public async Task<PagedList<EmployeeManagementModel>> GoToNextPageAsync()
    {
        if (Model.CurrentPage >= Model.TotalPages)
            return new PagedList<EmployeeManagementModel>();

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage++;
            ApplyFiltersAndSort();
            return new PagedList<EmployeeManagementModel>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<PagedList<EmployeeManagementModel>> GoToPreviousPageAsync()
    {
        if (Model.CurrentPage <= 1)
            return new PagedList<EmployeeManagementModel>();

        await _semaphore.WaitAsync();
        try
        {
            Model.CurrentPage--;
            ApplyFiltersAndSort();
            return new PagedList<EmployeeManagementModel>();
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

    public async Task AddEmployeeAsync(string name, long positionId, string email,
        string phone, DateTime hireDate, DateTime resignDate, string status,
        decimal baseSalary, string salaryType)
    {
        var input = new CreateEmployeeInput
        {
            FullName = name,
            PositionId = positionId,
            Email = email,
            PhoneNumber = phone,
            HireDate = hireDate,
        };

        await _employeeManagementService.AddEmployeeAsync(input);

        await RefreshCacheAsync();
    }

    public async Task UpdateUserAsync(long id, string name, long positionId, string email,
        string phone, DateTime hireDate, DateTime resignDate, string status, decimal baseSalary,
        string salaryType)
    {
        var input = new UpdateEmployeeInput
        {
            EmployeeId = id,
            FullName = name,
            PositionId = positionId,
            Email = email,
            Phone = phone,
            HireDate = hireDate,
            ResignDate = resignDate,
            Status = status,
            BaseSalary = baseSalary,
            SalaryType = salaryType
        };

        await _employeeManagementService.UpdateEmployeeAsync(input);

        await RefreshCacheAsync();
    }

    public async Task DeleteUserAsync(long id)
    {
        await _employeeManagementService.DeleteEmployeeAsync(id);

        await RefreshCacheAsync();
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}