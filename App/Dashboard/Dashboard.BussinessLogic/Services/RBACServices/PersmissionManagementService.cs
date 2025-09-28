using AutoMapper;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Services.RBACServices
{
    public interface IPermissionManagementService
    {
        Task AssignPermissionToRoleAsync(long roleId, long permissionId);
        Task RemovePermissionFromRoleAsync(long roleId, long permissionId);
        Task<List<long>> GetPermissionsByRoleIdAsync(long roleId);
        Task<List<long>> GetRolesByPermissionIdAsync(long permissionId);
        Task<List<PermissionDto>> GetAllPermission();

        // Thêm CRUD methods
        Task<PermissionDto?> GetPermissionByIdAsync(long id);
        Task<bool> CreatePermissionAsync(string name, string resource, string action, string? description = null);
        Task<bool> UpdatePermissionAsync(long id, string name, string resource, string action, string? description = null);
        Task<bool> DeletePermissionAsync(long id);
        Task<bool> PermissionExistsAsync(long id);
    }

    public class PermissionManagementService : BaseTransactionalService, IPermissionManagementService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionManagementService(
            IPermissionRepository permissionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task AssignPermissionToRoleAsync(long roleId, long permissionId)
        {
            var existingSpec = new Specification<RolePermission>(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            var existing = await _unitOfWork.Repository<RolePermission>().GetWithSpecAsync(existingSpec);

            if (existing == null)
            {
                await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<PermissionDto>> GetAllPermission()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }

        public async Task<PermissionDto?> GetPermissionByIdAsync(long id)
        {
            var permission = await _permissionRepository.GetAsync(id);
            return permission != null ? _mapper.Map<PermissionDto>(permission) : null;
        }

        public async Task<bool> CreatePermissionAsync(string name, string resource, string action, string? description = null)
        {
            try
            {
                var existingPermissions = await _permissionRepository.GetAllAsync();
                var isDuplicate = existingPermissions.Any(p =>
                    p.Name.ToLower() == name.ToLower() &&
                    p.Resource.ToLower() == resource.ToLower() &&
                    p.Action.ToLower() == action.ToLower());

                if (isDuplicate)
                {
                    return false;
                }

                var permission = new Permission
                {
                    Name = name,
                    Resource = resource,
                    Action = action,
                    Description = description,
                    CreatedAt = DateTime.UtcNow
                };

                await _permissionRepository.AddAsync(permission);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePermissionAsync(long id, string name, string resource, string action, string? description = null)
        {
            try
            {
                var permission = await _permissionRepository.GetAsync(id);
                if (permission == null)
                {
                    return false;
                }

                var existingPermissions = await _permissionRepository.GetAllAsync();
                var isDuplicate = existingPermissions.Any(p =>
                    p.Id != id &&
                    p.Name.ToLower() == name.ToLower() &&
                    p.Resource.ToLower() == resource.ToLower() &&
                    p.Action.ToLower() == action.ToLower());

                if (isDuplicate)
                {
                    return false;
                }

                permission.Name = name;
                permission.Resource = resource;
                permission.Action = action;
                permission.Description = description;
                permission.LastModified = DateTime.UtcNow;

                _permissionRepository.Update(permission);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeletePermissionAsync(long id)
        {
            try
            {
                return await ExecuteInTransactionAsync(async () =>
                {
                    var rolePermissionSpec = new Specification<RolePermission>(rp => rp.PermissionId == id);
                    var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetAllWithSpecAsync(rolePermissionSpec);

                    foreach (var rp in rolePermissions)
                    {
                        _unitOfWork.Repository<RolePermission>().Remove(rp);
                    }

                    // Xóa permission
                    var permission = await _permissionRepository.GetAsync(id);
                    if (permission != null)
                    {
                        _permissionRepository.Remove(permission);
                        await _unitOfWork.SaveChangesAsync();
                        return true;
                    }
                    return false;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PermissionExistsAsync(long id)
        {
            var permission = await _permissionRepository.GetAsync(id);
            return permission != null;
        }

        public async Task<List<long>> GetPermissionsByRoleIdAsync(long roleId)
        {
            var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId);
            var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetAllWithSpecAsync(spec);
            return rolePermissions.Select(rp => rp.PermissionId).ToList();
        }

        public async Task<List<long>> GetRolesByPermissionIdAsync(long permissionId)
        {
            var spec = new Specification<RolePermission>(rp => rp.PermissionId == permissionId);
            var rolePermissions = await _unitOfWork.Repository<RolePermission>().GetAllWithSpecAsync(spec);
            return rolePermissions.Select(rp => rp.RoleId).ToList();
        }

        public async Task RemovePermissionFromRoleAsync(long roleId, long permissionId)
        {
            await ExecuteInTransactionAsync(async () =>
            {
                var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
                var rolePermission = await _unitOfWork.Repository<RolePermission>().GetWithSpecAsync(spec);
                if (rolePermission != null)
                {
                    _unitOfWork.Repository<RolePermission>().Remove(rolePermission);
                    await _unitOfWork.SaveChangesAsync();
                }
            });
        }
    }
}