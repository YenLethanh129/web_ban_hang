using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.DataAccess.Context;
using Dashboard.DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BussinessLogic.Services;

public interface IBranchService
{
    Task<PagedList<BranchDto>> GetBranchesAsync(GetBranchesInput input);
}

public class BranchService : IBranchService
{
    private readonly WebbanhangDbContext _context;

    public BranchService(WebbanhangDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<BranchDto>> GetBranchesAsync(GetBranchesInput input)
    {
        IQueryable<Branch> query = _context.Branches.AsQueryable();

        if (!string.IsNullOrEmpty(input.Name))
        {
            query = query.Where(x => x.Name.Contains(input.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(input.Address))
        {
            query = query.Where(x => x.Address != null && x.Address.Contains(input.Address, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(input.Phone))
        {
            query = query.Where(x => x.Phone != null && x.Phone.Contains(input.Phone, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(input.Manager))
        {
            query = query.Where(x => x.Manager != null && x.Manager.Contains(input.Manager, StringComparison.CurrentCultureIgnoreCase));
        }

        int totalRecords = await query.CountAsync();
        List<BranchDto> branchDtos = await query
            .Select(x => new BranchDto
            {
                Name = x.Name,
                Address = x.Address,
                Manager = x.Manager,
                Phone = x.Phone
            })
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToListAsync();

        return new PagedList<BranchDto>
        {
            PageNumber = input.PageNumber,
            PageSize = input.PageSize,
            TotalRecords = totalRecords,
            Items = branchDtos
        };
    }
}
