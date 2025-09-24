using AutoMapper;

using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Dashboard.Winform.ViewModels.RBACModels;


namespace Dashboard.Winform.Mappings;
public class RBACViewModelMappingProfile : Profile
{
    public RBACViewModelMappingProfile()
    {
        CreateMap<CreateUserInput, UserDetailViewModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedRoles, opt => opt.Ignore())
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateUserInput, UserDetailViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) 
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedRoles, opt => opt.Ignore())
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UserDetailViewModel, CreateUserInput>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId ?? 0L))
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UserDetailViewModel, UpdateUserInput>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId ?? 0L))
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UserDto, UserViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName ?? string.Empty))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<UserDto, UserDetailViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.AssignedRoles, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<UserDto, UpdateUserInput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId ?? 0L))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ChangePasswordInput, UserDetailViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword))
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedRoles, opt => opt.Ignore());


        CreateMap<RoleDto, RoleViewModel>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.PermissionCount, opt => opt.MapFrom(src => src.Permissions.Count));
            //.ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));


        CreateMap<RoleViewModel, RoleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<PermissionDto, PermissionViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src.Resource))
            .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<EmployeeDto, EmployeeSimpleViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.PositionName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap(typeof(PagedList<>), typeof(List<>))
            .ConvertUsing(typeof(PagedListToListConverter<,>));

        CreateMap<EmployeeViewModel, EmployeeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.PositionName))
            .ForAllOtherMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UserDto, EmployeeUserAccount>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId ?? 0))
            //.ForMember(dest => dest.Tokens, opt => opt.Ignore()) 
            .ForMember(dest => dest.Role, opt => opt.Ignore()) 
            .ForMember(dest => dest.Employee, opt => opt.Ignore()) 
            .ForAllOtherMembers(opt => opt.Ignore());

        CreateMap<EmployeeUserAccount, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

    }
}
