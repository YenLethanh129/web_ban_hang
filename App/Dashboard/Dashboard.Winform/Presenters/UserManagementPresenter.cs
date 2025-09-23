using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Specification;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.RBACModels;
using System.ComponentModel;
using System.Data;

namespace Dashboard.Winform.Presenters
{
    public interface IUserManagementPresenter : IManagementPresenter<UserManagementModel>
    {
        Task LoadDataAsync(long? roleId = null, string? status = null,
            string? searchTerm = null, int? pageSize = 10, int? page = 1, bool forceRefresh = false);
        Task AddUserAsync(UserDetailViewModel user);
        Task UpdateUserAsync(UserDetailViewModel user);
        Task DeleteUserAsync(long id);
        Task SearchAsync(string searchTerm);
        Task FilterByRoleAsync(long roleId);
        Task FilterByStatusAsync(string status);
        Task<PagedList<UserManagementModel>> GoToNextPageAsync();
        Task<PagedList<UserManagementModel>> GoToPreviousPageAsync();
        Task ChangePageSizeAsync(int pageSize);
        Task RefreshCacheAsync();
        Task SortByAsync(string sortBy);

        // Role assignment methods
        Task<RoleViewModel> GetUserRolesAsync(long userId);
        Task<List<RoleViewModel>> GetAvailableRolesAsync(long userId);
        Task AssignRoleToUserAsync(long userId, long roleId);
        Task RemoveRoleFromUserAsync(long userId, long roleId);

        // Employee methods
        Task<List<EmployeeSimpleViewModel>> GetEmployeesAsync();
    }

    public class UserManagementPresenter : IUserManagementPresenter
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly IEmployeeManagementService _employeeManagementService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementModel Model { get; }
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isLoading = false;

        // Cache properties
        private List<UserViewModel> _allUsersCache = [];
        private List<UserViewModel> _filteredUsers = [];
        private string _currentSearchTerm = string.Empty;
        private string _currentStatusFilter = "Tất cả";
        private long? _currentRoleFilter = null;
        private string _currentSortBy = "Id"; // Default sort by Id
        private bool _sortDescending = false; // Default sort order
        //private bool _emplo

        IManagableModel IManagementPresenter<UserManagementModel>.Model
        {
            get => Model;
            set => throw new NotImplementedException();
        }

        public event EventHandler? OnDataLoaded;

        public UserManagementPresenter(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserManagementService userManagementService,
            IRoleManagementService roleManagementService,
            IEmployeeManagementService employeeManagementService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
            _roleManagementService = roleManagementService;
            _employeeManagementService = employeeManagementService;

            Model = new UserManagementModel();
        }

        public async Task LoadDataAsync(
            long? roleId = null,
            string? status = null,
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

                if (!_allUsersCache.Any() || forceRefresh)
                {
                    await LoadAllDataToCache();
                }

                // Load roles first if not loaded
                if (!Model.Roles.Any() || forceRefresh)
                {
                    await LoadRoles();
                }

                _currentSearchTerm = searchTerm ?? string.Empty;
                _currentStatusFilter = status ?? "Tất cả";
                _currentRoleFilter = roleId;

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
            await LoadDataAsync(null, null, null);
        }

        private async Task LoadAllDataToCache()
        {
            var userDtos = await _userManagementService.GetUsersAsync(new GetUsersInput
            {
                PageSize = int.MaxValue,
                PageNumber = 1
            });
            var mapped = _mapper.Map<List<UserViewModel>>(userDtos);
            _allUsersCache = mapped ?? new List<UserViewModel>();
        }

        private async Task LoadRoles()
        {
            try
            {
                var roleDtos = await _roleManagementService.GetAllRolesAsync();
                var availableRoles = roleDtos.Where(r => r.Name != "GUEST" && r.Name != "CUSTOMER").ToList();

                var roles = availableRoles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.LastModified,
                    PermissionCount = r.Permissions?.Count ?? 0,
                    Permissions = (r.Permissions ?? [])
                        .Select(p => new PermissionViewModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Resource = p.Resource,
                            Action = p.Action,
                            CreatedAt = p.CreatedAt
                        }).ToList()
                }).ToList();

                Model.Roles.Clear();
                foreach (var role in roles)
                {
                    Model.Roles.Add(role);
                }

                Console.WriteLine($"Loaded {roles.Count} roles");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading roles: {ex.Message}");
            }
        }

        private void ApplyFiltersAndSort()
        {
            var query = _allUsersCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentSearchTerm))
            {
                query = query.Where(user =>
                    user.Id.ToString().Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    user.Username.Contains(_currentSearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (_currentStatusFilter != "Tất cả")
            {
                bool isActive = _currentStatusFilter == "Hoạt động";
                query = query.Where(user => user.IsActive == isActive);
            }

            if (_currentRoleFilter.HasValue && _currentRoleFilter > 0)
            {
                query = query.Where(user => user.RoleId == _currentRoleFilter.Value);
            }

            query = ApplySorting(query);

            _filteredUsers = query.ToList();

            UpdateModelWithPagination();

            var currentPageUsers = GetCurrentPageUsers();

            // Trigger event với data như EmployeeManagement
            OnDataLoaded?.Invoke(this, new UsersLoadedEventArgs(currentPageUsers));
        }

        private IQueryable<UserViewModel> ApplySorting(IQueryable<UserViewModel> query)
        {
            // Sort by the current sort column
            query = _currentSortBy switch
            {
                "Username" => _sortDescending ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                "RoleName" => _sortDescending ? query.OrderByDescending(u => u.RoleName) : query.OrderBy(u => u.RoleName),
                "EmployeeName" => _sortDescending ? query.OrderByDescending(u => u.EmployeeName) : query.OrderBy(u => u.EmployeeName),
                "CreatedAt" => _sortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                "IsActive" => _sortDescending ? query.OrderByDescending(u => u.IsActive) : query.OrderBy(u => u.IsActive),
                _ => _sortDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
            };
            return query;
        }

        private void UpdateModelWithPagination()
        {
            Model.TotalItems = _filteredUsers.Count;

            if (Model.CurrentPage < 1) Model.CurrentPage = 1;
            if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
                Model.CurrentPage = Model.TotalPages;
        }

        private List<UserViewModel> GetCurrentPageUsers()
        {
            int skip = (Model.CurrentPage - 1) * Model.PageSize;
            return _filteredUsers.Skip(skip).Take(Model.PageSize).ToList();
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

        public async Task FilterByRoleAsync(long roleId)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentRoleFilter = roleId > 0 ? roleId : null;
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
                _currentStatusFilter = status ?? "Tất cả";
                Model.CurrentPage = 1;
                ApplyFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<PagedList<UserManagementModel>> GoToNextPageAsync()
        {
            if (Model.CurrentPage >= Model.TotalPages)
                return new PagedList<UserManagementModel>();

            await _semaphore.WaitAsync();
            try
            {
                Model.CurrentPage++;
                ApplyFiltersAndSort();
                return new PagedList<UserManagementModel>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<PagedList<UserManagementModel>> GoToPreviousPageAsync()
        {
            if (Model.CurrentPage <= 1)
                return new PagedList<UserManagementModel>();

            await _semaphore.WaitAsync();
            try
            {
                Model.CurrentPage--;
                ApplyFiltersAndSort();
                return new PagedList<UserManagementModel>();
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
            await Task.Delay(200);
            await LoadDataAsync(forceRefresh: true);
        }

        public async Task AddUserAsync(UserDetailViewModel user)
        {
            var existingUser = await _userManagementService.GetUserByIdAsync(user.Id);
            if (existingUser != null)
                throw new InvalidOperationException($"Người dùng với ID: {user.Id} đã tồn tại.");
            var newUser = _mapper.Map<CreateUserInput>(user);
            await _userManagementService.CreateUserAsync(newUser);
            await RefreshCacheAsync();
        }

        public async Task UpdateUserAsync(UserDetailViewModel user)
        {
            var existingUser = await _userManagementService.GetUserByIdAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {user.Id}");

            existingUser.IsActive = user.IsActive;
            existingUser.EmployeeId = user.EmployeeId;
            existingUser.RoleId = user.RoleId;
            existingUser.Username = user.Username;
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = user.Password;
            }
            
            var input = _mapper.Map<UpdateUserInput>(existingUser);

            await _userManagementService.UpdateUserAsync(input);

            await RefreshCacheAsync();
        }

        public async Task DeleteUserAsync(long id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id) ?? throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {id}");
            user.IsActive = false;
            var input = _mapper.Map<UpdateUserInput>(user);
            var uster = await _userManagementService.UpdateUserAsync(input);

            await RefreshCacheAsync();
        }

        public async Task<RoleViewModel> GetUserRolesAsync(long userId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId);

            if (user == null)
                return new RoleViewModel();

            var role = await _roleManagementService.GetRoleByIdAsync(user.RoleId);
            if (role == null)
                return new RoleViewModel();

            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.LastModified,
                PermissionCount = role.Permissions?.Count ?? 0,
                Permissions = (role.Permissions ?? Enumerable.Empty<PermissionDto>())
                              .Select(p => new PermissionViewModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Description = p.Description,
                                  Resource = p.Resource,
                                  Action = p.Action,
                                  CreatedAt = p.CreatedAt
                              }).ToList()
            };

        }

        public async Task<List<RoleViewModel>> GetAvailableRolesAsync(long userId)
        {
            var allRoles = await _roleManagementService.GetAllRolesAsync();
            var user = await _userManagementService.GetUserByIdAsync(userId);

            var filteredRoles = allRoles
                .Where(r => r.Name != "GUEST" && r.Name != "CUSTOMER");

            if (user != null)
                filteredRoles = filteredRoles.Where(r => r.Id != user.RoleId);

            return [.. filteredRoles
                .Where(r => !string.Equals(r.Name, "Customer", StringComparison.OrdinalIgnoreCase)
                         && !string.Equals(r.Name, "Guest", StringComparison.OrdinalIgnoreCase))
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.LastModified,
                    PermissionCount = r.Permissions?.Count ?? 0,
                    Permissions = [.. (r.Permissions ?? Enumerable.Empty<PermissionDto>())
                                  .Select(p => new PermissionViewModel
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Description = p.Description,
                                      Resource = p.Resource,
                                      Action = p.Action,
                                      CreatedAt = p.CreatedAt
                                  })]
                })];
        }

        public async Task AssignRoleToUserAsync(long userId, long roleId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId) 
                ?? throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {userId}");
            var role = await _roleManagementService.GetRoleByIdAsync(roleId)
                ?? throw new InvalidOperationException($"Không tìm thấy vai trò với ID: {roleId}");
            user.RoleId = roleId;
            var input = _mapper.Map<UpdateUserInput>(user);
            await _userManagementService.UpdateUserAsync(input);
            await _unitOfWork.SaveChangesAsync();

            await RefreshCacheAsync();
        }

        public async Task RemoveRoleFromUserAsync(long userId, long roleId)
        {
            var user = await _userManagementService.GetUserByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException($"Không tìm thấy người dùng với ID: {userId}");

            var defaultRole = await _unitOfWork.Repository<Role>().GetWithSpecAsync(
                new Specification<Role>(r => r.Name == "CUSTOMER"));

            if (defaultRole != null)
            {
                user.RoleId = defaultRole.Id;
                var updateInput = _mapper.Map<UpdateUserInput>(user);

                await _userManagementService.UpdateUserAsync(updateInput);
            }
            else
            {
                throw new InvalidOperationException("Không tìm thấy vai trò mặc định CUSTOMER");
            }

            await RefreshCacheAsync();
        }

        public async Task<List<EmployeeSimpleViewModel>> GetEmployeesAsync()
        {
            var input = new GetEmployeesInput
            {
                PageSize = int.MaxValue,
                PageNumber = 1
            };
            var employees = await _employeeManagementService.GetEmployeesAsync(input);
            return _mapper.Map<List<EmployeeSimpleViewModel>>(employees);
        }

        public async Task SortByAsync(string sortBy)
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
                    _currentSortBy = sortBy;
                    _sortDescending = false;
                }

                Model.CurrentPage = 1;
                ApplyFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
}