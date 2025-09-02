using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.OrderDtos;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services;

public interface IOrderService
{
    Task<PagedList<OrderDto>> GetOrdersAsync(GetOrdersInput input);
    Task<OrderDto?> GetOrderByIdAsync(long id);
    Task<OrderDto> CreateOrderAsync(CreateOrderInput input);
    Task<OrderDto> UpdateOrderAsync(long id, UpdateOrderInput input);
    Task DeleteOrderAsync(long id);
    Task<OrderSummaryDto> GetOrderSummaryAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(DateTime date, long? branchId = null);
    Task<IEnumerable<BranchOrderSummary>> GetBranchOrderSummaryAsync(DateTime fromDate, DateTime toDate);
}

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IBranchRepository branchRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(GetOrdersInput input)
    {
        var specification = new Specification<Order>(o =>
            (string.IsNullOrEmpty(input.OrderCode) || o.OrderCode.Contains(input.OrderCode)) &&
            (!input.CustomerId.HasValue || o.CustomerId == input.CustomerId.Value) &&
            (!input.BranchId.HasValue || o.BranchId == input.BranchId.Value) &&
            (!input.FromDate.HasValue || o.CreatedAt >= input.FromDate.Value) &&
            (!input.ToDate.HasValue || o.CreatedAt.Date <= input.ToDate.Value) &&
            (!input.MinAmount.HasValue || o.TotalMoney >= input.MinAmount.Value) &&
            (!input.MaxAmount.HasValue || o.TotalMoney <= input.MaxAmount.Value)
        );
        specification.Includes.Add(o => o.Customer!);
        specification.Includes.Add(o => o.Branch!);
        specification.Includes.Add(o => o.Status!);
        var allOrders = await _orderRepository.GetAllWithSpecAsync(specification, true);
        var totalCount = allOrders.Count();

        IEnumerable<Order> sortedProducts = allOrders;
        if (input.SortBy.HasValue)
        {
            sortedProducts = input.SortBy switch
            {
                SortByEnum.CreatedDate => input.IsDescending
                                            ? allOrders.OrderByDescending(p => p.CreatedAt)
                                            : allOrders.OrderBy(p => p.CreatedAt),
                _ => allOrders.OrderByDescending(p => p.CreatedAt),
            };
        }
        else
        {
            sortedProducts = allOrders.OrderByDescending(p => p.CreatedAt);
        }


        var pagedOrders = allOrders
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var orderDtos = _mapper.Map<List<OrderDto>>(pagedOrders);

        return new PagedList<OrderDto>
        {
            Items = orderDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }
    public async Task<OrderDto?> GetOrderByIdAsync(long id)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(id);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderInput input)
    {
        var order = _mapper.Map<Order>(input);
        order.CreatedAt = DateTime.Now;
        order.StatusId = 1;

        var createdOrder = await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderDto>(createdOrder);
    }

    public async Task<OrderDto> UpdateOrderAsync(long id, UpdateOrderInput input)
    {
        var order = await _orderRepository.GetAsync(id) ?? throw new ArgumentException($"Order with id {id} not found");
        _mapper.Map(input, order);

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderDto>(order);
    }
    public async Task DeleteOrderAsync(long id)
    {
        var order = await _orderRepository.GetAsync(id) ?? throw new ArgumentException($"Order with id {id} not found");
        _orderRepository.Remove(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(DateTime fromDate, long? branchId = null)
    {
        var specification = new Specification<Order>(o =>
            o.CreatedAt.Date == fromDate.Date &&
            o.Status!.Id == (long)OrderStatusEnum.Delivered &&
            (!branchId.HasValue || o.BranchId == branchId.Value)
        );
        specification.Includes.Add(o => o.Branch!);
        
        var orders = await _orderRepository.GetAllWithSpecAsync(specification, true);
        var dailySummary = orders
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new DailyOrderSummary(
                g.Key,
                g.Count(),
                g.Average(o => o.TotalMoney ?? 0),
                g.Sum(o => o.TotalMoney ?? 0)
            ))
            .OrderBy(s => s.Date)
            .ToList();

        return dailySummary;
    }

    public async Task<IEnumerable<BranchOrderSummary>> GetBranchOrderSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var specification = new Specification<Order>(o =>
            o.CreatedAt.Date >= fromDate.Date &&
            o.CreatedAt.Date <= toDate.Date &&
            o.Status!.Id == (int)OrderStatusEnum.Delivered);

        var orders = await _orderRepository.GetAllWithSpecAsync(specification, true);
        var branches = await _branchRepository.GetAllAsync();

        var branchSummary = branches
            .GroupJoin(orders,
                b => b.Id,
                o => o.BranchId,
                (branch, branchOrders) => new BranchOrderSummary(
                    branch.Id,
                    branchOrders.Count(),
                    branchOrders.Sum(o => o.TotalMoney ?? 0),
                    branchOrders.Any() ? branchOrders.Average(o => o.TotalMoney ?? 0) : 0,
                    fromDate,
                    toDate
                )
                {
                    BranchName = branch.Name
                })
            .OrderByDescending(b => b.TotalRevenue);
        return branchSummary;
    }

    public async Task<OrderSummaryDto> GetOrderSummaryAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var specification = new Specification<Order>(o =>
            o.CreatedAt.Date >= fromDate.Date &&
            o.CreatedAt.Date <= toDate.Date &&
            (!branchId.HasValue || o.BranchId == branchId.Value)
        );
        specification.Includes.Add(o => o.Branch!);
        specification.Includes.Add(o => o.Status!);
        var orders = await _orderRepository.GetAllWithSpecAsync(specification, true);

        var totalOrders = orders.Count();
        var totalRevenue = orders.Sum(o => o.TotalMoney ?? 0);
        var averageOrderValue = totalOrders > 0 ? orders.Average(o => o.TotalMoney ?? 0) : 0;

        var pendingOrders = orders.Count(o => o.Status != null && o.Status.Id == (int)OrderStatusEnum.Pending);
        var completedOrders = orders.Count(o => o.Status!.Id == (int)OrderStatusEnum.Delivered);
        var cancelledOrders = orders.Count(o => o.Status!.Id == (int)OrderStatusEnum.Cancelled);

        var dailySummary = orders
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new DailyOrderSummary(
                g.Key,
                g.Count(),
                g.Average(o => o.TotalMoney ?? 0),
                g.Sum(o => o.TotalMoney ?? 0)
            ))
            .OrderBy(s => s.Date)
            .ToList();

        var branchSummary = orders
            .GroupBy(o => o.Branch!)
            .Select(g => new BranchOrderSummary(
                g.Key.Id,
                g.Count(),
                g.Sum(o => o.TotalMoney ?? 0),
                g.Average(o => o.TotalMoney ?? 0),
                fromDate,
                toDate
            )
            {
                BranchName = g.Key.Name
            })
            .ToList();


        return new OrderSummaryDto(
            totalOrders,
            totalRevenue,
            averageOrderValue,
            pendingOrders,
            completedOrders,
            cancelledOrders
        )
        {
            DailySummary = dailySummary,
            BranchSummary = branchSummary
        };
    }
}

