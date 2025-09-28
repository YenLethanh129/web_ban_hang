using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.Winform.ViewModels.EmployeeModels;

namespace Dashboard.Winform.Mappings;

public class BranchViewModelMappingProfile : Profile
{
    public BranchViewModelMappingProfile()
    {
        CreateMap<BranchDto, BranchViewModel>();
    }
}