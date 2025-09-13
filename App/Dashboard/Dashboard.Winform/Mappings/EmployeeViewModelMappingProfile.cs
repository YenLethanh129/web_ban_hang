using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.Winform.ViewModels.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Winform.Mappings
{
    public class EmployeeViewModelMappingProfile : Profile
    {
        public EmployeeViewModelMappingProfile()
        {
            CreateMap<EmployeeDto, EmployeeViewModel>();
            CreateMap<EmployeeDetailDto, EmployeeDetailViewModel>();
            CreateMap<PositionDto, PositionViewModel>();
            CreateMap<CreateEmployeeInput, Employee>();

            CreateMap<UpdateEmployeeInput, Employee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId)) 
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone)) 
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ResignDate == null));

            CreateMap<EmployeeViewModel, CreateEmployeeInput>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<EmployeeViewModel, UpdateEmployeeInput>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
                .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate));



        }
    }
}
