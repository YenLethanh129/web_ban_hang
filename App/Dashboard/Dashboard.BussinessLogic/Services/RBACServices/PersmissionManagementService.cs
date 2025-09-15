using AutoMapper;
using AutoMapper.Features;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    }
    public class PersmissionManagementService : BaseTransactionalService, IPermissionManagementService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        public PersmissionManagementService(
            IPermissionRepository permissionRepository, 
            IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task AssignPermissionToRoleAsync(long roleId, long permissionId)
        {
            var permission = _permissionRepository.GetAsync(permissionId);
            var role = _permissionRepository.GetAsync(roleId);

            if (permission == null || role == null)
            {
                throw new ArgumentException("Role or Permission not found.");
            }
            
            await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<PermissionDto>> GetAllPermission()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }

        public Task<List<long>> GetPermissionsByRoleIdAsync(long roleId)
        {
            var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId);
            return _unitOfWork.Repository<RolePermission>()
                .GetAllWithSpecAsync(spec)
                .ContinueWith(t => t.Result.Select(rp => rp.PermissionId).ToList());
        }

        public Task<List<long>> GetRolesByPermissionIdAsync(long permissionId)
        {
           var spec = new Specification<RolePermission>(rp => rp.PermissionId == permissionId);
            return _unitOfWork.Repository<RolePermission>()
                .GetAllWithSpecAsync(spec)
                .ContinueWith(t => t.Result.Select(rp => rp.RoleId).ToList());
        }

        public Task RemovePermissionFromRoleAsync(long roleId, long permissionId)
        {
            var spec = new Specification<RolePermission>(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            return ExecuteInTransactionAsync(async () =>
            {
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
