using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages));

        CreateMap<ProductImage, ProductImageDto>();

        CreateMap<CreateProductInput, Product>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

        CreateMap<UpdateProductInput, Product>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore());
    }
}
