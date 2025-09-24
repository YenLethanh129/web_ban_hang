using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.StockWorker;
using Dashboard.StockWorker.Services;
using Dashboard.StockWorker.Workers;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

Dashboard.DataAccess.DependencyInjection.AddDataAccess(builder);
Dashboard.BussinessLogic.DependencyInjection.AddBussinessLogicServices(builder);

builder.Services.AddScoped<WebbanhangDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("WebbanhangDB")
        ?? throw new InvalidOperationException("Connection string is missing");
    var encryptionKey = configuration["Encryption:Key"] ??
                        throw new InvalidOperationException("Encryption key is missing");

    var options = new DbContextOptionsBuilder<WebbanhangDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    return new WebbanhangDbContext(options, encryptionKey);
});

builder.Services.AddScoped<StockCalculationService>();
builder.Services.AddScoped<LowStockReportTemplateService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<PurchaseEnrichmentService>();
builder.Services.AddScoped<IFinancialReportTemplateService, FinancialReportTemplateService>();

builder.Services.AddScoped<INotificationService>(sp =>
{
    var useAdvancedEmail = sp.GetRequiredService<IConfiguration>()
        .GetValue<bool>("Email:UseAdvancedNotifications", true);

    return useAdvancedEmail
        ? sp.GetRequiredService<LowStockReportTemplateService>()
        : sp.GetRequiredService<NotificationService>();
});

builder.Services.AddScoped<Dashboard.BussinessLogic.Services.ReportServices.IReportingService,
    Dashboard.BussinessLogic.Services.ReportServices.ReportingService>();

builder.Services.AddHostedService<LowStockAlertWorker>();
builder.Services.AddHostedService<FinancialReportingWorker>();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WebbanhangDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Checking database connection...");
    if (!await context.Database.CanConnectAsync())
    {
        logger.LogError("Cannot connect to database. Please check connection string.");
        throw new InvalidOperationException("Database connection failed");
    }
    logger.LogInformation("Database connection verified successfully");
}

host.Run();