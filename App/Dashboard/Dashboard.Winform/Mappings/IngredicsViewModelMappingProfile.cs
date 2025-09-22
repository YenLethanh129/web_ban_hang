using AutoMapper;
using Dashboard.Winform.ViewModels;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;

namespace Dashboard.Winform.Mappings
{
    public class IngredicsViewModelMappingProfile : Profile
    {
        public IngredicsViewModelMappingProfile()
        {
            CreateMap<IngredientDetailViewModel, UpdateIngredientInput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit ?? string.Empty));
            CreateMap<IngredientDetailViewModel, CreateIngredientInput>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => (int)src.CategoryId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit ?? string.Empty));

            CreateMap<IngredientDto, IngredientDetailViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.TaxId, opt => opt.Ignore());
        }
    }
}