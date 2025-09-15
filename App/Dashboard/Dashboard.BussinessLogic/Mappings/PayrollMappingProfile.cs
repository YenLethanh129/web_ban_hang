using AutoMapper;
using Dashboard.BussinessLogic.Dtos.PayrollDtos;
using Dashboard.DataAccess.Models.Entities.Employees;

namespace Dashboard.BussinessLogic.Mappings;

public class PayrollMappingProfile : Profile
{
    public PayrollMappingProfile()
    {
        CreateMap<EmployeePayroll, PayrollDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Employee.Branch.Name))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Employee.Position))
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.BaseSalary ?? 0))
            .ForMember(dest => dest.Allowance, opt => opt.MapFrom(src => src.Allowance ?? 0))
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0))
            .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => src.Penalty ?? 0))
            .ForMember(dest => dest.GrossSalary, opt => opt.MapFrom(src => src.GrossSalary ?? 0))
            .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount ?? 0))
            .ForMember(dest => dest.NetSalary, opt => opt.MapFrom(src => src.NetSalary ?? 0));

        CreateMap<CreatePayrollInput, EmployeePayroll>()
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0))
            .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => src.Penalty ?? 0));

        CreateMap<EmployeeSalary, EmployeeSalaryDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.SalaryType ?? "MONTHLY"))
            .ForMember(dest => dest.Allowance, opt => opt.MapFrom(src => src.Allowance ?? 0))
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0))
            .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => src.Penalty ?? 0))
            .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.TaxRate ?? 0));

        CreateMap<CreateEmployeeSalaryInput, EmployeeSalary>()
            .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.SalaryType ?? "MONTHLY"))
            .ForMember(dest => dest.Allowance, opt => opt.MapFrom(src => src.Allowance ?? 0))
            .ForMember(dest => dest.Bonus, opt => opt.MapFrom(src => src.Bonus ?? 0))
            .ForMember(dest => dest.Penalty, opt => opt.MapFrom(src => src.Penalty ?? 0))
            .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.TaxRate ?? 0));

        CreateMap<UpdateEmployeeSalaryInput, EmployeeSalary>()
            .ForAllOtherMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
