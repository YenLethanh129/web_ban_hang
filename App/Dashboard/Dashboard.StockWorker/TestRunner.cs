using Dashboard.DataAccess.Context;
using Dashboard.StockWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dashboard.StockWorker
{
    public class TestRunner
    {
    public static async Task RunTestAsync(bool skipSeed = false, bool sendEmailTest = false)
        {
            Console.WriteLine("=== Stock Worker Service Test ===");
            Console.WriteLine();

            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Setup services
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=webbanHang;Trusted_Connection=true;TrustServerCertificate=true;";

            // WebbanhangDbContext requires an encryption key in its constructor; register with factory
            services.AddScoped<WebbanhangDbContext>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<WebbanhangDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                var encryptionKey = configuration["EncryptionKey"] ?? configuration["DbEncryptionKey"] ?? string.Empty;
                return new WebbanhangDbContext(optionsBuilder.Options, encryptionKey);
            });

            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // Add services
            services.AddScoped<StockCalculationService>();
            services.AddScoped<PurchaseEnrichmentService>();
            services.AddScoped<EmailNotificationService>();
            services.AddScoped<INotificationService>(sp => sp.GetRequiredService<EmailNotificationService>());
            services.AddScoped<InventoryMovementService>();
            services.AddScoped<DataSeedService>();
            
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebbanhangDbContext>();
            var stockService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeedService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TestRunner>>();
            var notifier = scope.ServiceProvider.GetRequiredService<INotificationService>();

            // Test 1: Database connection
            Console.WriteLine("1. Testing database connection...");
            var canConnect = await context.Database.CanConnectAsync();
            if (canConnect)
            {
                Console.WriteLine("✓ Database connection successful");

                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("✓ Database schema verified");
            }
            else
            {
                Console.WriteLine("✗ Cannot connect to database");
                return;
            }

            // Test 2: Check existing data
            Console.WriteLine("\n2. Checking existing data...");
            var ingredientCount = await context.Ingredients.CountAsync();
            var categoryCount = await context.IngredientCategories.CountAsync();
            var recipeCount = await context.Recipes.CountAsync();
            var thresholdCount = await context.InventoryThresholds.CountAsync();
            var inventoryCount = await context.BranchIngredientInventories.CountAsync();

            Console.WriteLine($"  - Ingredient Categories: {categoryCount}");
            Console.WriteLine($"  - Ingredients: {ingredientCount}");
            Console.WriteLine($"  - Recipes: {recipeCount}");
            Console.WriteLine($"  - Inventory Thresholds: {thresholdCount}");
            Console.WriteLine($"  - Branch Inventories: {inventoryCount}");

            // Seed data if needed (unless user asked to skip)
            if (!skipSeed)
            {
                if (ingredientCount == 0)
                {
                    Console.WriteLine("\n3. Seeding test data...");
                    await dataSeeder.SeedDataAsync();
                    Console.WriteLine("✓ All data seeded successfully");
                }
            }
            else
            {
                Console.WriteLine("\nSkipping data seeding because skipSeed=true");
            }

            // Test 4: Basic stock calculations
            Console.WriteLine("\n4. Testing basic calculations...");
            List<Dashboard.StockWorker.Models.StockAlert> lowStockAlerts = new();
            List<Dashboard.StockWorker.Models.StockAlert> outOfStockAlerts = new();

            try
            {
                lowStockAlerts = await stockService.GetLowStockAlertsAsync();
                outOfStockAlerts = await stockService.GetOutOfStockAlertsAsync();
                Console.WriteLine($"✓ Stock alerts check completed: {lowStockAlerts.Count} low stock, {outOfStockAlerts.Count} out of stock");

                if (lowStockAlerts.Any())
                {
                    Console.WriteLine("Low stock alerts:");
                    foreach (var alert in lowStockAlerts.Take(3))
                    {
                        Console.WriteLine($"  - {alert.IngredientName} (ReorderPoint: {alert.ReorderPoint:N2})");
                    }
                }

                if (outOfStockAlerts.Any())
                {
                    Console.WriteLine("Out of stock alerts:");
                    foreach (var alert in outOfStockAlerts.Take(3))
                    {
                        Console.WriteLine($"  - {alert.IngredientName} (MinStock: {alert.SafetyStock:N2})");
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                    Console.WriteLine($"Warning: could not run stock calculations due to database schema mismatch: {sqlEx.Message}");
                    // Print full exception (includes SQL text in many cases)
                    Console.WriteLine(sqlEx.ToString());
                    Console.WriteLine("If you want full calculation tests, either run with seeded data or migrate your DB to the expected schema.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: error while running stock calculations: {ex.Message}");
            }

            Console.WriteLine("\n=== Test completed ===");
            // Optional: send a test email with sample alerts
            if (sendEmailTest)
            {
                Console.WriteLine("\n5. Sending test emails (using INotificationService)...");

                var sampleAlerts = new List<Dashboard.StockWorker.Models.StockAlert>
                {
                    new Dashboard.StockWorker.Models.StockAlert
                    {
                        BranchId = 1,
                        BranchName = "Chi nhánh A",
                        IngredientId = 1,
                        IngredientName = "Muối",
                        CurrentStock = 5m,
                        Unit = "kg",
                        ReorderPoint = 10m,
                        SafetyStock = 2m,
                        AverageDailyConsumption = 1.2m,
                        DaysRemaining = 4,
                        AlertLevel = Dashboard.StockWorker.Models.StockAlertLevel.Low
                    },
                    new Dashboard.StockWorker.Models.StockAlert
                    {
                        BranchId = 1,
                        BranchName = "Chi nhánh A",
                        IngredientId = 2,
                        IngredientName = "Đường",
                        CurrentStock = 1m,
                        Unit = "kg",
                        ReorderPoint = 5m,
                        SafetyStock = 1m,
                        AverageDailyConsumption = 1.5m,
                        DaysRemaining = 1,
                        AlertLevel = Dashboard.StockWorker.Models.StockAlertLevel.Critical
                    },
                    new Dashboard.StockWorker.Models.StockAlert
                    {
                        BranchId = 2,
                        BranchName = "Chi nhánh B",
                        IngredientId = 3,
                        IngredientName = "Bột mì",
                        CurrentStock = 0m,
                        Unit = "kg",
                        ReorderPoint = 8m,
                        SafetyStock = 2m,
                        AverageDailyConsumption = 2.0m,
                        DaysRemaining = 0,
                        AlertLevel = Dashboard.StockWorker.Models.StockAlertLevel.OutOfStock
                    }
                };

                try
                {
                    await notifier.SendStockAlertsAsync(sampleAlerts);
                    Console.WriteLine("Test emails processed (check SMTP logs / inboxes if configured)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while sending test emails: " + ex);
                }
            }
            Console.WriteLine("\nTo run the worker service:");
            Console.WriteLine("dotnet run");
            Console.WriteLine("\nTo run worker in background:");
            Console.WriteLine("dotnet run > worker.log 2>&1 &");
        }
    }
}
