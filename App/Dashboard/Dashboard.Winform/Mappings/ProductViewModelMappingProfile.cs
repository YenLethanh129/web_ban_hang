using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Mappings;

public class ProductViewModelMappingProfile : Profile
{
    public ProductViewModelMappingProfile()
    {
        CreateMap<ProductDto, ProductViewModel>()
            .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : null))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"));

        CreateMap<ProductDto, ProductDetailViewModel>()
            .ForMember(dest => dest.ThumbnailPath, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : null))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => new List<RecipeViewModel>()))
            .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => new List<ProductRecipeViewModel>()));


        CreateMap<ProductDetailDto, ProductDetailViewModel>()
            .ForMember(dest => dest.ThumbnailPath, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.First().ImageUrl : null))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => src.Recipes))
            .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"));

        // Product images
        CreateMap<ProductImageDto, ProductImageViewModel>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now)); 

        // Recipe
        CreateMap<RecipeDto, RecipeViewModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "ACTIVE" : "INACTIVE"))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<RecipeViewModel, RecipeDto>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));


        CreateMap<RecipeDto, RecipeDetailViewModel>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.ProductName, opt => opt.Ignore());

        CreateMap<RecipeDto, RecipeViewModel>()
            .ReverseMap();

        CreateMap<RecipeDetailViewModel, CreateRecipeInput>();
        CreateMap<RecipeViewModel, CreateRecipeInput>();
        CreateMap<RecipeDetailViewModel, UpdateRecipeInput>();
        CreateMap<RecipeViewModel, UpdateRecipeInput>();


        CreateMap<ProductRecipeDto, ProductRecipeViewModel>()
            .ReverseMap();

        CreateMap<CategoryDto, CategoryViewModel>();
        CreateMap<TaxDto, TaxViewModel>()
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.TaxRate));

        CreateMap<ProductDetailViewModel, UpdateProductInput>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => (int)(src.CategoryId ?? 0)))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                src.ProductImages != null
                    ? src.ProductImages
                        .Where(pi => pi.ImageUrl != null)
                        .Select(pi => pi.ImageUrl!)
                        .ToList()
                    : new List<string>()));

        CreateMap<ProductDetailViewModel, CreateProductInput>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => (int)(src.CategoryId ?? 0)))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                src.ProductImages != null
                    ? src.ProductImages
                        .Where(pi => pi.ImageUrl != null)
                        .Select(pi => pi.ImageUrl!)
                        .ToList()
                    : new List<string>()));

        CreateMap<RecipeIngredientDto, RecipeIngredientViewModel>()
            .ReverseMap();


        CreateMap<ProductRecipeDto, ProductRecipeViewModel>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, CategoryViewModel>().ReverseMap();

        CreateMap<CreateRecipeIngredientInput, RecipeIngredient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        CreateMap<RecipeIngredientViewModel, UpdateRecipeIngredientInput>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.WastePercentage, opt => opt.MapFrom(src => src.WastePercentage))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.IsOptional, opt => opt.MapFrom(src => src.IsOptional))
            .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));

        
    }
}
