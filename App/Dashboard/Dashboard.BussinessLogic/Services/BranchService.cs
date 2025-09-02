using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services;

public interface IBranchService
{
    Task<PagedList<BranchDto>> GetBranchesAsync(GetBranchesInput input);
}

public class BranchService : IBranchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<BranchDto>> GetBranchesAsync(GetBranchesInput input)
    {
        var spec = new Specification<Branch>(b => 
            (string.IsNullOrEmpty(input.Name) || b.Name.Contains(input.Name)) &&
            (string.IsNullOrEmpty(input.Address) || (b.Address != null && b.Address.Contains(input.Address))) &&
            (string.IsNullOrEmpty(input.Phone) || (b.Phone != null && b.Phone.Contains(input.Phone))) &&
            (string.IsNullOrEmpty(input.Manager) || (b.Manager != null && b.Manager.Contains(input.Manager)))
        );

        int skip = (input.PageNumber - 1) * input.PageSize;
        int take = input.PageSize;
        IEnumerable<Branch> branches = await _unitOfWork.Repository<Branch>().GetAllWithSpecAsync(spec, true, skip, take);
        int count = await _unitOfWork.Repository<Branch>().GetCountAsync();

        List<BranchDto> branchDtos = _mapper.Map<List<BranchDto>>(branches);

        return new PagedList<BranchDto>
        {
            Items = branchDtos,
            TotalRecords = count,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }
}
