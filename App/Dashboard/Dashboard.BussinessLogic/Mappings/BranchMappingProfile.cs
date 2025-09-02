using AutoMapper;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Mappings;

public class BranchMappingProfile : Profile
{
    public BranchMappingProfile()
    {
        CreateMap<Branch, BranchDto>();
    }
}
