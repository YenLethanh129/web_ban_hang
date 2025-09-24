using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.Customers;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
    Task<Customer?> GetByEmailOrPhone(string findStr);
}
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(WebbanhangDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive())
            .ToListAsync();
    }

    public async Task<Customer?> GetByEmailOrPhone(string findStr)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == findStr || c.PhoneNumber == findStr);
    }
    
}


       
