using AutoMapper;
using Dashboard.BussinessLogic.Dtos.AuthDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(long userId, string permission);
    Task<bool> IsInRoleAsync(long userId, string roleName);
    Task<List<string>> GetUserPermissionsAsync(long userId);
    Task<List<RoleDto>> GetRolesAsync();
    Task<List<PermissionDto>> GetPermissionsAsync();
    Task<bool> AssignRoleToUserAsync(long userId, long roleId);
    Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
    Task<bool> CanAccessResourceAsync(long userId, string resource, string action);
    Task<UserDto?> GetUserWithPermissionsAsync(long userId);
}

public class AuthorizationService : BaseTransactionalService, IAuthorizationService
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorizationService> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthorizationService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<AuthorizationService> logger,
        IAuthenticationService authenticationService) : base(unitOfWork)
    {
        _mapper = mapper;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public async Task<bool> HasPermissionAsync(long userId, string permission)
    {
        try
        {
            return await _authenticationService.HasPermissionAsync(userId, permission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission {Permission} for user: {UserId}", permission, userId);
            return false;
        }
    }

    public async Task<bool> IsInRoleAsync(long userId, string roleName)
    {
        try
        {
            var session = await _authenticationService.GetCurrentSessionAsync(userId);
            return session?.RoleName == roleName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking role {Role} for user: {UserId}", roleName, userId);
            return false;
        }
    }

    public async Task<List<string>> GetUserPermissionsAsync(long userId)
    {
        try
        {
            return await _authenticationService.GetUserPermissionsAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for user: {UserId}", userId);
            return new List<string>();
        }
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        try
        {
            // Only return WinForms relevant roles
            var roleSpec = new Specification<Role>(r => 
                r.Name == Roles.ADMIN || r.Name == Roles.MANAGER || r.Name == Roles.EMPLOYEE);
            roleSpec.IncludeStrings.Add("RolePermissions");
            roleSpec.IncludeStrings.Add("RolePermissions.Permission");
            roleSpec.IncludeStrings.Add("Users");

            var roles = await _unitOfWork.Repository<Role>().GetAllWithSpecAsync(roleSpec, true);

            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                UserCount = r.Users.Count,
                CreatedAt = r.CreatedAt,
                Permissions = r.RolePermissions.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    Resource = rp.Permission.Resource,
                    Action = rp.Permission.Action,
                    CreatedAt = rp.Permission.CreatedAt
                }).ToList()
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles");
            return new List<RoleDto>();
        }
    }

    public async Task<List<PermissionDto>> GetPermissionsAsync()
    {
        try
        {
            var permissions = await _unitOfWork.Repository<Permission>().GetAllAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions");
            return new List<PermissionDto>();
        }
    }

    public async Task<bool> AssignRoleToUserAsync(long userId, long roleId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            var roleSpec = new Specification<Role>(r => r.Id == roleId);
            var role = await _unitOfWork.Repository<Role>().GetWithSpecAsync(roleSpec);

            if (role == null || !IsValidWinFormsRole(role.Name))
            {
                _logger.LogWarning("Invalid role for assignment: {RoleId}", roleId);
                return false;
            }

            user.RoleId = roleId;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Role {RoleId} assigned to user {UserId}", roleId, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}", roleId, userId);
            return false;
        }
    }

    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
    {
        try
        {
            if (!IsValidWinFormsRole(roleName))
            {
                return new List<UserDto>();
            }

            var userSpec = new Specification<User>(u => u.Role.Name == roleName && u.IsActive);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(userSpec, true);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.Fullname ?? "",
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                IsActive = u.IsActive,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                EmployeeId = u.EmployeeId,
                EmployeeName = u.Employee?.FullName,
                Position = u.Employee?.Position,
                BranchId = u.Employee?.BranchId,
                BranchName = u.Employee?.Branch?.Name,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users by role: {Role}", roleName);
            return new List<UserDto>();
        }
    }

    public async Task<bool> CanAccessResourceAsync(long userId, string resource, string action)
    {
        try
        {
            var session = await _authenticationService.GetCurrentSessionAsync(userId);
            if (session == null) return false;

            // Check specific permission for resource + action
            var permissionName = $"{resource}_{action}";
            if (session.Permissions.Contains(permissionName))
                return true;

            // Check general permissions based on role
            return session.RoleName switch
            {
                Roles.ADMIN => true, // Admin has access to everything
                Roles.MANAGER => IsManagerAllowed(resource, action),
                Roles.EMPLOYEE => IsEmployeeAllowed(resource, action),
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking resource access for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<UserDto?> GetUserWithPermissionsAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");
            userSpec.IncludeStrings.Add("Role.RolePermissions");
            userSpec.IncludeStrings.Add("Role.RolePermissions.Permission");

            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null) return null;

            var permissions = user.Role.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.Fullname ?? "",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                RoleName = user.Role.Name,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.Employee?.FullName,
                Position = user.Employee?.Position,
                BranchId = user.Employee?.BranchId,
                BranchName = user.Employee?.Branch?.Name,
                Permissions = permissions,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with permissions: {UserId}", userId);
            return null;
        }
    }

    private static bool IsValidWinFormsRole(string roleName)
    {
        return roleName == Roles.ADMIN || roleName == Roles.MANAGER || roleName == Roles.EMPLOYEE;
    }

    private static bool IsManagerAllowed(string resource, string action)
    {
        // Managers can access most resources except system management
        return resource != Resources.SYSTEM;
    }

    private static bool IsEmployeeAllowed(string resource, string action)
    {
        // Employees have limited access
        var allowedResources = new[] { Resources.INVENTORY, Resources.ORDERS, Resources.CUSTOMERS };
        var restrictedActions = new[] { Actions.DELETE, Actions.MANAGE };

        return allowedResources.Contains(resource) && !restrictedActions.Contains(action);
    }

    public async Task<bool> AssignPermissionToRoleAsync(long roleId, long permissionId)
    {
        var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        var existingRolePermission = await _unitOfWork.Repository<RolePermission>().GetWithSpecAsync(spec, true);
        if (existingRolePermission != null) return false;

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        await _unitOfWork.Repository<RolePermission>().AddAsync(rolePermission);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(long roleId, long permissionId)
    {
     var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        var rolePermission = await _unitOfWork.Repository<RolePermission>().GetWithSpecAsync(spec);
        if (rolePermission == null) return false;

        _unitOfWork.Repository<RolePermission>().Remove(rolePermission);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}