using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Models.Entities.FinacialAndReports;
using Dashboard.DataAccess.Models.Entities.Products;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.TaxName, opt => opt.MapFrom(src => src.Tax != null ? src.Tax.Name : string.Empty))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.TaxName, opt => opt.MapFrom(src => src.Tax != null ? src.Tax.Name : string.Empty))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages))
            .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => src.Recipes))
            .ForMember(dest => dest.ProductRecipes, opt => opt.MapFrom(src => src.ProductRecipes));

        CreateMap<ProductImage, ProductImageDto>()
            .ConstructUsing(src => new ProductImageDto(src.Id, false)
            {
                ImageUrl = src.ImageUrl ?? string.Empty
            });

        CreateMap<Recipe, RecipeDto>();

        CreateMap<ProductRecipe, ProductRecipeDto>()
            .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient != null ? src.Ingredient.Name : string.Empty));

        CreateMap<CreateProductInput, Product>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore());
        CreateMap<UpdateProductInput, Product>()
            .ForMember(dest => dest.TaxId, opt => opt.MapFrom(src => src.TaxId))
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CreateCategoryInput>();
        CreateMap<Category, UpdateCategoryInput>();
        CreateMap<CreateCategoryInput, Category>();
        CreateMap<UpdateCategoryInput, Category>();

        CreateMap<Taxes, TaxDto>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastModified));
        CreateMap<TaxDto, Taxes>();
        CreateMap<CreateTaxInput, Taxes>();
        CreateMap<UpdateTaxInput, Taxes>();
    }
}
