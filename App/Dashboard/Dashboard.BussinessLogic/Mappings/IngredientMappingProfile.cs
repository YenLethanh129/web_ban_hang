using AutoMapper;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;

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
            .ForMember(dest => dest.IngredientCategoryId,
                opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.LastModified));


        CreateMap<IngredientDto, Ingredient>()
            .ForMember(dest => dest.CategoryId,
                opt => opt.MapFrom(src => src.IngredientCategoryId))
            .ForMember(dest => dest.Category,
                opt => opt.Ignore())
            .ForMember(dest => dest.BranchIngredientInventories,
                opt => opt.Ignore())
            .ForMember(dest => dest.GoodsReceivedDetails,
                opt => opt.Ignore())
            .ForMember(dest => dest.IngredientPurchaseOrderDetails,
                opt => opt.Ignore())
            .ForMember(dest => dest.IngredientTransfers,
                opt => opt.Ignore())
            .ForMember(dest => dest.TransferRequestDetails,
                opt => opt.Ignore())
            .ForMember(dest => dest.ProductRecipes,
                opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseInvoiceDetails,
                opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseReturnDetails,
                opt => opt.Ignore())
            .ForMember(dest => dest.SupplierIngredientPrices,
                opt => opt.Ignore())
            .ForMember(dest => dest.InventoryThresholds,
                opt => opt.Ignore())
            .ForMember(dest => dest.InventoryMovements,
                opt => opt.Ignore());


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