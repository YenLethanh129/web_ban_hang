using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Mappings;

public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        CreateMap<DashboardSummaryDto, LandingDashboardModel>()
            .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.TotalRevenue))
            .ForMember(dest => dest.NetProfit, opt => opt.MapFrom(src => src.NetProfit))
            .ForMember(dest => dest.TotalExpenses, opt => opt.MapFrom(src => src.TotalExpenses))
            .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.TotalOrders))
            .ForMember(dest => dest.PendingOrders, opt => opt.MapFrom(src => src.PendingOrders))
            .ForMember(dest => dest.TopProducts, opt => opt.MapFrom(src => src.TopProducts))
            .ForMember(dest => dest.BranchPerformance, opt => opt.MapFrom(src => src.BranchPerformance));

        CreateMap<TopSellingProductDto, TopProductViewModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.QuantitySold, opt => opt.MapFrom(src => src.QuantitySold))
            .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Revenue));

        CreateMap<BranchPerformanceDto, BranchPerformanceViewModel>()
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Revenue))
            .ForMember(dest => dest.Profit, opt => opt.MapFrom(src => src.Profit))
            .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.OrderCount));

        CreateMap<FinacialReportDto, RevenueReportViewModel>()
            .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.TotalRevenue))
            .ForMember(dest => dest.TotalExpenses, opt => opt.MapFrom(src => src.TotalExpenses))
            .ForMember(dest => dest.NetProfit, opt => opt.MapFrom(src => src.NetProfit))
            .ForMember(dest => dest.ReportDate, opt => opt.MapFrom(src => src.ReportDate))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName));
        CreateMap<FinacialReportDto, FinancialReportByDateViewModel>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ReportDate))
            .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.TotalRevenue))
            .ForMember(dest => dest.Expense, opt => opt.MapFrom(src => src.TotalExpenses));
    }
}