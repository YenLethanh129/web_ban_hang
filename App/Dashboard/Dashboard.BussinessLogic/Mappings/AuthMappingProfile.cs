using AutoMapper;
using Dashboard.BussinessLogic.Dtos.AuthDtos;
using Dashboard.DataAccess.Models.Entities.RBAC;

namespace Dashboard.BussinessLogic.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Fullname ?? ""))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Position : null))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Employee != null ? (long?)src.Employee.BranchId : null))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Branch != null ? src.Employee.Branch.Name : null))
            .ForMember(dest => dest.Permissions, opt => opt.Ignore());

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => new PermissionDto
            {
                Id = rp.Permission.Id,
                Name = rp.Permission.Name,
                Description = rp.Permission.Description,
                Resource = rp.Permission.Resource,
                Action = rp.Permission.Action,
                CreatedAt = rp.Permission.CreatedAt
            })));

        CreateMap<Permission, PermissionDto>();

        CreateMap<CreateUserInput, User>()
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        CreateMap<UpdateUserInput, User>()
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());
    }
}
