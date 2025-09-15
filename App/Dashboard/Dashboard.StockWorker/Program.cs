using Dashboard.DataAccess.Context;
using Dashboard.StockWorker;
using Dashboard.StockWorker.Services;
using Microsoft.EntityFrameworkCore;

// Check if running in test mode
if (args.Length > 0 && args[0].ToLower() == "test")
{
    // Support optional flags: --no-seed and --send-email-test
    var skipSeed = args.Contains("--no-seed");
    var sendEmailTest = args.Contains("--send-email-test");
    await TestRunner.RunTestAsync(skipSeed, sendEmailTest);
    return;
}

// Check if running in demo mode
if (args.Length > 0 && args[0].ToLower() == "demo")
{
    await DemoRunner.RunDemoAsync();
    return;
}
var builder = Host.CreateApplicationBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();

// Add DbContext
builder.Services.AddDbContext<WebbanhangDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=localhost;Database=webbanHang;Trusted_Connection=true;TrustServerCertificate=true;";
    options.UseSqlServer(connectionString);
});

// Add services
builder.Services.AddScoped<StockCalculationService>();
builder.Services.AddScoped<EmailNotificationService>();
builder.Services.AddScoped<InventoryMovementService>();
builder.Services.AddScoped<DataSeedService>();
builder.Services.AddScoped<PurchaseEnrichmentService>();
// register notification interface
builder.Services.AddScoped<INotificationService, EmailNotificationService>();

// Add the worker service
builder.Services.AddHostedService<Worker>();
// register low stock alert background worker
builder.Services.AddHostedService<LowStockAlertWorker>();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var host = builder.Build();

// Ensure database is created
using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WebbanhangDbContext>();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeedService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("Checking database connection...");
    var canConnect = await context.Database.CanConnectAsync();
    
    if (canConnect)
    {
        await context.Database.EnsureCreatedAsync();
        logger.LogInformation("Database connection verified successfully");
        
        // await dataSeeder.SeedDataAsync();
        logger.LogInformation("Initial data seeding completed");
    }
    else
    {
        logger.LogError("Cannot connect to database. Please check connection string.");
        throw new InvalidOperationException("Database connection failed");
    }
}
Console.WriteLine("Stock Worker Service is starting...");
Console.WriteLine("Press Ctrl+C to stop the service");
Console.WriteLine("To run test mode: dotnet run test");

host.Run();
