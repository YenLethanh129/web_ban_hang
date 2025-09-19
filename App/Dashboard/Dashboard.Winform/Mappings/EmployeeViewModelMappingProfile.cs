using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Dtos.PayrollDtos;
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

            CreateMap<EmployeeSalaryDto, EmployeeSalaryViewModel>()
                .ForMember(dest => dest.Allowance, opt => opt.MapFrom(src => (decimal?)src.Allowance))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => (decimal?)src.Bonus))
                .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => (decimal?)src.Penalty))
                .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => (decimal?)src.TaxRate));

            CreateMap<EmployeeSalaryViewModel, EmployeeSalaryDto>()
                .ForMember(dest => dest.Allowance, opt => opt.MapFrom(src => src.Allowance ?? 0))
                .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0))
                .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => src.Penalty ?? 0))
                .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.TaxRate ?? 0));

            CreateMap<PayrollDto, PayrollViewModel>()
                .ForMember(dest => dest.PayrollMonth, opt => opt.MapFrom(src => $"{src.Year}-{src.Month:D2}-01"))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.TotalWorkingHours ?? 0))
                .ForMember(dest => dest.BasicSalary, opt => opt.MapFrom(src => src.BaseSalary))
                .ForMember(dest => dest.Allowances, opt => opt.MapFrom(src => src.Allowance))
                .ForMember(dest => dest.Bonuses, opt => opt.MapFrom(src => src.Bonus))
                .ForMember(dest => dest.Deductions, opt => opt.MapFrom(src => src.Penalty + src.TaxAmount))
                .ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.NetSalary))
                .ForMember(dest => dest.PayrollDate, opt => opt.MapFrom(src => src.CreatedAt));
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => "PAID"));

            CreateMap<PayrollViewModel, PayrollDto>()
                .ConvertUsing((PayrollViewModel src, PayrollDto dest, ResolutionContext ctx) =>
                {
                    var dto = new PayrollDto();

                    if (DateTime.TryParse(src.PayrollMonth, out var dt))
                    {
                        dto.Month = dt.Month;
                        dto.Year = dt.Year;
                    }

                    dto.EmployeeId = src.EmployeeId;
                    dto.TotalWorkingHours = src.WorkingHours;
                    dto.BaseSalary = src.BasicSalary;
                    dto.Allowance = src.Allowances;
                    dto.Bonus = src.Bonuses;
                    dto.Penalty = src.Deductions;
                    dto.NetSalary = src.TotalSalary;
                    dto.CreatedAt = src.PayrollDate;
                    dto.LastModified = DateTime.Now;

                    return dto;
                });

            CreateMap<EmployeeShiftDto, EmployeeShiftViewModel>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status));

            CreateMap<EmployeeDetailViewModel, CreateEmployeeInput>()
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.HireDate,
                    opt => opt.MapFrom(src => src.HireDate ?? DateTime.Now));

            CreateMap<EmployeeShiftViewModel, EmployeeShiftDto>()
                .ForMember(dest => dest.WorkingHours,
                    opt => opt.MapFrom(src => (decimal)src.WorkingHours))
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.LastModified,
                    opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
