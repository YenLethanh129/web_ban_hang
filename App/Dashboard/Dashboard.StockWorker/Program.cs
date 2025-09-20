using Dashboard.DataAccess.Context;
using Dashboard.StockWorker;
using Dashboard.StockWorker.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Load configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Register DataAccess & BusinessLogic
Dashboard.DataAccess.DependencyInjection.AddDataAccess(builder);
Dashboard.BussinessLogic.DependencyInjection.AddBussinessLogicServices(builder);

// Register DbContext
builder.Services.AddScoped<WebbanhangDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string is missing");
    var encryptionKey = configuration["Encryption:Key"] ?? 
                        throw new InvalidOperationException("Encryption key is missing");

    var options = new DbContextOptionsBuilder<WebbanhangDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    return new WebbanhangDbContext(options, encryptionKey);
});

// Register services
builder.Services.AddScoped<StockCalculationService>();
builder.Services.AddScoped<EmailNotificationService>();
builder.Services.AddScoped<InventoryMovementService>();
builder.Services.AddScoped<DataSeedService>();
builder.Services.AddScoped<PurchaseEnrichmentService>();
builder.Services.AddScoped<FinancialReportService>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
builder.Services.AddScoped<Dashboard.BussinessLogic.Services.ReportServices.IReportingService,
    Dashboard.BussinessLogic.Services.ReportServices.ReportingService>();

// Register workers
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<LowStockAlertWorker>();

// Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var host = builder.Build();

// Database check
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
