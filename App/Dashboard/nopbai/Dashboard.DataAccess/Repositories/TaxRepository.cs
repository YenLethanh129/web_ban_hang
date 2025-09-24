using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities.FinacialAndReports;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Repositories;

public interface ITaxRepository : IRepository<Taxes>
{
    Task<Taxes?> GetByNameAsync(string name);
    Task<List<Taxes>> GetActiveTaxesAsync();
}

public class TaxRepository : Repository<Taxes>, ITaxRepository
{
    public TaxRepository(WebbanhangDbContext context) : base(context)
    {
    }

    public async Task<Taxes?> GetByNameAsync(string name)
    {
        return await _context.Taxes
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<List<Taxes>> GetActiveTaxesAsync()
    {
        return await _context.Taxes
            //.Where(t => !t.IsActive)  // TODO Make soft delete later 
            .ToListAsync();
    }
}