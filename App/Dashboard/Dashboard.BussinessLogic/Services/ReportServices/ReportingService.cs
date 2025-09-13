using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ExpenseDtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Dtos.OrderDtos;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Dashboard.BussinessLogic.Services.ReportServices;

public interface IReportingService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime? toDate = null, DateTime? fromDate = null);
    Task<PagedList<FinacialReportDto>> GetFinacialReportAsync(GetRevenueReportInput input);
    Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    Task<List<FinacialReportDto>> GetRevenueComparisonAsync(DateTime fromDate, DateTime toDate, ReportPeriodEnum period);
}

public class ReportingService : BaseTransactionalService, IReportingService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;
    public ReportingService(
        IUnitOfWork unitOfWork,
        IIngredientRepository ingredientRepository,
        IExpenseRepository expenseRepository,
        IOrderService orderService,
        IOrderRepository orderRepository,
        IExpenseService expenseService,
        IMapper mapper) : base(unitOfWork)
    {
        _ingredientRepository = ingredientRepository;
        _expenseRepository = expenseRepository;
        _orderService = orderService;
        _orderRepository = orderRepository;
        _expenseService = expenseService;
        _mapper = mapper;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(DateTime? fromDate = null, DateTime? toDate  = null)
    {
        var startDate = fromDate ?? DateTime.MinValue;
        var endDate = toDate ?? DateTime.Today;

        var summaryData = await GetDashboardDataAsync(startDate, endDate);
       
        return new DashboardSummaryDto
        {
            TotalRevenue = summaryData.RevenueSummary,
            NetProfit = summaryData.ProfitSummary,
            TotalExpenses = summaryData.ExpenseSummary,
            TotalOrders = summaryData.OrderSummary.TotalOrders,
            PendingOrders = summaryData.OrderSummary.PendingOrders,
            TopProducts = summaryData.TopProducts,
            UnderstockIngredients = summaryData.UnderstockIngredientsDto,
            FinacialReports = summaryData.FinacialReports,
            BranchPerformance = await GetBranchPerformanceAsync(startDate, endDate)
        };
    }

    public async Task<PagedList<FinacialReportDto>> GetFinacialReportAsync(GetRevenueReportInput input)
    {
        var (fromDate, toDate) = ValidateDates(input.FromDate, input.ToDate);

        var orders = await GetOrdersAsync(fromDate, toDate, input.BranchId);
        var expenses = await GetExpensesAsync(fromDate, toDate, input.BranchId);

        var finacialReport = GroupFinancialReport(orders, expenses, input.Period, fromDate, toDate);
        return PaginateResults(finacialReport, input.PageNumber, input.PageSize);
    }

    private List<FinacialReportDto> GroupFinancialReport(
        IEnumerable<Order> orders,
        IEnumerable<ExpenseDto> expenses,
        ReportPeriodEnum period,
        DateTime fromDate,
        DateTime toDate)
    {
        Func<DateTime, DateTime> keySelector = period switch
        {
            ReportPeriodEnum.Daily => date => date.Date,
            ReportPeriodEnum.Weekly => date => FirstDateOfWeek(date),
            ReportPeriodEnum.Monthly => date => new DateTime(date.Year, date.Month, 1),
            ReportPeriodEnum.Yearly => date => new DateTime(date.Year, 1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(period), "Unsupported reporting period")
        };

        var orderGroups = orders
            .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
            .GroupBy(o => keySelector(o.CreatedAt))
            .ToDictionary(g => g.Key, g => g.ToList());

        var expenseGroups = expenses
            .Where(e => e.StartDate >= fromDate && e.StartDate <= toDate)
            .GroupBy(e => keySelector(e.StartDate))
            .ToDictionary(g => g.Key, g => g.ToList());

        var allKeys = orderGroups.Keys
            .Union(expenseGroups.Keys)
            .OrderBy(k => k);

        var result = new List<FinacialReportDto>();

        foreach (var key in allKeys)
        {
            var ordersForPeriod = orderGroups.ContainsKey(key) ? orderGroups[key] : new List<Order>();
            var expensesForPeriod = expenseGroups.ContainsKey(key) ? expenseGroups[key] : new List<ExpenseDto>();

            var revenue = ordersForPeriod.Sum(o => o.TotalMoney ?? 0);
            var expense = expensesForPeriod.Sum(e => e.Amount);

            result.Add(new FinacialReportDto
            {
                ReportDate = key,
                TotalRevenue = revenue,
                TotalExpenses = expense,
                NetProfit = revenue - expense
            });
        }

        return result;
    }


    private static DateTime FirstDateOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }

    public async Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var orders = await _orderService.GetOrderSummaryAsync(fromDate, toDate, branchId);
        var expenses = await GetExpensesAsync(fromDate, toDate, branchId);

        var operatingExpenses = expenses.Sum(e => e.Amount);
        var netProfit = orders.TotalRevenue - operatingExpenses;

        return new ProfitAnalysisDto
        {
            GrossProfit = orders.TotalRevenue,
            OperatingExpenses = operatingExpenses,
            NetProfit = netProfit,
            ProfitMargin = CalculateProfitMargin(orders.TotalRevenue, netProfit),
            PeriodStart = fromDate,
            PeriodEnd = toDate,
            ExpenseBreakdown = GroupExpenseBreakdown(expenses, operatingExpenses)
        };
    }

    public async Task<List<FinacialReportDto>> GetRevenueComparisonAsync(DateTime fromDate, DateTime toDate, ReportPeriodEnum period)
    {
        var reportInput = new GetRevenueReportInput
        {
            FromDate = fromDate,
            ToDate = toDate,
            Period = period,
            PageNumber = 1,
            PageSize = int.MaxValue
        };

        var report = await GetFinacialReportAsync(reportInput);
        return [.. report.Items];
    }

    private async Task<DashboardSummaryData> GetDashboardDataAsync(DateTime fromDate, DateTime toDate)
    {
        var orders = await _orderService.GetOrderSummaryAsync(fromDate, toDate);
        var topProducts = await GetTopProductsAsync(fromDate, toDate, 5);
        var finacialReports = await GetFinacialReportAsync(new GetRevenueReportInput
        {
            FromDate = fromDate,
            ToDate = toDate,
            PageNumber = 1,
            PageSize = int.MaxValue,
        });
        var nonZeroExpenses = finacialReports.Items
            .Where(f => f.TotalExpenses != 0)
            .ToList();

        var lowStockIngredients = await _ingredientRepository.GetLowStockWarehouseIngredientsAsync();
        return new DashboardSummaryData
        {
            OrderSummary = orders,
            RevenueSummary = finacialReports.Items.Sum(f => f.TotalRevenue),
            ExpenseSummary = finacialReports.Items.Sum(f => f.TotalExpenses),
            ProfitSummary = finacialReports.Items.Sum(f => f.NetProfit),
            TopProducts = topProducts,
            FinacialReports = [.. finacialReports.Items],
            UnderstockIngredientsDto = _mapper.Map<List<LowStockIngredientDto>>(lowStockIngredients)
        };
    }

    private async Task<List<TopSellingProductDto>> GetTopProductsAsync(DateTime fromDate, DateTime toDate, int numberOfRanks)
    {
        var orders = await GetOrdersAsync(fromDate, toDate);
        return [.. orders
            .SelectMany(o => o.OrderDetails!)
            .GroupBy(od => new { od.ProductId, od.Product!.Name })
            .Select(g => new TopSellingProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                QuantitySold = g.Sum(od => od.Quantity),
                Revenue = g.Sum(od => od.UnitPrice * od.Quantity)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(numberOfRanks)];
    }

    private async Task<List<Order>> GetOrdersAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var spec = new Specification<Order>(o =>
            o.CreatedAt >= fromDate &&
            o.CreatedAt <= toDate);

        if (branchId.HasValue)
        {
            spec = new Specification<Order>(o =>
                o.CreatedAt >= fromDate &&
                o.CreatedAt <= toDate);
        }
        spec.Includes.Add(o => o.Include(order => order.OrderDetails!).ThenInclude(od => od.Product!));
        var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec, true) ?? [];
        return [.. orders];
    }

    private async Task<List<ExpenseDto>> GetExpensesAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var spec = new GetExpensesInput
        {
            FromDate = fromDate,
            ToDate = toDate,
            BranchId = branchId,
            PageNumber = 1,
            PageSize = int.MaxValue
        };

        var expenses = await _expenseService.GetExpensesAsync(spec);
        return expenses.Items.ToList();
    }

    private async Task<List<BranchPerformanceDto>> GetBranchPerformanceAsync(DateTime fromDate, DateTime toDate)
    {
        var branches = await _orderService.GetBranchOrderSummaryAsync(fromDate, toDate);
        var results = new List<BranchPerformanceDto>();

        foreach (var branch in branches)
        {
            var expenses = await GetExpensesAsync(fromDate, toDate, branch.BranchId);
            results.Add(new BranchPerformanceDto
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                Revenue = branch.TotalRevenue,
                Profit = branch.TotalRevenue - expenses.Sum(e => e.Amount),
                OrderCount = branch.TotalOrders
            });
        }

        return results;
    }

    private static decimal CalculateProfitMargin(decimal revenue, decimal netProfit)
    {
        return revenue > 0 ? netProfit / revenue * 100 : 0;
    }

    private static List<ExpenseBredownDto> GroupExpenseBreakdown(IEnumerable<ExpenseDto> expenses, decimal totalExpenses)
    {
        return [.. expenses
            .GroupBy(e => e.ExpenseType)
            .Select(g => new ExpenseBredownDto
            {
                Category = g.Key,
                Amount = g.Sum(e => e.Amount),
                Percentage = totalExpenses > 0 ? g.Sum(e => e.Amount) / totalExpenses * 100 : 0
            })
            .OrderByDescending(e => e.Amount)];
    }

    private static (DateTime fromDate, DateTime toDate) ValidateDates(DateTime? fromDate, DateTime? toDate)
    {
        return (fromDate ?? DateTime.Today.AddDays(-30), toDate ?? DateTime.Today);
    }

    private static PagedList<FinacialReportDto> PaginateResults(List<FinacialReportDto> data, int pageNumber, int pageSize)
    {
        return new PagedList<FinacialReportDto>
        {
            Items = [.. data.Skip((pageNumber - 1) * pageSize).Take(pageSize)],
            TotalRecords = data.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

}
internal class DashboardSummaryData
{
    public OrderSummaryDto OrderSummary { get; set; } = null!;
    public decimal RevenueSummary { get; set; }
    public decimal ExpenseSummary { get; set; }
    public decimal ProfitSummary { get; set; }
    public List<TopSellingProductDto> TopProducts { get; set; } = null!;
    public List<FinacialReportDto> FinacialReports { get; set; } = null!;
    public List<LowStockIngredientDto> UnderstockIngredientsDto { get; set; } = null!;
}