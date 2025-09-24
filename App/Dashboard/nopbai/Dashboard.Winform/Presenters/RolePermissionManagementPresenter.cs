using AutoMapper;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.RBACModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters
{
    public interface IRolePermissionManagementPresenter : IManagementPresenter<RolePermissionManagementModel>
    {
        Task LoadRolesAsync(int page = 1, int pageSize = 100, bool forceRefresh = false);
        Task LoadPermissionsAsync(int page = 1, int pageSize = 100, bool forceRefresh = false);
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

    public class RolePermissionManagementPresenter : IRolePermissionManagementPresenter, IDisposable
    {
        private readonly IRoleManagementService _roleService;
        private readonly IPermissionManagementService _permissionService;
        private readonly IMapper _mapper;

        public RolePermissionManagementModel Model { get; }
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isLoading = false;

        // Cache properties
        private List<RoleViewModel> _allRolesCache = new List<RoleViewModel>();
        private List<PermissionViewModel> _allPermissionsCache = new List<PermissionViewModel>();
        private List<RoleViewModel> _filteredRoles = new List<RoleViewModel>();
        private List<PermissionViewModel> _filteredPermissions = new List<PermissionViewModel>();
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
            _permission_service_check(permissionService);
            _permissionService = permissionService;

            Model = new RolePermissionManagementModel();
        }

        // small guard because some DI setups might pass null
        private void _permission_service_check(IPermissionManagementService? svc)
        {
            if (svc == null) throw new ArgumentNullException(nameof(svc));
        }

        public async Task LoadDataAsync()
        {
            await LoadRolesAsync();
            await LoadPermissionsAsync();
            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadRolesAsync(int page = 1, int pageSize = 100, bool forceRefresh = false)
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

        public async Task LoadPermissionsAsync(int page = 1, int pageSize = 100, bool forceRefresh = false)
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
            var rolesFromService = await _roleService.GetAllRolesAsync();

            // manual robust mapping to avoid AutoMapper type mismatch issues
            _allRolesCache = rolesFromService.Select(dto => MapRoleDtoToViewModel(dto)).ToList();
            _filteredRoles = new List<RoleViewModel>(_allRolesCache);
            UpdateModelWithRolePagination();
        }

        private async Task LoadAllPermissionsToCache()
        {
            var permissionsFromService = await _permissionService.GetAllPermission();
            _allPermissionsCache = permissionsFromService.Select(MapPermissionDtoToViewModel).ToList();
            _filteredPermissions = new List<PermissionViewModel>(_allPermissionsCache);
        }

        private RoleViewModel MapRoleDtoToViewModel(RoleDto dto)
        {
            var vm = new RoleViewModel
            {
                Id = dto.Id,
                Name = dto.Name ?? string.Empty,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.LastModified,
                PermissionCount = dto.Permissions?.Count ?? 0,
                Permissions = (dto.Permissions ?? new List<PermissionDto>()).Select(MapPermissionDtoToViewModel).ToList()
            };
            return vm;
        }

        private PermissionViewModel MapPermissionDtoToViewModel(PermissionDto dto)
        {
            return new PermissionViewModel
            {
                Id = dto.Id,
                Name = dto.Name ?? string.Empty,
                Description = dto.Description,
                Resource = dto.Resource ?? string.Empty,
                Action = dto.Action ?? string.Empty,
                CreatedAt = dto.CreatedAt
            };
        }

        private void ApplyRoleFiltersAndSort()
        {
            var query = _allRolesCache.AsQueryable();

            if (!string.IsNullOrWhiteSpace(_currentRoleSearchTerm))
            {
                var term = _currentRoleSearchTerm;
                query = query.Where(role =>
                    role.Id.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(role.Name) && role.Name.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(role.Description) && role.Description.Contains(term, StringComparison.OrdinalIgnoreCase)));
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
                var term = _currentPermissionSearchTerm;
                query = query.Where(perm =>
                    perm.Id.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(perm.Name) && perm.Name.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(perm.Resource) && perm.Resource.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(perm.Action) && perm.Action.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(perm.Description) && perm.Description.Contains(term, StringComparison.OrdinalIgnoreCase)));
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
            var dto = await _roleService.GetRoleByIdAsync(id);
            if (dto == null) return null;
            return MapRoleDtoToViewModel(dto);
        }

        public async Task<PermissionViewModel?> GetPermissionByIdAsync(long id)
        {
            var perms = await _permissionService.GetAllPermission();
            var dto = perms.FirstOrDefault(p => p.Id == id);
            if (dto == null) return null;
            return MapPermissionDtoToViewModel(dto);
        }

        public async Task<RoleDetailViewModel> CreateRoleDetailAsync(long? roleId = null)
        {
            var roleDetail = new RoleDetailViewModel
            {
                AllPermissions = _allPermissionsCache.ToList()
            };

            if (roleId.HasValue)
            {
                var dto = await _roleService.GetRoleByIdAsync(roleId.Value);
                if (dto != null)
                {
                    roleDetail.Id = dto.Id;
                    roleDetail.Name = dto.Name ?? string.Empty;
                    roleDetail.Description = dto.Description;

                    var assignedIds = await _permissionService.GetPermissionsByRoleIdAsync(roleId.Value);
                    roleDetail.AssignedPermissionIds = assignedIds ?? new List<long>();
                }
                else
                {
                    roleDetail.AssignedPermissionIds = new List<long>();
                }
            }

            return roleDetail;
        }

        public async Task<PermissionDetailViewModel> CreatePermissionDetailAsync(long? permissionId = null)
        {
            var permissionDetail = new PermissionDetailViewModel();

            if (permissionId.HasValue)
            {
                var dto = (await _permissionService.GetAllPermission()).FirstOrDefault(p => p.Id == permissionId.Value);
                if (dto != null)
                {
                    permissionDetail.Id = dto.Id;
                    permissionDetail.Name = dto.Name;
                    permissionDetail.Description = dto.Description;
                    permissionDetail.Resource = dto.Resource;
                    permissionDetail.Action = dto.Action;
                }
            }

            return permissionDetail;
        }

        public async Task SaveRoleAsync(RoleDetailViewModel roleDetail)
        {
            if (roleDetail == null) return;

            if (roleDetail.Id == 0)
            {
                await _roleService.CreateRoleAsync(roleDetail.Name, roleDetail.Description ?? string.Empty);
            }
            else
            {
                await _roleService.UpdateRoleAsync(roleDetail.Id, roleDetail.Name, roleDetail.Description);
            }

            try
            {
                await _roleService.UpdateRolePermission(roleDetail.Id, roleDetail.AssignedPermissionIds ?? new List<long>());
            }
            catch
            {
            }

            await LoadRolesAsync(forceRefresh: true);
            await LoadPermissionsAsync(forceRefresh: true);
        }

        public async Task SavePermissionAsync(PermissionDetailViewModel permissionDetail)
        {
            if (permissionDetail == null) return;

            await LoadPermissionsAsync(forceRefresh: true);
        }

        public async Task DeleteRoleAsync(long id)
        {
            await _roleService.DeleteRoleAsync(id);
            await LoadRolesAsync(forceRefresh: true);
        }

        public async Task DeletePermissionAsync(long id)
        {
            await LoadPermissionsAsync(forceRefresh: true);
        }

        public async Task AssignPermissionsToRoleAsync(long roleId, List<long> permissionIds)
        {
            await _roleService.UpdateRolePermission(roleId, permissionIds ?? new List<long>());

            var role = _allRolesCache.FirstOrDefault(r => r.Id == roleId);
            if (role != null)
            {
                role.PermissionCount = permissionIds?.Count ?? 0;
                role.UpdatedAt = DateTime.Now;
            }

            ApplyRoleFiltersAndSort();
        }

        public async Task<List<PermissionViewModel>> GetRolePermissionsAsync(long roleId)
        {
            var ids = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            var perms = (await _permissionService.GetAllPermission())
                .Where(p => ids.Contains(p.Id))
                .Select(MapPermissionDtoToViewModel)
                .ToList();
            return perms;
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