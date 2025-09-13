using Ardalis.GuardClauses;
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Data.Interceptors;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dashboard.DataAccess;
public static class DependencyInjection
{
    public static void AddDataAccess(this IHostApplicationBuilder builder)
    {
        var Context = builder.Configuration.GetConnectionString("WebbanhangDB");
        Guard.Against.Null(Context, message: "Connection string 'WebbanhangDB' not found.");

        var encryptionKey = builder.Configuration["Encryption:Key"];
        Guard.Against.NullOrEmpty(encryptionKey, nameof(encryptionKey));

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IBranchRepository, BranchRepository>();
        builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
        builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IIngredientTransferRepository, IngredientTransferRepository>();
        builder.Services.AddScoped<IIngredientTransferRequestRepository, IngredientTransferRequestRepository>();
        builder.Services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
        builder.Services.AddScoped<IEmployeeSalaryRepository, EmployeeSalaryRepository>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddDbContext<WebbanhangDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(Context);
        });

        builder.Services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<DbContextOptions<WebbanhangDbContext>>();
            return new WebbanhangDbContext(options, encryptionKey);
        });

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddScoped<WebbanhangDbContextInitializer>();
    }
}