using Dashboard.Common.Utitlities;
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    new void Add(Order entity);
    void Update(Order order);
    Task<Order?> GetOrderWithDetailsAsync(long id);
    Task<List<Order>> GetOrdersWithDetailsAsync(ISpecification<Order> spec);
    Task<Order?> GetOrderByCode(string orderCode);
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
    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }
    private async Task<string> GenerateOrderCodeAsync()
    {
        return await CodeGenerator.GenerateCodeAsync("ORD", async (code) =>
        {
            var spec = new Specification<Order>(o => o.OrderCode == code);
            var existingOrder = await GetWithSpecAsync(spec, true);
            return existingOrder != null;
        });
    }

    public Task<Order?> GetOrderByCode(string orderCode)
    {
        var spec = new Specification<Order>(o => o.OrderCode == orderCode);
        return GetWithSpecAsync(spec, true);
    }

    public Task<List<Order>> GetOrdersWithDetailsAsync(ISpecification<Order> spec)
    {
        spec.Includes.Add(o => o.Customer!);
        spec.Includes.Add(o => o.Branch!);
        spec.Includes.Add(o => o.Status!);
        spec.Includes.Add(o => o.OrderDetails);
        spec.Includes.Add(o => o.OrderPayments);
        spec.Includes.Add(o => o.OrderShipments);
        spec.Includes.Add(o => o.OrderDeliveryTrackings);
        return GetAllWithSpecAsync(spec, true).ContinueWith(t => t.Result.ToList());
    }

    public Task<Order?> GetOrderWithDetailsAsync(long id)
    {
        var spec = new Specification<Order>(o => o.Id == id);
        spec.Includes.Add(o => o.Customer!);
        spec.Includes.Add(o => o.Branch!);
        spec.Includes.Add(o => o.Status!);
        spec.Includes.Add(o => o.OrderDetails);
        spec.Includes.Add(o => o.OrderPayments);
        spec.Includes.Add(o => o.OrderShipments);
        spec.Includes.Add(o => o.OrderDeliveryTrackings);
        return GetWithSpecAsync(spec, true);
    }

    public Task<Dictionary<long, decimal>> GetProductPricesAsync(List<long> productIds)
    {
        return _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => (long)p.Id, p => p.Price);
    }
}