using AutoMapper;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.BussinessLogic.Dtos.ExpenseDtos;

namespace Dashboard.BussinessLogic.Mappings;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<BranchExpense, ExpenseDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));

        CreateMap<CreateExpenseInput, BranchExpense>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore())
            .ForMember(dest => dest.Branch, opt => opt.Ignore());

        CreateMap<UpdateExpenseInput, BranchExpense>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BranchId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore())
            .ForMember(dest => dest.Branch, opt => opt.Ignore());

        CreateMap<VExpensesSummary, ExpenseSummaryDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));
    }
}
