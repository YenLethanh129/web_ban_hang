using AutoMapper;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.RBACModels;

namespace Dashboard.Winform.Presenters
{
    public interface IRolePermissionManagementPresenter : IManagementPresenter<RolePermissionManagementModel>
    {
        Task LoadRolesAsync(int page = 1, int pageSize = 10, bool forceRefresh = false);
        Task LoadPermissionsAsync(int page = 1, int pageSize = 10, bool forceRefresh = false);
        Task SearchRolesAsync(string searchTerm);
        Task SearchPermissionsAsync(string searchTerm);
        Task SortRolesBy(string sortBy);
        Task SortPermissionsBy(string sortBy);
        Task<RoleViewModel?> GetRoleByIdAsync(long id);
        Task<PermissionViewModel?> GetPermissionByIdAsync(long id);
        Task<RoleDetailViewModel> CreateRoleDetailAsync(long? roleId = null);
        Task<PermissionDetailViewModel> CreatePermissionDetailAsync(long? permissionId = null);
        Task SaveRoleAsync(RoleDetailViewModel roleDetail);
        Task SavePermissionAsync(PermissionDetailViewModel permissionDetail);
        Task DeleteRoleAsync(long id);
        Task DeletePermissionAsync(long id);
        Task AssignPermissionsToRoleAsync(long roleId, List<long> permissionIds);
        Task<List<PermissionViewModel>> GetRolePermissionsAsync(long roleId);
        Task GoToNextPageAsync();
        Task GoToPreviousPageAsync();
        Task ChangePageSizeAsync(int pageSize);

        event EventHandler<RolesLoadedEventArgs>? OnRolesLoaded;
        event EventHandler<PermissionsLoadedEventArgs>? OnPermissionsLoaded;
    }

    public class RolePermissionManagementPresenter : IRolePermissionManagementPresenter
    {
        private readonly IRoleManagementService _roleService;
        private readonly IPermissionManagementService _permissionService;
        private readonly IMapper _mapper;

        public RolePermissionManagementModel Model { get; }
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isLoading = false;

        // Cache properties
        private List<RoleViewModel> _allRolesCache = [];
        private List<PermissionViewModel> _allPermissionsCache = [];
        private List<RoleViewModel> _filteredRoles = [];
        private List<PermissionViewModel> _filteredPermissions = [];
        private string _currentRoleSearchTerm = string.Empty;
        private string _currentPermissionSearchTerm = string.Empty;
        private string _currentRoleSortBy = string.Empty;
        private string _currentPermissionSortBy = string.Empty;
        private bool _roleSortDescending = false;
        private bool _permissionSortDescending = false;

        IManagableModel IManagementPresenter<RolePermissionManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

        public event EventHandler<RolesLoadedEventArgs>? OnRolesLoaded;
        public event EventHandler<PermissionsLoadedEventArgs>? OnPermissionsLoaded;
        public event EventHandler? OnDataLoaded;

        public RolePermissionManagementPresenter(
             IRoleManagementService roleService,
             IPermissionManagementService permissionService,
             IMapper mapper
            )
        {
            _mapper = mapper;
            _roleService = roleService;
            _permissionService = permissionService;

            Model = new RolePermissionManagementModel();
        }

        public async Task LoadDataAsync()
        {
            await LoadRolesAsync();
            await LoadPermissionsAsync();
            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadRolesAsync(int page = 1, int pageSize = 10, bool forceRefresh = false)
        {
            if (_isLoading && !forceRefresh) return;

            await _semaphore.WaitAsync();
            try
            {
                _isLoading = true;

                if (_allRolesCache.Count == 0 || forceRefresh)
                {
                    await LoadAllRolesToCache();
                }

                Model.CurrentPage = page;
                Model.PageSize = pageSize;

                ApplyRoleFiltersAndSort();
            }
            finally
            {
                _isLoading = false;
                _semaphore.Release();
            }
        }

        public async Task LoadPermissionsAsync(int page = 1, int pageSize = 10, bool forceRefresh = false)
        {
            if (_isLoading && !forceRefresh) return;

            await _semaphore.WaitAsync();
            try
            {
                _isLoading = true;

                if (_allPermissionsCache.Count == 0 || forceRefresh)
                {
                    await LoadAllPermissionsToCache();
                }

                ApplyPermissionFiltersAndSort();
            }
            finally
            {
                _isLoading = false;
                _semaphore.Release();
            }
        }

        private async Task LoadAllRolesToCache()
        {
            var roles = await _roleService.GetAllRolesAsync();
            _allRolesCache = _mapper.Map<List<RoleViewModel>>(roles);
            _filteredRoles = [.. _allRolesCache];
            UpdateModelWithRolePagination();
        }

        private async Task LoadAllPermissionsToCache()
        {
            var permissions = await _permissionService.GetAllPermission();
            _allPermissionsCache = _mapper.Map<List<PermissionViewModel>>(permissions);
            _filteredPermissions = [.. _allPermissionsCache];
        }

        private void ApplyRoleFiltersAndSort()
        {
            var query = _allRolesCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentRoleSearchTerm))
            {
                query = query.Where(role =>
                    role.Id.ToString().Contains(_currentRoleSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    role.Name.Contains(_currentRoleSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (role.Description != null && role.Description.Contains(_currentRoleSearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(_currentRoleSortBy))
            {
                query = _currentRoleSortBy.ToLower() switch
                {
                    "id" => _roleSortDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                    "name" => _roleSortDescending ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                    "createdat" => _roleSortDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                    "permissioncount" => _roleSortDescending ? query.OrderByDescending(r => r.PermissionCount) : query.OrderBy(r => r.PermissionCount),
                    _ => query.OrderBy(r => r.Id)
                };
            }

            _filteredRoles = query.ToList();
            UpdateModelWithRolePagination();

            var currentPageRoles = GetCurrentPageRoles();
            OnRolesLoaded?.Invoke(this, new RolesLoadedEventArgs(currentPageRoles));
        }

        private void ApplyPermissionFiltersAndSort()
        {
            var query = _allPermissionsCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentPermissionSearchTerm))
            {
                query = query.Where(perm =>
                    perm.Id.ToString().Contains(_currentPermissionSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    perm.Name.Contains(_currentPermissionSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    perm.Resource.Contains(_currentPermissionSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    perm.Action.Contains(_currentPermissionSearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(perm.Description) && perm.Description.Contains(_currentPermissionSearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(_currentPermissionSortBy))
            {
                query = _currentPermissionSortBy.ToLower() switch
                {
                    "id" => _permissionSortDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                    "name" => _permissionSortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "resource" => _permissionSortDescending ? query.OrderByDescending(p => p.Resource) : query.OrderBy(p => p.Resource),
                    "action" => _permissionSortDescending ? query.OrderByDescending(p => p.Action) : query.OrderBy(p => p.Action),
                    "createdat" => _permissionSortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderBy(p => p.Id)
                };
            }

            _filteredPermissions = query.ToList();

            var currentPagePermissions = GetCurrentPagePermissions();
            OnPermissionsLoaded?.Invoke(this, new PermissionsLoadedEventArgs(currentPagePermissions));
        }

        private void UpdateModelWithRolePagination()
        {
            Model.TotalItems = _filteredRoles.Count;
            if (Model.CurrentPage < 1) Model.CurrentPage = 1;
            if (Model.CurrentPage > Model.TotalPages && Model.TotalPages > 0)
                Model.CurrentPage = Model.TotalPages;
        }

        private List<RoleViewModel> GetCurrentPageRoles()
        {
            int skip = (Model.CurrentPage - 1) * Model.PageSize;
            return _filteredRoles.Skip(skip).Take(Model.PageSize).ToList();
        }

        private List<PermissionViewModel> GetCurrentPagePermissions()
        {
            int skip = (Model.CurrentPage - 1) * Model.PageSize;
            return _filteredPermissions.Skip(skip).Take(Model.PageSize).ToList();
        }

        public async Task SearchRolesAsync(string searchTerm)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentRoleSearchTerm = searchTerm ?? string.Empty;
                Model.SearchText = _currentRoleSearchTerm;
                Model.CurrentPage = 1;
                ApplyRoleFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SearchPermissionsAsync(string searchTerm)
        {
            await _semaphore.WaitAsync();
            try
            {
                _currentPermissionSearchTerm = searchTerm ?? string.Empty;
                Model.CurrentPage = 1;
                ApplyPermissionFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SortRolesBy(string sortBy)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_currentRoleSortBy == sortBy)
                {
                    _roleSortDescending = !_roleSortDescending;
                }
                else
                {
                    _currentRoleSortBy = sortBy ?? string.Empty;
                    _roleSortDescending = false;
                }

                ApplyRoleFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SortPermissionsBy(string sortBy)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_currentPermissionSortBy == sortBy)
                {
                    _permissionSortDescending = !_permissionSortDescending;
                }
                else
                {
                    _currentPermissionSortBy = sortBy ?? string.Empty;
                    _permissionSortDescending = false;
                }

                ApplyPermissionFiltersAndSort();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<RoleViewModel?> GetRoleByIdAsync(long id)
        {
            // TODO: Replace with real service call
            await Task.Delay(50);
            return _allRolesCache.FirstOrDefault(r => r.Id == id);
        }

        public async Task<PermissionViewModel?> GetPermissionByIdAsync(long id)
        {
            // TODO: Replace with real service call
            await Task.Delay(50);
            return _allPermissionsCache.FirstOrDefault(p => p.Id == id);
        }

        public async Task<RoleDetailViewModel> CreateRoleDetailAsync(long? roleId = null)
        {
            // TODO: Replace with real service call
            await Task.Delay(50);

            var roleDetail = new RoleDetailViewModel
            {
                AllPermissions = _allPermissionsCache.ToList()
            };

            if (roleId.HasValue)
            {
                var role = await GetRoleByIdAsync(roleId.Value);
                if (role != null)
                {
                    roleDetail.Id = role.Id;
                    roleDetail.Name = role.Name;
                    roleDetail.Description = role.Description;
                    // TODO: Load assigned permissions from service
                    roleDetail.AssignedPermissionIds = [1, 2, 3]; // Mock data
                }
            }

            return roleDetail;
        }

        public async Task<PermissionDetailViewModel> CreatePermissionDetailAsync(long? permissionId = null)
        {
            // TODO: Replace with real service call
            await Task.Delay(50);

            var permissionDetail = new PermissionDetailViewModel();

            if (permissionId.HasValue)
            {
                var permission = await GetPermissionByIdAsync(permissionId.Value);
                if (permission != null)
                {
                    permissionDetail.Id = permission.Id;
                    permissionDetail.Name = permission.Name;
                    permissionDetail.Description = permission.Description;
                    permissionDetail.Resource = permission.Resource;
                    permissionDetail.Action = permission.Action;
                }
            }

            return permissionDetail;
        }

        public async Task SaveRoleAsync(RoleDetailViewModel roleDetail)
        {
            // TODO: Replace with real service call
            await Task.Delay(100);

            if (roleDetail.Id == 0)
            {
                // Create new role
                var newId = _allRolesCache.Max(r => r.Id) + 1;
                var newRole = new RoleViewModel
                {
                    Id = newId,
                    Name = roleDetail.Name,
                    Description = roleDetail.Description,
                    CreatedAt = DateTime.Now,
                    PermissionCount = roleDetail.AssignedPermissionIds.Count
                };
                _allRolesCache.Add(newRole);
            }
            else
            {
                // Update existing role
                var existingRole = _allRolesCache.FirstOrDefault(r => r.Id == roleDetail.Id);
                if (existingRole != null)
                {
                    existingRole.Name = roleDetail.Name;
                    existingRole.Description = roleDetail.Description;
                    existingRole.UpdatedAt = DateTime.Now;
                    existingRole.PermissionCount = roleDetail.AssignedPermissionIds.Count;
                }
            }

            ApplyRoleFiltersAndSort();
        }

        public async Task SavePermissionAsync(PermissionDetailViewModel permissionDetail)
        {
            // TODO: Replace with real service call
            await Task.Delay(100);

            if (permissionDetail.Id == 0)
            {
                // Create new permission
                var newId = _allPermissionsCache.Max(p => p.Id) + 1;
                var newPermission = new PermissionViewModel
                {
                    Id = newId,
                    Name = permissionDetail.Name,
                    Description = permissionDetail.Description,
                    Resource = permissionDetail.Resource,
                    Action = permissionDetail.Action,
                    CreatedAt = DateTime.Now
                };
                _allPermissionsCache.Add(newPermission);
            }
            else
            {
                // Update existing permission
                var existingPermission = _allPermissionsCache.FirstOrDefault(p => p.Id == permissionDetail.Id);
                if (existingPermission != null)
                {
                    existingPermission.Name = permissionDetail.Name;
                    existingPermission.Description = permissionDetail.Description;
                    existingPermission.Resource = permissionDetail.Resource;
                    existingPermission.Action = permissionDetail.Action;
                    existingPermission.UpdatedAt = DateTime.Now;
                }
            }

            ApplyPermissionFiltersAndSort();
        }

        public async Task DeleteRoleAsync(long id)
        {
            // TODO: Replace with real service call
            await Task.Delay(100);

            var roleToRemove = _allRolesCache.FirstOrDefault(r => r.Id == id);
            if (roleToRemove != null)
            {
                _allRolesCache.Remove(roleToRemove);
                ApplyRoleFiltersAndSort();
            }
        }

        public async Task DeletePermissionAsync(long id)
        {
            // TODO: Replace with real service call
            await Task.Delay(100);

            var permissionToRemove = _allPermissionsCache.FirstOrDefault(p => p.Id == id);
            if (permissionToRemove != null)
            {
                _allPermissionsCache.Remove(permissionToRemove);
                ApplyPermissionFiltersAndSort();
            }
        }

        public async Task AssignPermissionsToRoleAsync(long roleId, List<long> permissionIds)
        {
            // TODO: Replace with real service call
            await Task.Delay(100);

            var role = _allRolesCache.FirstOrDefault(r => r.Id == roleId);
            if (role != null)
            {
                role.PermissionCount = permissionIds.Count;
                role.UpdatedAt = DateTime.Now;
            }
        }

        public async Task<List<PermissionViewModel>> GetRolePermissionsAsync(long roleId)
        {
            // TODO: Replace with real service call
            await Task.Delay(50);

            // Mock data - return some permissions for the role
            return _allPermissionsCache.Take(3).ToList();
        }

        public async Task GoToNextPageAsync()
        {
            if (Model.CurrentPage >= Model.TotalPages) return;

            await _semaphore.WaitAsync();
            try
            {
                Model.CurrentPage++;
                ApplyRoleFiltersAndSort();
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
                ApplyRoleFiltersAndSort();
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
                ApplyRoleFiltersAndSort();
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