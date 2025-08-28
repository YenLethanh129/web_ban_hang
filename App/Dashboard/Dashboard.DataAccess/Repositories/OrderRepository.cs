using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    //Task<Order?> GetOrderWithDetailsAsync(int id);
    //Task<List<Order>> GetOrdersWithDetailsAsync(ISpecification<Order> spec);
    //Task<Order?> GetOrderByCodeAsync(string orderCode);
    //Task<string> GenerateOrderCodeAsync();
    //Task<Dictionary<int, decimal>> GetProductPricesAsync(List<int> productIds);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(WebbanhangDbContext context) : base(context)
    {
    }

    
}