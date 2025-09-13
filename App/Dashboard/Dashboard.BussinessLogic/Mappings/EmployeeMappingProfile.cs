using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.DataAccess.Models.Entities.Employees;

namespace Dashboard.BussinessLogic.Mappings;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<EmployeeShift, EmployeeShiftDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Employee.Branch.Name))
            .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => CalculateWorkingHours(src.StartTime, src.EndTime)));

        CreateMap<CreateEmployeeShiftInput, EmployeeShift>();
        
        CreateMap<UpdateEmployeeShiftInput, EmployeeShift>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeDetailDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));
        CreateMap<CreateEmployeeInput, Employee>()
            .ForMember(Employee => Employee.Status, opt => opt.MapFrom(_ => "Active"));
        CreateMap<EmployeePosition, PositionDto>();

    }

    private static decimal CalculateWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        var duration = endTime.ToTimeSpan() - startTime.ToTimeSpan();
        if (duration.TotalHours < 0)
        {
            duration = duration.Add(TimeSpan.FromDays(1));
        }
        return (decimal)duration.TotalHours;
    }
}
