using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsWithImagesAsync();
    Task<Product?> GetProductWithDetailsAsync(long id);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(long categoryId);
    Task<bool> IsProductNameExistsAsync(string name, long? excludeId = null);
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsWithImagesAsync()
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithDetailsAsync(long id)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .Include(p => p.ProductRecipes)
                .ThenInclude(pr => pr.Ingredient)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(long categoryId)
    {
        return await _context.Products
            .Include(p => p.ProductImages)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<bool> IsProductNameExistsAsync(string name, long? excludeId = null)
    {
        var query = _context.Products.Where(p => p.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

}