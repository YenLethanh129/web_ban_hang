using AutoMapper;
using Dashboard.BussinessLogic.Dtos.CustomerDto;
using Dashboard.DataAccess.Models.Entities.Customers;

namespace Dashboard.BussinessLogic.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Count : 0))
            .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Sum(o => o.TotalMoney ?? 0) : 0))
            .ForMember(dest => dest.LastOrderDate, opt => opt.MapFrom(src => src.Orders != null && src.Orders.Any() ? src.Orders.Max(o => o.CreatedAt) : (DateTime?)null));

        CreateMap<CreateCustomerInput, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<UpdateCustomerInput, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<Customer, TopCustomerDto>()
            .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Count : 0))
            .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Sum(o => o.TotalMoney ?? 0) : 0))
            .ForMember(dest => dest.LastOrderDate, opt => opt.MapFrom(src => src.Orders != null && src.Orders.Any() ? src.Orders.Max(o => o.CreatedAt) : (DateTime?)null));
    }
}