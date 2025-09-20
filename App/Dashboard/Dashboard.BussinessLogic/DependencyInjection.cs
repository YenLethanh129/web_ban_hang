using Dashboard.BussinessLogic.Mappings;
using Dashboard.BussinessLogic.Services;
using Dashboard.BussinessLogic.Services.BranchServices;
using Dashboard.BussinessLogic.Services.Customers;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.BussinessLogic.Services.ProductServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.BussinessLogic.Services.ReportServices;
using Dashboard.BussinessLogic.Services.SupplierServices;
using Dashboard.Common.Utitlities;
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
        builder.Services.AddAutoMapper(typeof(ExpenseMappingProfile));
        builder.Services.AddAutoMapper(typeof(SupplierMappingProfile));
        builder.Services.AddAutoMapper(typeof(RBACMappingProfile));
        builder.Services.AddAutoMapper(typeof(EmployeeMappingProfile));
        builder.Services.AddAutoMapper(typeof(PayrollMappingProfile));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ITaxService, TaxService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IBranchService, BranchService>();
        builder.Services.AddScoped<IExpenseService, ExpenseService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IReportingService, ReportingService>();
        builder.Services.AddHttpClient<IImageUrlValidator, ImageUrlValidator>();


        // Ingredient-related services
        builder.Services.AddScoped<IIngredientManagementService, IngredientManagementService>();
        builder.Services.AddScoped<IBranchInventoryService, BranchInventoryService>();
        builder.Services.AddScoped<IWarehouseInventoryService, WarehouseInventoryService>();
        builder.Services.AddScoped<IInventoryMonitoringService, InventoryMonitoringService>();
        
        // Supplier-related services
        builder.Services.AddScoped<ISupplierManagementService, SupplierManagementService>();
        builder.Services.AddScoped<ISupplierPriceService, SupplierPriceService>();
        builder.Services.AddScoped<ISupplierPerformanceService, SupplierPerformanceService>();
        builder.Services.AddScoped<ISupplierAnalyticsService, SupplierAnalyticsService>();
        
        // Authentication and Authorization services
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();

        // Employee Shift and Payroll services
        builder.Services.AddScoped<IEmployeeShiftService, EmployeeShiftService>();
        builder.Services.AddScoped<IPayrollService, PayrollService>();
        builder.Services.AddScoped<IEmployeeManagementService, EmployeeManagementService>();

        // Role management service
        builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();

    }
}