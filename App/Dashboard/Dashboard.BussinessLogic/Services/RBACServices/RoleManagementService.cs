using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Services.RBACServices;
public interface IRoleManagementService
{
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(long roleId);
    Task<bool> CreateRoleAsync(string roleName, string description);
    Task<bool> DeleteRoleAsync(long roleId);
    Task<bool> UpdateRoleAsync(long roleId, string? roleName = null, string? description = null);
    Task<bool> UpdateRolePermission(long roleId, List<long> permissionIds);
}

public class RoleManagementService : IRoleManagementService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RoleManagementService(IUnitOfWork unitOfWork, IRoleRepository roleRepository)
    {
        _unitOfWork = unitOfWork;
        _roleRepository = roleRepository;
    }
    public async Task<bool> CreateRoleAsync(string roleName, string description)
    {
        var spec = new Specification<Role>(r => r.Name
            .Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

        var existingRole = await _roleRepository.GetWithSpecAsync(spec);
        if (existingRole != null)
        {
            return false;
        }
        var newRole = new Role
        {
            Name = roleName,
            Description = description
        };
        await _roleRepository.AddAsync(newRole);
        await _roleRepository.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteRoleAsync(long roleId)
    {
        var role = await _roleRepository.GetAsync(roleId);
        if (role == null)
        {
            return false;
        }
        _roleRepository.Remove(role);
        await _roleRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync(true);
        var roleDtos = roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description
        }).ToList();
        return roleDtos;
    }
    public async Task<RoleDto?> GetRoleByIdAsync(long roleId)
    {
        var role = await _roleRepository.GetAsync(roleId);
        if (role == null)
        {
            return null;
        }
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }

    public async Task<bool> UpdateRoleAsync(long roleId, string? roleName = null, string? description = null)
    {
        var role = await _roleRepository.GetAsync(roleId);
        if (role == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(roleName))
        {
            var spec = new Specification<Role>
                (r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase) && r.Id != roleId);

            var existingRole = await _roleRepository.GetWithSpecAsync(spec);
            if (existingRole != null)
            {
                return false;
            }
            role.Name = roleName;
        }
        if (description != null)
        {
            role.Description = description;
        }
        await _roleRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateRolePermission(long roleId, List<long> permissionIds)
    {
        var role = await _roleRepository.GetAsync(roleId);
        if (role == null)
            return false;

        var newPermissions = new HashSet<long>(permissionIds);
        var currentPermissions = role.RolePermissions
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        var toRemove = role.RolePermissions
            .Where(rp => !newPermissions.Contains(rp.PermissionId))
            .ToList();
        foreach (var rp in toRemove)
            role.RolePermissions.Remove(rp);

        var toAdd = newPermissions.Except(currentPermissions);
        foreach (var permId in toAdd)
        {
            role.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permId
            });
        }

        await _roleRepository.SaveChangesAsync();
        return true;
    }
}
public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public List<PermissionDto> Permissions { get; set; } = [];
}
public class CreateRoleInput
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}