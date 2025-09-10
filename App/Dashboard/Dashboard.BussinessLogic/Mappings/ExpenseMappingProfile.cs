using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ExpenseDtos;
using Dashboard.DataAccess.Models.Entities;

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

        CreateMap<VCogsSummary, ExpenseSummaryDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));
        CreateMap<VCogsSummary, ExpenseDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId ?? 0))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty))
            .ForMember(dest => dest.ExpenseType, opt => opt.MapFrom(src => "COGS"))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Period))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.ExpenseAfterTax))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => new DateTime(src.Year, src.Month, 1)))
            .ForMember(dest => dest.EndDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

    }
}
