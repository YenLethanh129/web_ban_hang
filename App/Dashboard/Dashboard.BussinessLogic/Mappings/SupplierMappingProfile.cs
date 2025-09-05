using AutoMapper;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Mappings;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        // Supplier mappings
        CreateMap<Supplier, SupplierDto>()
            .ForMember(dest => dest.TotalOrders, 
                opt => opt.MapFrom(src => src.IngredientPurchaseOrders.Count))
            .ForMember(dest => dest.TotalAmount, 
                opt => opt.MapFrom(src => src.IngredientPurchaseOrders.Sum(po => po.FinalAmount ?? 0)))
            .ForMember(dest => dest.ActiveIngredients, 
                opt => opt.MapFrom(src => src.SupplierIngredientPrices
                    .Where(sip => (sip.EffectiveDate == null || sip.EffectiveDate <= DateTime.Now) &&
                                  (sip.ExpiredDate == null || sip.ExpiredDate > DateTime.Now))
                    .Count()));

        CreateMap<CreateSupplierInput, Supplier>();
        CreateMap<UpdateSupplierInput, Supplier>();

        // Supplier Price mappings
        CreateMap<SupplierIngredientPrice, SupplierIngredientPriceDto>()
            .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.IngredientName, 
                opt => opt.MapFrom(src => src.Ingredient.Name))
            .ForMember(dest => dest.IsActive, 
                opt => opt.MapFrom(src => (src.EffectiveDate == null || src.EffectiveDate <= DateTime.Now) &&
                                          (src.ExpiredDate == null || src.ExpiredDate > DateTime.Now)));

        CreateMap<CreateSupplierPriceInput, SupplierIngredientPrice>();
        CreateMap<UpdateSupplierPriceInput, SupplierIngredientPrice>();

        // Supplier Performance mappings
        CreateMap<SupplierPerformance, SupplierPerformanceDto>()
            .ForMember(dest => dest.SupplierName, 
                opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.AverageOrderValue, 
                opt => opt.MapFrom(src => src.TotalOrders > 0 ? src.TotalAmount / src.TotalOrders : 0))
            .ForMember(dest => dest.OnTimeDeliveryRate, 
                opt => opt.MapFrom(src => src.TotalOrders > 0 && src.OnTimeDeliveries.HasValue 
                    ? (decimal)src.OnTimeDeliveries.Value / src.TotalOrders.Value * 100 : 0));

        // Supplier Summary mappings
        CreateMap<Supplier, SupplierSummaryDto>()
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.TotalIngredients, 
                opt => opt.MapFrom(src => src.SupplierIngredientPrices.Select(sip => sip.IngredientId).Distinct().Count()))
            .ForMember(dest => dest.ActivePrices, 
                opt => opt.MapFrom(src => src.SupplierIngredientPrices
                    .Where(sip => (sip.EffectiveDate == null || sip.EffectiveDate <= DateTime.Now) &&
                                  (sip.ExpiredDate == null || sip.ExpiredDate > DateTime.Now))
                    .Count()))
            .ForMember(dest => dest.TotalPurchaseOrders, 
                opt => opt.MapFrom(src => src.IngredientPurchaseOrders.Count))
            .ForMember(dest => dest.TotalPurchaseAmount, 
                opt => opt.MapFrom(src => src.IngredientPurchaseOrders.Sum(po => po.FinalAmount ?? 0)))
            .ForMember(dest => dest.LastOrderDate, 
                opt => opt.MapFrom(src => src.IngredientPurchaseOrders.Any() ? 
                    src.IngredientPurchaseOrders.OrderByDescending(po => po.CreatedAt).First().CreatedAt : (DateTime?)null))
            .ForMember(dest => dest.OverallRating, 
                opt => opt.MapFrom(src => src.SupplierPerformances.Any() ? 
                    src.SupplierPerformances.OrderByDescending(sp => sp.CreatedAt).First().OverallRating : (decimal?)null))
            .ForMember(dest => dest.IsActive, 
                opt => opt.MapFrom(src => src.SupplierIngredientPrices.Any(sip => 
                    (sip.EffectiveDate == null || sip.EffectiveDate <= DateTime.Now) &&
                    (sip.ExpiredDate == null || sip.ExpiredDate > DateTime.Now))));
    }
}
