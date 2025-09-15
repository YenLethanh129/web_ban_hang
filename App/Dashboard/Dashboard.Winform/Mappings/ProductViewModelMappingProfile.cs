using AutoMapper;
using Dashboard.BussinessLogic.Dtos.CategoryDto;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Mappings;

public class ProductViewModelMappingProfile : Profile
{
    public ProductViewModelMappingProfile()
    {
        // Product list view
        CreateMap<ProductDto, ProductViewModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"));

        // Product detail view
        CreateMap<ProductDetailDto, ProductDetailViewModel>()
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => src.Recipes))
            .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"));

        // Product images
        CreateMap<ProductImageDto, ProductImageViewModel>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now)); // fallback

        // Recipe
        CreateMap<RecipeDto, RecipeViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"));

        CreateMap<RecipeDto, RecipeDetailViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore());

        // RecipeIngredient (chưa có DTO, nếu cần thì bổ sung sau)

        // ProductRecipe
        CreateMap<ProductRecipeDto, ProductRecipeViewModel>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, CategoryViewModel>();
        //CreateMap<Cate, CreateCategoryInput>();
    }
}
