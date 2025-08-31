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
        public static async Task RunTestAsync()
        {
            Console.WriteLine("=== Stock Worker Service Test ===");
            Console.WriteLine();

            try
            {
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
                services.AddDbContext<WebbanhangDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

                // Add logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                // Add services
            services.AddScoped<StockCalculationService>();
            services.AddScoped<EmailNotificationService>();
            services.AddScoped<InventoryMovementService>();
            services.AddScoped<DataSeedService>();                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WebbanhangDbContext>();
                var stockService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
                var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeedService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TestRunner>>();

                // Test 1: Database connection
                Console.WriteLine("1. Testing database connection...");
                try
                {
                    var canConnect = await context.Database.CanConnectAsync();
                    if (canConnect)
                    {
                        Console.WriteLine("✓ Database connection successful");
                        
                        // Try to ensure database exists
                        await context.Database.EnsureCreatedAsync();
                        Console.WriteLine("✓ Database schema verified");
                    }
                    else
                    {
                        Console.WriteLine("✗ Cannot connect to database");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Database error: {ex.Message}");
                    return;
                }

                // Test 2: Check existing data
                Console.WriteLine("\n2. Checking existing data...");
                try
                {
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

                    // Seed data if needed
                    if (ingredientCount == 0)
                    {
                        Console.WriteLine("\n3. Seeding test data...");
                        try
                        {
                            await dataSeeder.SeedDataAsync();
                            Console.WriteLine("✓ All data seeded successfully");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"! Data seeding failed: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error checking data: {ex.Message}");
                }

                // Test 4: Basic stock calculations
                Console.WriteLine("\n4. Testing basic calculations...");
                try
                {
                    var lowStockAlerts = await stockService.GetLowStockAlertsAsync();
                    var outOfStockAlerts = await stockService.GetOutOfStockAlertsAsync();
                    Console.WriteLine($"✓ Stock alerts check completed: {lowStockAlerts.Count} low stock, {outOfStockAlerts.Count} out of stock");
                    
                    if (lowStockAlerts.Any())
                    {
                        Console.WriteLine("Low stock alerts:");
                        foreach (var alert in lowStockAlerts.Take(3))
                        {
                            Console.WriteLine($"  - {alert.Ingredient.Name} (ReorderPoint: {alert.ReorderPoint:N2})");
                        }
                    }
                    
                    if (outOfStockAlerts.Any())
                    {
                        Console.WriteLine("Out of stock alerts:");
                        foreach (var alert in outOfStockAlerts.Take(3))
                        {
                            Console.WriteLine($"  - {alert.Ingredient.Name} (MinStock: {alert.MinimumStock:N2})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error in calculations: {ex.Message}");
                }

                Console.WriteLine("\n=== Test completed ===");
                Console.WriteLine("\nTo run the worker service:");
                Console.WriteLine("dotnet run");
                Console.WriteLine("\nTo run worker in background:");
                Console.WriteLine("dotnet run > worker.log 2>&1 &");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
