using Dashboard.BussinessLogic.Mappings;
using Dashboard.BussinessLogic.Services;
using Dashboard.DataAccess.Data;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Dashboard.BussinessLogic;
public static class DependencyInjection
{
    // Keep the existing method for IHostApplicationBuilder
    public static void AddBussinessLogicServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(ProductMappingProfile));
        builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));
        builder.Services.AddAutoMapper(typeof(BranchMappingProfile));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IBranchService, BranchService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IReportingService, ReportingService>();
    }
}