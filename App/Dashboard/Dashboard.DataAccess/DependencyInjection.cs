using Ardalis.GuardClauses;
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Data.Interceptors;
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
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<WebbanhangDbContext>((sp, options)=>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(Context);
        });

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddScoped<WebbanhangDbContextInitializer>();
    }
}

