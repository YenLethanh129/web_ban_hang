using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;
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

public class RoleManagementService : BaseTransactionalService, IRoleManagementService
{
    private readonly IRoleRepository _roleRepository;

    public RoleManagementService(IUnitOfWork unitOfWork, IRoleRepository roleRepository) : base(unitOfWork)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> CreateRoleAsync(string roleName, string? description = null)
    {
        try
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var existingRoles = await _roleRepository.GetAllAsync();
                var isDuplicate = existingRoles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

                if (isDuplicate)
                {
                    return false;
                }

                var role = new Role
                {
                    Name = roleName,
                    Description = description,
                    CreatedAt = DateTime.UtcNow
                };

                await _roleRepository.AddAsync(role);
                await _unitOfWork.SaveChangesAsync();
                return true;
            });
        }
        catch (Exception)
        {
            return false;
        }
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
        // Load roles with related rolepermissions and permissions to be able to return permissions per role
        var roles = await _roleRepository.GetAllAsync(true);

        // If RolePermissions not included by repository, try to load them explicitly
        foreach (var r in roles)
        {
            if (r.RolePermissions == null)
            {
                r.RolePermissions = await _unitOfWork.Repository<RolePermission>()
                    .GetQueryable()
                    .Where(rp => rp.RoleId == r.Id)
                    .Include(rp => rp.Permission)
                    .ToListAsync();
            }
        }

        var roleDtos = roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            CreatedAt = r.CreatedAt,
            LastModified = r.LastModified,
            Permissions = (r.RolePermissions ?? new List<RolePermission>())
                .Where(rp => rp.Permission != null)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission!.Id,
                    Name = rp.Permission!.Name,
                    Description = rp.Permission!.Description,
                    Resource = rp.Permission!.Resource,
                    Action = rp.Permission!.Action,
                    CreatedAt = rp.Permission!.CreatedAt
                }).ToList()
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

        // Ensure role permissions and permission entities are loaded
        var rolePermissions = await _unitOfWork.Repository<RolePermission>()
            .GetQueryable()
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .ToListAsync();

        var permissionDtos = rolePermissions
            .Where(rp => rp.Permission != null)
            .Select(rp => new PermissionDto
            {
                Id = rp.Permission!.Id,
                Name = rp.Permission!.Name,
                Description = rp.Permission!.Description,
                Resource = rp.Permission!.Resource,
                Action = rp.Permission!.Action,
                CreatedAt = rp.Permission!.CreatedAt
            })
            .ToList();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            LastModified = role.LastModified,
            Permissions = permissionDtos
        };
    }

    public async Task<bool> UpdateRoleAsync(long roleId, string? roleName = null, string? description = null)
    {
        try
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var role = await _roleRepository.GetAsync(roleId);
                if (role == null)
                {
                    return false;
                }

                if (roleName != null)
                {
                    var existingRoles = await _roleRepository.GetAllAsync();
                    var isDuplicate = existingRoles.Any(r =>
                        r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase) && r.Id != roleId);

                    if (isDuplicate)
                    {
                        return false;
                    }

                    role.Name = roleName;
                }

                if (description != null)
                {
                    role.Description = description;
                }

                role.LastModified = DateTime.UtcNow;

                _roleRepository.Update(role);
                await _unitOfWork.SaveChangesAsync();
                return true;
            });
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateRolePermission(long roleId, List<long> permissionIds)
    {
        try
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var currentRolePermissions = await _unitOfWork.Repository<RolePermission>()
                    .GetAllWithSpecAsync(new Specification<RolePermission>(rp => rp.RoleId == roleId));

                // Remove those not in new list
                foreach (var rp in currentRolePermissions)
                {
                    if (!permissionIds.Contains(rp.PermissionId))
                        _unitOfWork.Repository<RolePermission>().Remove(rp);
                }

                // Add missing ones
                var existingIds = currentRolePermissions.Select(rp => rp.PermissionId).ToHashSet();
                foreach (var permissionId in permissionIds.Except(existingIds))
                {
                    await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    });
                }

                await _unitOfWork.SaveChangesAsync();
                return true;
            });
        }
        catch (Exception)
        {
            return false;
        }
    }

}
public class CreateRoleInput
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}