using Dashboard.Common.Options;
using Dashboard.DataAccess.Context;
using Dashboard.StockWorker;
using Dashboard.StockWorker.Services;
using Dashboard.StockWorker.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;

LoadEnvironmentVariables();

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<EmailOptions>(options =>
{
    var emailSection = builder.Configuration.GetSection("Email");
    emailSection.Bind(options);
});

builder.Services.PostConfigure<EmailOptions>(opts =>
{
    var raw = builder.Configuration["Email:AlertRecipients"];
    
    if (!string.IsNullOrWhiteSpace(raw))
    {
        opts.AlertRecipients = raw
            .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();
    }
});

builder.Services.Configure<StockWorkerOptions>(
    builder.Configuration.GetSection("StockWorker"));

builder.Services.Configure<FinancialReportingOptions>(
    builder.Configuration.GetSection("FinancialReporting"));

Dashboard.DataAccess.DependencyInjection.AddDataAccess(builder);
Dashboard.BussinessLogic.DependencyInjection.AddBussinessLogicServices(builder);

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    
    var connectionString = configuration.GetConnectionString("WebbanhangDB")
        ?? throw new InvalidOperationException("Connection string 'WebbanhangDB' is missing");
    
    var encryptionOptions = sp.GetRequiredService<IOptions<EncryptionOptions>>().Value;
    var encryptionKey = encryptionOptions?.Key
        ?? throw new InvalidOperationException("Encryption key is missing");
    
    var options = new DbContextOptionsBuilder<WebbanhangDbContext>()
        .UseSqlServer(connectionString)
        .Options;
    
    return new WebbanhangDbContext(options, encryptionKey);
});

builder.Services.AddScoped<StockCalculationService>();
builder.Services.AddScoped<PurchaseEnrichmentService>();
builder.Services.AddScoped<IFinancialReportTemplateService, FinancialReportTemplateService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<Dashboard.BussinessLogic.Services.ReportServices.IReportingService,
    Dashboard.BussinessLogic.Services.ReportServices.ReportingService>();

builder.Services.AddHostedService<StockAlertWorker>();
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
    var emailOptions = scope.ServiceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;
    var stockWorkerOptions = scope.ServiceProvider.GetRequiredService<IOptions<StockWorkerOptions>>().Value;
    var financialOptions = scope.ServiceProvider.GetRequiredService<IOptions<FinancialReportingOptions>>().Value;
    
    logger.LogInformation("========================================");
    logger.LogInformation("Stock Worker Service Starting...");
    logger.LogInformation("========================================");
    
    logger.LogInformation("Validating database connection...");
    if (!await context.Database.CanConnectAsync())
    {
        logger.LogError($"Cannot connect to database");
        throw new InvalidOperationException("Database connection failed");
    }
    logger.LogInformation("Database connection verified successfully");
    
    logger.LogInformation("Configuration loaded:");
    logger.LogInformation("- Stock Check Interval: {Minutes} minutes", stockWorkerOptions.CheckIntervalMinutes);
    logger.LogInformation("- Low Stock Threshold: {Threshold}", stockWorkerOptions.LowStockThreshold);
    logger.LogInformation("- Critical Stock Threshold: {Threshold}", stockWorkerOptions.CriticalStockThreshold);
    logger.LogInformation("- Financial Report Interval: {Minutes} minutes", financialOptions.IntervalMinutes);
    logger.LogInformation("- Financial Report Type: {Type}", financialOptions.ReportType);
    logger.LogInformation("- Financial Reporting Enabled: {Enabled}", financialOptions.Enabled);
    
    if (emailOptions.AlertRecipients == null || emailOptions.AlertRecipients.Length == 0)
    {
        logger.LogWarning("No alert recipients configured! Emails will not be sent.");
    }
    else
    {
        logger.LogInformation("Alert recipients configured: {Count} recipient(s)", 
            emailOptions.AlertRecipients.Length);
        
        foreach (var recipient in emailOptions.AlertRecipients)
        {
            logger.LogInformation("  - {Recipient}", recipient);
        }
    }
    
    if (string.IsNullOrEmpty(emailOptions.Smtp?.Host) && string.IsNullOrEmpty(emailOptions.SmtpHost))
    {
        logger.LogWarning("SMTP Host not configured!");
    }
    else
    {
        var smtpHost = emailOptions.Smtp?.Host ?? emailOptions.SmtpHost;
        var smtpPort = emailOptions.Smtp?.Port ?? emailOptions.SmtpPort;
        logger.LogInformation("SMTP configured: {Host}:{Port}", smtpHost, smtpPort);
    }
    
    if (emailOptions.DryRun)
    {
        logger.LogWarning("EMAIL DRY RUN MODE ENABLED - No emails will be sent!");
    }
    
    if (emailOptions.UsePickupDirectory)
    {
        logger.LogInformation("Using pickup directory: {Directory}", emailOptions.PickupDirectory);
    }
    
    logger.LogInformation("========================================");
    logger.LogInformation("Stock Worker Service Started Successfully");
    logger.LogInformation("========================================");
}

host.Run();

static void LoadEnvironmentVariables()
{
    try
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        
        if (File.Exists(envPath))
        {
            DotNetEnv.Env.Load(envPath);
            Console.WriteLine($"Loaded .env from: {envPath}");
        }
        else
        {
            var parentEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
            if (File.Exists(parentEnvPath))
            {
                DotNetEnv.Env.Load(parentEnvPath);
                Console.WriteLine($"Loaded .env from: {parentEnvPath}");
            }
            else
            {
                Console.WriteLine("No .env file found. Using system environment variables only.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading .env file: {ex.Message}");
        Console.WriteLine("Continuing with system environment variables...");
    }
}