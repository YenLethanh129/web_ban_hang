using AutoMapper;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Mappings;

public class IngredientMappingProfile : Profile
{
    public IngredientMappingProfile()
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

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