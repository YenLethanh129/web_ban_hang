using AutoMapper;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Models.Entities.Products;

namespace Dashboard.BussinessLogic.Mappings;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<Ingredient, LowStockIngredientDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.InStockQuantity, opt => opt.MapFrom(src => src.IngredientWarehouse != null ? src.IngredientWarehouse.Quantity : 0))
            .ForMember(dest => dest.SafetyStock, opt => opt.MapFrom(src => src.IngredientWarehouse != null ? src.IngredientWarehouse.SafetyStock : 0))
            .ForMember(dest => dest.MaximumStock, opt => opt.MapFrom(src => src.IngredientWarehouse != null ? src.IngredientWarehouse.MaximumStock : 0));

        CreateMap<Ingredient, IngredientDto>()
            .ForMember(dest => dest.CategoryId,
                opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive));

        CreateMap<IngredientDto, Ingredient>()
            .ForMember(dest => dest.CategoryId,
                opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Unit,
                opt => opt.MapFrom(src => src.Unit))
            .ForAllOtherMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<CreateRecipeIngredientInput, RecipeIngredient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.LastModified, opt => opt.Ignore());

        // Update -> Entity
        CreateMap<UpdateRecipeIngredientInput, RecipeIngredient>()
            .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
            .ForMember(dest => dest.IngredientId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Entity -> DTO
        CreateMap<RecipeIngredient, RecipeIngredientDto>();

        CreateMap<BranchIngredientInventory, BranchIngredientInventoryDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));

        CreateMap<IngredientWarehouse, WarehouseIngredientInventoryDto>();
        CreateMap<CreateIngredientInput, Ingredient>();
        CreateMap<UpdateIngredientInput, Ingredient>();
        CreateMap<CreateBranchInventoryInput, BranchIngredientInventory>();
        CreateMap<UpdateBranchInventoryInput, BranchIngredientInventory>();
        CreateMap<CreateWarehouseInventoryInput, IngredientWarehouse>();
        CreateMap<UpdateWarehouseInventoryInput, IngredientWarehouse>();

    }
}