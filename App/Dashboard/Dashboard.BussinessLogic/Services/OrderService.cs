using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.OrderDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services;

public interface IOrderService
{
    Task<int> GetCountAsync();
    Task<PagedList<OrderDto>> GetOrdersAsync(GetOrdersInput input);
    Task<OrderDto?> GetOrderByIdAsync(long id);
    Task<OrderDto> CreateOrderAsync(CreateOrderInput input);
    Task<OrderDto> UpdateOrderAsync(long id, UpdateOrderInput input);
    Task DeleteOrderAsync(long id);
    Task<OrderSummaryDto> GetOrderSummaryAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(DateTime date, long? branchId = null);
    Task<IEnumerable<BranchOrderSummary>> GetBranchOrderSummaryAsync(DateTime fromDate, DateTime toDate);
}

public class OrderService : BaseTransactionalService, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IBranchRepository branchRepository,
        IMapper mapper) : base(unitOfWork)
    {
        _orderRepository = orderRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<int> GetCountAsync()
    {
        return await _unitOfWork.Repository<Order>().GetCountAsync();
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(GetOrdersInput input)
    {
        // Use Specification from BusinessLogic layer
        var specification = OrderSpecifications.BySearchCriteria(input);
        var allOrders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(specification, true);
        
        // Apply sorting
        var sortedOrders = ApplySorting(allOrders, input);
        
        // Apply pagination
        var pagedOrders = sortedOrders
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var orderDtos = _mapper.Map<List<OrderDto>>(pagedOrders);

        return new PagedList<OrderDto>
        {
            Items = orderDtos,
            TotalRecords = allOrders.Count(),
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    private static IEnumerable<Order> ApplySorting(IEnumerable<Order> orders, GetOrdersInput input)
    {
        if (input.SortBy.HasValue)
        {
            return input.SortBy switch
            {
                SortByEnum.CreatedDate => input.IsDescending
                    ? orders.OrderByDescending(o => o.CreatedAt)
                    : orders.OrderBy(o => o.CreatedAt),
                _ => orders.OrderByDescending(o => o.CreatedAt),
            };
        }
        
        return orders.OrderByDescending(o => o.CreatedAt);
    }
    public async Task<OrderDto?> GetOrderByIdAsync(long id)
    {
        var specification = OrderSpecifications.WithFullDetails(id);
        var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(specification);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderInput input)
    {
        var order = _mapper.Map<Order>(input);
        order.CreatedAt = DateTime.Now;
        order.StatusId = 1;

        await _unitOfWork.Repository<Order>().AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return await GetOrderByIdAsync(order.Id) ?? throw new InvalidOperationException("Failed to retrieve created order");
    }

    public async Task<OrderDto> UpdateOrderAsync(long id, UpdateOrderInput input)
    {
        var order = await _unitOfWork.Repository<Order>().GetAsync(id) 
            ?? throw new KeyNotFoundException($"Order with id {id} not found");
        
        _mapper.Map(input, order);
        _unitOfWork.Repository<Order>().Remove(order);
        _unitOfWork.Repository<Order>().Add(order);
        await _unitOfWork.SaveChangesAsync();

        return await GetOrderByIdAsync(id) ?? throw new InvalidOperationException("Failed to retrieve updated order");
    }

    public async Task DeleteOrderAsync(long id)
    {
        var order = await _unitOfWork.Repository<Order>().GetAsync(id) 
            ?? throw new KeyNotFoundException($"Order with id {id} not found");
        
        _unitOfWork.Repository<Order>().Remove(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<DailyOrderSummary>> GetDailyOrderSummaryAsync(DateTime date, long? branchId = null)
    {
        var specification = OrderSpecifications.ForDailySummary(date, branchId);
        var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(specification, true);
        
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
        var specification = OrderSpecifications.ForBranchSummary(fromDate, toDate);
        var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(specification, true);
        
        var branchSummary = orders
            .GroupBy(o => new { o.BranchId, BranchName = o.Branch!.Name })
            .Select(g => new BranchOrderSummary(
                g.Key.BranchId ?? 0,
                g.Count(),
                g.Sum(o => o.TotalMoney ?? 0),
                g.Average(o => o.TotalMoney ?? 0),
                fromDate,
                toDate
            ))
            .OrderByDescending(s => s.TotalRevenue)
            .ToList();

        return branchSummary;
    }

    public async Task<OrderSummaryDto> GetOrderSummaryAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var specification = OrderSpecifications.ForRevenueSummary(fromDate, toDate, branchId);
        var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(specification, true);

        var totalOrders = orders.Count();
        var totalRevenue = orders.Sum(o => o.TotalMoney ?? 0);
        var averageOrderValue = totalOrders > 0 ? orders.Average(o => o.TotalMoney ?? 0) : 0;

        var pendingOrders = orders.Count(o => o.Status != null && o.Status.Id == (long)OrderStatusEnum.Pending);
        var completedOrders = orders.Count(o => o.Status!.Id == (long)OrderStatusEnum.Delivered);
        var cancelledOrders = orders.Count(o => o.Status!.Id == (long)OrderStatusEnum.Cancelled);

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
            ))
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

