using AutoMapper;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.DataAccess.Models.Entities.RBAC;

namespace Dashboard.BussinessLogic.Mappings;

public class RBACMappingProfile : Profile
{
    public RBACMappingProfile()
    {
        CreateMap<EmployeeUserAccount, UserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.CustomerUsers.Count))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => new PermissionDto
            {
                Id = rp.Permission.Id,
                Name = rp.Permission.Name,
                Description = rp.Permission.Description,
                Resource = rp.Permission.Resource,
                Action = rp.Permission.Action,
                CreatedAt = rp.Permission.CreatedAt
            })));

        CreateMap<Permission, PermissionDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<CreateUserInput, EmployeeUserAccount>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        CreateMap<UpdateUserInput, EmployeeUserAccount>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username ?? string.Empty))
            .ForMember(dest => dest.Password, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId ?? 0))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        CreateMap<ChangePasswordInput, EmployeeUserAccount>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword))
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.Now))
            .ForAllOtherMembers(opt => opt.Ignore()); 
        CreateMap<Permission, PermissionDto>();
        CreateMap<PermissionDto, Permission>();
    }
}
