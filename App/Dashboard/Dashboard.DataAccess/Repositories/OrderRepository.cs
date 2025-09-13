using Dashboard.Common.Utitlities;
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    new void Add(Order entity);
    Task<Order?> GetOrderByCodeAsync(string orderCode);
    Task<Dictionary<long, decimal>> GetProductPricesAsync(List<long> productIds);
}

public class OrderRepository(WebbanhangDbContext context) : Repository<Order>(context), IOrderRepository
{
    public new void Add(Order entity)
    {
        entity.OrderCode = GenerateOrderCodeAsync().Result;
        entity.OrderUuid = Guid.NewGuid().ToString();
        base.Add(entity);
    }

    private async Task<string> GenerateOrderCodeAsync()
    {
        return await CodeGenerator.GenerateCodeAsync("ORD", async (code) =>
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == code);
            return existingOrder != null;
        });
    }

    public async Task<Order?> GetOrderByCodeAsync(string orderCode)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
    }

    public async Task<Dictionary<long, decimal>> GetProductPricesAsync(List<long> productIds)
    {
        return await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => (long)p.Id, p => p.Price);
    }
}