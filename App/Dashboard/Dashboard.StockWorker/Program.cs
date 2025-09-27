using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.StockWorker;
using Dashboard.StockWorker.Services;
using Dashboard.StockWorker.Workers;
using Dashboard.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<StockWorkerOptions>(builder.Configuration.GetSection("StockWorker"));
builder.Services.Configure<FinancialReportingOptions>(builder.Configuration.GetSection("FinancialReporting"));
builder.Services.Configure<EmailSimpleOptions>(builder.Configuration.GetSection("EmailSimple"));


builder.Services.PostConfigure<EmailOptions>(opts =>
{
    var raw = builder.Configuration["Email:AlertRecipients"];

    if (!string.IsNullOrWhiteSpace(raw))
    {
        opts.AlertRecipients = raw
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();
    }

});

Dashboard.DataAccess.DependencyInjection.AddDataAccess(builder);
Dashboard.BussinessLogic.DependencyInjection.AddBussinessLogicServices(builder);

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("WebbanhangDB")
        ?? throw new InvalidOperationException("Connection string is missing");

    var encryptionOptions = sp.GetRequiredService<IOptions<EncryptionOptions>>().Value;
    var encryptionKey = encryptionOptions?.Key ?? throw new InvalidOperationException("Encryption key is missing");

    var options = new DbContextOptionsBuilder<WebbanhangDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    return new WebbanhangDbContext(options, encryptionKey);
});

// Services
builder.Services.AddScoped<StockCalculationService>();
builder.Services.AddScoped<LowStockReportTemplateService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<PurchaseEnrichmentService>();
builder.Services.AddScoped<IFinancialReportTemplateService, FinancialReportTemplateService>();

builder.Services.AddScoped<INotificationService>(sp =>
{
    var emailOptions = sp.GetRequiredService<IOptions<EmailOptions>>().Value;
    var useAdvancedEmail = emailOptions?.UseAdvancedNotifications ?? true;

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
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connFromContext = context.Database.GetDbConnection().ConnectionString;

    logger.LogInformation("Checking database connection...");
    if (!await context.Database.CanConnectAsync())
    {
        logger.LogError($"Cannot connect to database. Please check connection string: {connFromContext}");
        throw new InvalidOperationException("Database connection failed");
    }
    logger.LogInformation("Database connection verified successfully");
}

host.Run();