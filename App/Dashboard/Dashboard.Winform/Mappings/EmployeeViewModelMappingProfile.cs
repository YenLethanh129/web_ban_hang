using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.Winform.ViewModels.EmployeeModels;

namespace Dashboard.Winform.Mappings
{
    public class EmployeeViewModelMappingProfile : Profile
    {
        public EmployeeViewModelMappingProfile()
        {
            // Entity <-> DTO
            CreateMap<EmployeeDto, EmployeeViewModel>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));

            CreateMap<EmployeeDetailDto, EmployeeDetailViewModel>();

            CreateMap<EmployeeViewModel, EmployeeDto>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<EmployeeDetailViewModel, EmployeeDetailDto>();

            // Position
            CreateMap<PositionDto, PositionViewModel>();

            // Input -> Entity
            CreateMap<CreateEmployeeInput, Employee>();

            CreateMap<UpdateEmployeeInput, Employee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForAllOtherMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Entity -> ViewModel
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ResignDate == null));

            // ViewModel -> Input (CREATE)
            CreateMap<EmployeeViewModel, CreateEmployeeInput>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            // ViewModel -> Input (UPDATE) - List View
            CreateMap<EmployeeViewModel, UpdateEmployeeInput>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate));

            // ViewModel -> Input (UPDATE) - Detail View
            CreateMap<EmployeeDetailViewModel, UpdateEmployeeInput>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate))
                .ForMember(dest => dest.ResignDate, opt => opt.MapFrom(src => src.ResignDate))
                .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.CurrentBaseSalary))
                .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.Salaries.FirstOrDefault() != null ? src.Salaries.First().SalaryType : null));
        }
    }
}
