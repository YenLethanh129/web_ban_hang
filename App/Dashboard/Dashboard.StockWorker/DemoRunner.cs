using Dashboard.DataAccess.Context;
using Dashboard.StockWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dashboard.StockWorker
{
    public static class DemoRunner
    {
        public static async Task RunDemoAsync()
        {
            Console.WriteLine("=== Stock Worker Service Demo ===");
            Console.WriteLine("Sử dụng In-Memory Database để demo chức năng");
            Console.WriteLine();

            // Setup services với In-Memory database
            var services = new ServiceCollection();
            
            services.AddLogging(builder => builder.AddConsole());
            services.AddDbContext<WebbanhangDbContext>(options =>
                options.UseInMemoryDatabase("DemoStockWorker"));
            
            services.AddScoped<StockCalculationService>();
            services.AddScoped<InventoryMovementService>();
            services.AddScoped<DataSeedService>();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebbanhangDbContext>();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeedService>();
            var stockService = scope.ServiceProvider.GetRequiredService<StockCalculationService>();
            var inventoryService = scope.ServiceProvider.GetRequiredService<InventoryMovementService>();

            // Demo 1: Seed data
            Console.WriteLine("1. Seeding demo data...");
            await dataSeeder.SeedDataAsync();
            Console.WriteLine("✓ Demo data seeded successfully");

            // Demo 2: Hiển thị categories và ingredients
            Console.WriteLine("\n2. Hiển thị dữ liệu đã seed:");
            var categories = await context.IngredientCategories.ToListAsync();
            Console.WriteLine($"- Categories: {categories.Count}");
            foreach (var cat in categories.Take(3))
            {
                Console.WriteLine($"  + {cat.Name}: {cat.Description}");
            }

            var ingredients = await context.Ingredients.Include(i => i.Category).ToListAsync();
            Console.WriteLine($"- Ingredients: {ingredients.Count}");
            foreach (var ing in ingredients.Take(5))
            {
                Console.WriteLine($"  + {ing.Name} ({ing.Unit}) - {ing.Category.Name}");
            }

            // Demo 3: Tạo inventory movement
            Console.WriteLine("\n3. Demo inventory movements:");
            var coffeeIngredient = ingredients.FirstOrDefault(i => i.Name.Contains("Cà phê"));
            if (coffeeIngredient != null)
            {
                // Nhập kho
                await inventoryService.CreateStockReceiptAsync(
                    branchId: 1,
                    ingredientId: coffeeIngredient.Id,
                    quantity: 100m,
                    unit: "kg",
                    purchaseOrderCode: "PO-001",
                    notes: "Nhập kho ban đầu"
                );
                Console.WriteLine($"✓ Nhập kho {coffeeIngredient.Name}: 100kg");

                // Xuất kho
                await inventoryService.CreateInventoryMovementAsync(
                    branchId: 1,
                    ingredientId: coffeeIngredient.Id,
                    movementType: "OUT",
                    quantity: 5m,
                    unit: "kg",
                    referenceType: "ORDER",
                    referenceCode: "ORD-001",
                    notes: "Tiêu thụ cho order"
                );
                Console.WriteLine($"✓ Xuất kho {coffeeIngredient.Name}: 5kg");

                // Kiểm tra tồn kho
                var currentStock = await inventoryService.GetCurrentStockAsync(1, coffeeIngredient.Id);
                Console.WriteLine($"✓ Tồn kho hiện tại: {currentStock}kg");
            }

            // Demo 4: Tính toán ROP
            Console.WriteLine("\n4. Demo stock calculations:");
            await stockService.CalculateAndUpdateReorderPointsAsync();
            Console.WriteLine("✓ Reorder points calculated");

            // Demo 5: Kiểm tra alerts
            Console.WriteLine("\n5. Checking stock alerts:");
            var lowStockAlerts = await stockService.GetLowStockAlertsAsync();
            var outOfStockAlerts = await stockService.GetOutOfStockAlertsAsync();
            
            Console.WriteLine($"- Low stock alerts: {lowStockAlerts.Count}");
            Console.WriteLine($"- Out of stock alerts: {outOfStockAlerts.Count}");

            // Demo 6: Hiển thị inventory movements
            Console.WriteLine("\n6. Inventory movement history:");
            var movements = await inventoryService.GetMovementHistoryAsync(branchId: 1);
            Console.WriteLine($"Total movements: {movements.Count}");
            foreach (var movement in movements.Take(3))
            {
                Console.WriteLine($"  - {movement.MovementType}: {movement.Quantity} {movement.Unit} " +
                                $"({movement.QuantityBefore} → {movement.QuantityAfter})");
            }

            Console.WriteLine("\n=== Demo completed successfully! ===");
            Console.WriteLine("Worker service có thể:");
            Console.WriteLine("- Seed dữ liệu café (categories, ingredients, recipes, thresholds)");
            Console.WriteLine("- Quản lý inventory movements (IN/OUT/TRANSFER/ADJUSTMENT)");
            Console.WriteLine("- Tính toán Reorder Points tự động");
            Console.WriteLine("- Phát hiện low stock và out of stock alerts");
            Console.WriteLine("- Theo dõi lịch sử xuất nhập kho");
        }
    }
}
