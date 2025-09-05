using AutoMapper;
using Dashboard.BussinessLogic.Dtos.EmployeeShiftDtos;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Mappings;

public class EmployeeShiftMappingProfile : Profile
{
    public EmployeeShiftMappingProfile()
    {
        CreateMap<EmployeeShift, EmployeeShiftDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Employee.Branch.Name))
            .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => CalculateWorkingHours(src.StartTime, src.EndTime)));

        CreateMap<CreateEmployeeShiftInput, EmployeeShift>();
        
        CreateMap<UpdateEmployeeShiftInput, EmployeeShift>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    private static decimal CalculateWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        var duration = endTime.ToTimeSpan() - startTime.ToTimeSpan();
        if (duration.TotalHours < 0)
        {
            // Handle shifts that cross midnight
            duration = duration.Add(TimeSpan.FromDays(1));
        }
        return (decimal)duration.TotalHours;
    }
}
