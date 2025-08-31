using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class DataSeedService
    {
        private readonly WebbanhangDbContext _context;

        public DataSeedService(WebbanhangDbContext context)
        {
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            Console.WriteLine("Starting seeding data...");

            await SeedCafeIngredientsAsync();
            await SeedCafeRecipesAsync();
            await SeedSampleBranchAsync();
            await SeedInventoryThresholdsAsync();

            Console.WriteLine("Seeding data  completed!");
        }

        private async Task SeedCafeIngredientsAsync()
        {


            try
            {
                var categories = new List<IngredientCategory>
                {
                    new() { Name = "Cà phê", Description = "Các loại cà phê", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Sữa", Description = "Các loại sữa", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Đường & Syrup", Description = "Đường và các loại syrup", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Bột pha chế", Description = "Các loại bột pha chế", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Trái cây", Description = "Trái cây tươi", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Topping", Description = "Các loại topping", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Nước", Description = "Nước uống", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { Name = "Bao bì", Description = "Ly, ống hút, nắp", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow }
                };

                _context.IngredientCategories.AddRange(categories);
                await _context.SaveChangesAsync();

                var coffeeCategory = categories.First(c => c.Name == "Cà phê");
                var milkCategory = categories.First(c => c.Name == "Sữa");
                var sugarCategory = categories.First(c => c.Name == "Đường & Syrup");
                var powderCategory = categories.First(c => c.Name == "Bột pha chế");
                var fruitCategory = categories.First(c => c.Name == "Trái cây");
                var toppingCategory = categories.First(c => c.Name == "Topping");
                var waterCategory = categories.First(c => c.Name == "Nước");
                var packagingCategory = categories.First(c => c.Name == "Bao bì");

                var ingredients = new List<Ingredient>
                {
                    new() { CategoryId = coffeeCategory.Id, Name = "Cà phê Arabica", Unit = "kg", IsActive = true, Description = "Cà phê Arabica cao cấp", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = coffeeCategory.Id, Name = "Cà phê Robusta", Unit = "kg", IsActive = true, Description = "Cà phê Robusta", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = coffeeCategory.Id, Name = "Espresso", Unit = "kg", IsActive = true, Description = "Cà phê espresso", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = coffeeCategory.Id, Name = "Cà phê phin", Unit = "kg", IsActive = true, Description = "Cà phê phin truyền thống", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
 
                    new() { CategoryId = milkCategory.Id, Name = "Sữa tươi", Unit = "lít", IsActive = true, Description = "Sữa tươi nguyên chất", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = milkCategory.Id, Name = "Sữa đặc", Unit = "hộp", IsActive = true, Description = "Sữa đặc có đường", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = milkCategory.Id, Name = "Kem tươi", Unit = "hộp", IsActive = true, Description = "Kem tươi whipping", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = milkCategory.Id, Name = "Sữa không đường", Unit = "lít", IsActive = true, Description = "Sữa tươi không đường", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = sugarCategory.Id, Name = "Đường trắng", Unit = "kg", IsActive = true, Description = "Đường trắng tinh luyện", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = sugarCategory.Id, Name = "Đường nâu", Unit = "kg", IsActive = true, Description = "Đường nâu tự nhiên", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = sugarCategory.Id, Name = "Syrup Vanilla", Unit = "chai", IsActive = true, Description = "Syrup vani", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = sugarCategory.Id, Name = "Syrup Caramel", Unit = "chai", IsActive = true, Description = "Syrup caramel", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = sugarCategory.Id, Name = "Mật ong", Unit = "kg", IsActive = true, Description = "Mật ong thiên nhiên", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = powderCategory.Id, Name = "Bột cacao", Unit = "kg", IsActive = true, Description = "Bột cacao nguyên chất", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = powderCategory.Id, Name = "Bột matcha", Unit = "kg", IsActive = true, Description = "Bột trà xanh matcha", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = powderCategory.Id, Name = "Bột taro", Unit = "kg", IsActive = true, Description = "Bột khoai môn", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = fruitCategory.Id, Name = "Chanh tươi", Unit = "kg", IsActive = true, Description = "Chanh tươi", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = fruitCategory.Id, Name = "Cam tươi", Unit = "kg", IsActive = true, Description = "Cam tươi", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = toppingCategory.Id, Name = "Trân châu đen", Unit = "kg", IsActive = true, Description = "Trân châu đen", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = toppingCategory.Id, Name = "Trân châu trắng", Unit = "kg", IsActive = true, Description = "Trân châu trắng", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = waterCategory.Id, Name = "Nước lọc", Unit = "lít", IsActive = true, Description = "Nước lọc tinh khiết", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = waterCategory.Id, Name = "Đá viên", Unit = "kg", IsActive = true, Description = "Đá viên", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },

                    new() { CategoryId = packagingCategory.Id, Name = "Ly nhựa 500ml", Unit = "cái", IsActive = true, Description = "Ly nhựa trong suốt 500ml", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = packagingCategory.Id, Name = "Ống hút nhựa", Unit = "cái", IsActive = true, Description = "Ống hút nhựa", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                    new() { CategoryId = packagingCategory.Id, Name = "Nắp ly", Unit = "cái", IsActive = true, Description = "Nắp ly nhựa", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow }
                };

                _context.Ingredients.AddRange(ingredients);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while executing the seeding phase: {ex.Message}");
                throw;
            }
        }

        private async Task SeedCafeRecipesAsync()
        {

            try
            {
                var firstProduct = await _context.Products.FirstAsync();
                var recipe = new Recipe
                {
                    Name = "Cà phê sữa đá",
                    Description = "Công thức pha chế cà phê sữa đá truyền thống",
                    ProductId = firstProduct.Id,
                    ServingSize = 1,
                    Unit = "ly",
                    IsActive = true,
                    Notes = "Pha chế theo tỷ lệ chuẩn",
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                var coffeeIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Cà phê phin");
                var milkIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Sữa đặc");
                var iceIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Đá viên");
                var waterIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Nước lọc");
                var cupIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Ly nhựa 500ml");

                if (coffeeIngredient != null && milkIngredient != null && iceIngredient != null && waterIngredient != null && cupIngredient != null)
                {
                    var recipeIngredients = new List<RecipeIngredient>
                    {
                        new() { RecipeId = recipe.Id, IngredientId = coffeeIngredient.Id, Quantity = 0.02m, Unit = "kg", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                        new() { RecipeId = recipe.Id, IngredientId = milkIngredient.Id, Quantity = 0.03m, Unit = "hộp", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                        new() { RecipeId = recipe.Id, IngredientId = iceIngredient.Id, Quantity = 0.1m, Unit = "kg", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                        new() { RecipeId = recipe.Id, IngredientId = waterIngredient.Id, Quantity = 0.15m, Unit = "lít", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow },
                        new() { RecipeId = recipe.Id, IngredientId = cupIngredient.Id, Quantity = 1, Unit = "cái", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow }
                    };

                    _context.RecipeIngredients.AddRange(recipeIngredients);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Completed seeding recipes  with {recipeIngredients.Count} ingredients.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured whiling executing the seeding recipes phase: {ex.Message}");
                throw;
            }
        }

        private async Task SeedSampleBranchAsync()
        {
            if (await _context.Branches.AnyAsync())
            {
                Console.WriteLine("Branches already exist, skip seeding branches.");
                return;
            }

            try
            {
                var branch = new Branch
                {
                    Name = "Chi nhánh Demo",
                    Address = "123 Nguyễn Văn Linh, Quận 7, TP.HCM",
                    Phone = "0901234567",
                    Manager = "Nguyễn Văn A",
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.Branches.Add(branch);
                await _context.SaveChangesAsync();

                Console.WriteLine("Seeding sample completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured whiling executing the seeding branch phase: {ex.Message}");
                throw;
            }
        }

        private async Task SeedInventoryThresholdsAsync()
        {
            if (await _context.InventoryThresholds.AnyAsync())
            {
                Console.WriteLine("Inventory thresholds already existed.");
                return;
            }

            try
            {
                var coffeeIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Cà phê phin");
                var milkIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Sữa đặc");
                var firstBranch = await _context.Branches.FirstAsync();

                if (coffeeIngredient != null && milkIngredient != null)
                {
                    var thresholds = new List<InventoryThreshold>
                    {
                        new()
                        {
                            IngredientId = coffeeIngredient.Id,
                            BranchId = firstBranch.Id,
                            MinimumStock = 5m,
                            ReorderPoint = 10m,
                            MaximumStock = 50m,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow
                        },
                        new()
                        {
                            IngredientId = milkIngredient.Id,
                            BranchId = firstBranch.Id,
                            MinimumStock = 10m,
                            ReorderPoint = 20m,
                            MaximumStock = 100m,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow
                        }
                    };

                    _context.InventoryThresholds.AddRange(thresholds);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"{thresholds.Count} inventory thresholds were seeded for branch {firstBranch.Name}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured whiling executing the seeding inventory thresholds phase: {ex.Message}");
                throw;
            }
        }
    }
}
