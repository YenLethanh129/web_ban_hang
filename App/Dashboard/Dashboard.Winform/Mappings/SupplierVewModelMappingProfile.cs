using AutoMapper;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Mappings
{
    public class SupplierVewModelMappingProfile : Profile
    {
        public SupplierVewModelMappingProfile()
        {
            CreateMap<SupplierDto, SupplierViewModel>().ReverseMap();
            CreateMap<SupplierDto, SupplierDetailViewModel>().ReverseMap();
        }
    }
}