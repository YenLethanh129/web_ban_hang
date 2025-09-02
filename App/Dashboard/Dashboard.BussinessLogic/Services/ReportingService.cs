using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Dtos.OrderDtos;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using System.Globalization;

namespace Dashboard.BussinessLogic.Services;

public interface IReportingService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<PagedList<RevenueReportDto>> GetRevenueReportAsync(GetRevenueReportInput input);
    Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime fromDate, DateTime toDate, long? branchId = null);
    Task<List<RevenueReportDto>> GetRevenueComparisonAsync(DateTime fromDate, DateTime toDate, ReportPeriodEnum period);
}

public class ReportingService : IReportingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public ReportingService(
        IUnitOfWork unitOfWork,
        IIngredientRepository ingredientRepository,
        IExpenseRepository expenseRepository,
        IOrderService orderService,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _ingredientRepository = ingredientRepository;
        _expenseRepository = expenseRepository;
        _orderService = orderService;   
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var reportPeriods = GetReportingPeriods();
        
        var summaryData = await GetDashboardDataAsync(reportPeriods);
        var branchPerformance = await CalculateBranchPerformanceAsync(reportPeriods.StartOfMonth, reportPeriods.Today);

        return new DashboardSummaryDto
        {
            TodayRevenue = summaryData.TodayOrderSummary.TotalRevenue,
            MonthlyRevenue = summaryData.MonthlyOrderSummary.TotalRevenue,
            YearlyRevenue = summaryData.YearlyOrderSummary.TotalRevenue,
            TodayProfit = CalculateProfit(summaryData.TodayOrderSummary.TotalRevenue, summaryData.TodayExpenses),
            MonthlyProfit = CalculateProfit(summaryData.MonthlyOrderSummary.TotalRevenue, summaryData.MonthlyExpenses),
            YearlyProfit = CalculateProfit(summaryData.YearlyOrderSummary.TotalRevenue, summaryData.YearlyExpenses),
            TodayExpenses = summaryData.TodayExpenses,  
            MonthlyExpenses = summaryData.MonthlyExpenses,
            YearlyExpenses = summaryData.YearlyExpenses,
            TotalOrders = summaryData.YearlyOrderSummary.TotalOrders,
            PendingOrders = summaryData.YearlyOrderSummary.PendingOrders,
            TopProducts = summaryData.TopProducts,
            UnderstockIngredients = summaryData.UnderstockIngredientsDto,
            BranchPerformance = branchPerformance
        };
    }

    public async Task<PagedList<RevenueReportDto>> GetRevenueReportAsync(GetRevenueReportInput input)
    {
        var (fromDate, toDate) = ValidateAndNormalizeDates(input.FromDate, input.ToDate);
        
        var orderSpecification = CreateOrderSpecification(fromDate, toDate, input.BranchId);
        var orders = await _orderRepository.GetAllWithSpecAsync(orderSpecification, true);
        
        var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, input.BranchId);
        var revenueData = GroupOrdersByPeriodWithExpenses(orders, expenses, input.Period, fromDate, toDate);

        return CreatePagedResult(revenueData, input.PageNumber, input.PageSize);
    }

    public async Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var orderSummary = await _orderService.GetOrderSummaryAsync(fromDate, toDate, branchId);
        var grossProfit = orderSummary.TotalRevenue;

        var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, branchId);
        var operatingExpenses = expenses.Sum(e => e.Amount);

        var netProfit = grossProfit - operatingExpenses;
        var profitMargin = grossProfit > 0 ? (netProfit / grossProfit) * 100 : 0;

        var expenseBreakdown = expenses
            .GroupBy(e => e.ExpenseType)
            .Select(g => new ExpenseBredownDto
            {
                Category = g.Key,
                Amount = g.Sum(e => e.Amount),
                Percentage = operatingExpenses > 0 ? (g.Sum(e => e.Amount) / operatingExpenses) * 100 : 0
            })
            .OrderByDescending(e => e.Amount)
            .ToList();

        return new ProfitAnalysisDto
        {
            GrossProfit = grossProfit,
            OperatingExpenses = operatingExpenses,
            NetProfit = netProfit,
            ProfitMargin = profitMargin,
            PeriodStart = fromDate,
            PeriodEnd = toDate,
            ExpenseBreakdown = expenseBreakdown
        };
    }

    public async Task<List<RevenueReportDto>> GetRevenueComparisonAsync(DateTime fromDate, DateTime toDate, ReportPeriodEnum period)
    {
        var input = new GetRevenueReportInput
        {
            FromDate = fromDate,
            ToDate = toDate,
            Period = period,
            PageSize = int.MaxValue,
            PageNumber = 1
        };

        var result = await GetRevenueReportAsync(input);
        return result.Items.ToList();
    }

    private async Task<decimal> GetExpensesForPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, branchId);
        return expenses.Sum(e => e.Amount);
    }

    private async Task<decimal> GetExpensesForBranchInPeriodAsync(long branchId, DateTime fromDate, DateTime toDate)
    {
        var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, branchId);
        return expenses.Sum(e => e.Amount);
    }

    private async Task<IEnumerable<BranchExpense>> GetExpensesInPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        return await _expenseRepository.GetExpensesInPeriodAsync(fromDate, toDate, branchId);
    }

    private async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime fromDate, DateTime toDate, int limit = 5)
    {
        var orderSpecification = CreateTopProductsOrderSpecification(fromDate, toDate);
        var orders = await _orderRepository.GetAllWithSpecAsync(orderSpecification, true);

        return orders
            .SelectMany(o => o.OrderDetails!)
            .GroupBy(od => new { od.ProductId, od.Product!.Name })
            .Select(g => new TopSellingProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                QuantitySold = g.Sum(od => od.Quantity),
                Revenue = g.Sum(od => (od.UnitPrice) * (od.Quantity))
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(limit)
            .ToList();
    }

    private static List<RevenueReportDto> GroupOrdersByPeriodWithExpenses(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses,
        ReportPeriodEnum period,
        DateTime fromDate,
        DateTime toDate)
    {
        return period switch
        {
            ReportPeriodEnum.Daily => GroupByDailyWithExpenses(orders, expenses, fromDate, toDate),
            ReportPeriodEnum.Weekly => GroupByWeeklyWithExpenses(orders, expenses),
            ReportPeriodEnum.Monthly => GroupByMonthlyWithExpenses(orders, expenses),
            ReportPeriodEnum.Yearly => GroupByYearlyWithExpenses(orders, expenses),
            _ => GroupByPeriodWithExpense(orders, expenses, fromDate, toDate)
        };
    }

    private static List<RevenueReportDto> GroupByPeriodWithExpense(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses,
        DateTime fromDate,
        DateTime toDate)
    {
        var orderGroups = orders
            .Where(o => o.CreatedAt.Date >= fromDate.Date && o.CreatedAt.Date <= toDate.Date)
            .GroupBy(o => new { o.BranchId, BranchName = o.Branch!.Name });

        return orderGroups.Select(branchGroup =>
        {
            var branchExpenses = expenses.Where(e => e.BranchId == branchGroup.Key.BranchId);
            var totalExpense = ExpenseCalculationHelper.CalculatePeriodExpenseAllocation(branchExpenses, fromDate, toDate);
            var totalRevenue = branchGroup.Sum(o => o.TotalMoney ?? 0);

            return CreateRevenueReportDto(totalRevenue, totalExpense, fromDate, branchGroup.Key.BranchId, branchGroup.Key.BranchName);
        })
        .OrderByDescending(r => r.ReportDate)
        .ToList();
    }


    private static List<RevenueReportDto> GroupByDailyWithExpenses(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses,
        DateTime fromDate,
        DateTime toDate)
    {
        var result = new List<RevenueReportDto>();

        for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
        {
            var dailyOrders = orders.Where(o => o.CreatedAt.Date == date).GroupBy(o => new { o.BranchId, BranchName = o.Branch!.Name });

            foreach (var branchGroup in dailyOrders)
            {
                var branchExpenses = expenses.Where(e => e.BranchId == branchGroup.Key.BranchId);
                var dailyExpense = ExpenseCalculationHelper.CalculateDailyExpenseAllocation(branchExpenses, date);
                var totalRevenue = branchGroup.Sum(o => o.TotalMoney ?? 0);

                result.Add(CreateRevenueReportDto(totalRevenue, dailyExpense, date, branchGroup.Key.BranchId, branchGroup.Key.BranchName));
            }
        }

        return result.OrderByDescending(r => r.ReportDate).ToList();
    }

    private static List<RevenueReportDto> GroupByWeeklyWithExpenses(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses)
    {
        var orderGroups = orders
            .GroupBy(o => new {
                Year = o.CreatedAt.Year,
                Week = GetWeekOfYear(o.CreatedAt),
                o.BranchId,
                BranchName = o.Branch!.Name
            })
            .ToList();

        return orderGroups.Select(g =>
        {
            var weekStart = GetDateFromWeek(g.Key.Year, g.Key.Week);
            var weekEnd = weekStart.AddDays(6);
            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var weeklyExpense = ExpenseCalculationHelper.CalculateWeeklyExpenseAllocation(branchExpenses, weekStart, weekEnd);
            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return CreateRevenueReportDto(totalRevenue, weeklyExpense, weekStart, g.Key.BranchId, g.Key.BranchName);
        })
        .OrderByDescending(r => r.ReportDate)
        .ToList();
    }

    private static List<RevenueReportDto> GroupByMonthlyWithExpenses(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses)
    {
        var orderGroups = orders
            .GroupBy(o => new {
                o.CreatedAt.Year,
                o.CreatedAt.Month,
                o.BranchId,
                BranchName = o.Branch!.Name
            })
            .ToList();

        return orderGroups.Select(g =>
        {
            var monthStart = new DateTime(g.Key.Year, g.Key.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var monthlyExpense = ExpenseCalculationHelper.CalculatePeriodExpenseAllocation(branchExpenses, monthStart, monthEnd);
            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return CreateRevenueReportDto(totalRevenue, monthlyExpense, monthStart, g.Key.BranchId, g.Key.BranchName);
        })
        .OrderByDescending(r => r.ReportDate)
        .ToList();
    }

    private static List<RevenueReportDto> GroupByYearlyWithExpenses(
        IEnumerable<Order> orders,
        IEnumerable<BranchExpense> expenses)
    {
        var orderGroups = orders
            .GroupBy(o => new {
                o.CreatedAt.Year,
                o.BranchId,
                BranchName = o.Branch!.Name
            })
            .ToList();

        return orderGroups.Select(g =>
        {
            var yearStart = new DateTime(g.Key.Year, 1, 1);
            var yearEnd = new DateTime(g.Key.Year, 12, 31);
            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var yearlyExpense = ExpenseCalculationHelper.CalculatePeriodExpenseAllocation(branchExpenses, yearStart, yearEnd);
            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return CreateRevenueReportDto(totalRevenue, yearlyExpense, yearStart, g.Key.BranchId, g.Key.BranchName);
        })
        .OrderByDescending(r => r.ReportDate)
        .ToList();
    }

    private static int GetWeekOfYear(DateTime date)
    {
        return ISOWeek.GetWeekOfYear(date);
    }

    private static DateTime GetDateFromWeek(int year, int week)
    {
        return ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
    }

    private static ReportingPeriods GetReportingPeriods()
    {
        var today = DateTime.Today;
        return new ReportingPeriods
        {
            Today = today,
            StartOfMonth = new DateTime(today.Year, today.Month, 1),
            StartOfYear = new DateTime(today.Year, 1, 1)
        };
    }

    private async Task<DashboardSummaryData> GetDashboardDataAsync(ReportingPeriods periods)
    {
        //var todayOrderSummaryTask = _orderService.GetOrderSummaryAsync(periods.Today, periods.Today);
        //var monthlyOrderSummaryTask = _orderService.GetOrderSummaryAsync(periods.StartOfMonth, periods.Today);
        //var yearlyOrderSummaryTask = _orderService.GetOrderSummaryAsync(periods.StartOfYear, periods.Today);
        //var understockIngredientsTask = _ingredientRepository.GetLowStockWarehouseIngredientsAsync();
        //var todayExpensesTask = GetExpensesForPeriodAsync(periods.Today, periods.Today);
        //var monthlyExpensesTask = GetExpensesForPeriodAsync(periods.StartOfMonth, periods.Today);
        //var yearlyExpensesTask = GetExpensesForPeriodAsync(periods.StartOfYear, periods.Today);
        //var topProductsTask = GetTopSellingProductsAsync(periods.StartOfMonth, periods.Today);

        //await Task.WhenAll(
        //    todayOrderSummaryTask, monthlyOrderSummaryTask, yearlyOrderSummaryTask,
        //    understockIngredientsTask, todayExpensesTask, monthlyExpensesTask,
        //    yearlyExpensesTask, topProductsTask);

        var todayOrderSummaryTask = await _orderService.GetOrderSummaryAsync(periods.Today, periods.Today);
        var monthlyOrderSummaryTask = await _orderService.GetOrderSummaryAsync(periods.StartOfMonth, periods.Today);
        var yearlyOrderSummaryTask = await _orderService.GetOrderSummaryAsync(periods.StartOfYear, periods.Today);
        var understockIngredientsTask = await _ingredientRepository.GetLowStockWarehouseIngredientsAsync();
        var todayExpensesTask = await GetExpensesForPeriodAsync(periods.Today, periods.Today);
        var monthlyExpensesTask = await GetExpensesForPeriodAsync(periods.StartOfMonth, periods.Today);
        var yearlyExpensesTask = await GetExpensesForPeriodAsync(periods.StartOfYear, periods.Today);
        var topProductsTask = await GetTopSellingProductsAsync(periods.StartOfMonth, periods.Today);

        var understockIngredientsDto = _mapper.Map<List<LowStockIngredientDto>>(understockIngredientsTask);

        return new DashboardSummaryData
        {
            TodayOrderSummary = todayOrderSummaryTask,
            MonthlyOrderSummary = monthlyOrderSummaryTask,
            YearlyOrderSummary = yearlyOrderSummaryTask,
            TodayExpenses = todayExpensesTask,
            MonthlyExpenses = monthlyExpensesTask,
            YearlyExpenses = yearlyExpensesTask,
            TopProducts = topProductsTask,
            UnderstockIngredientsDto = understockIngredientsDto
        };
    }

    private async Task<List<BranchPerformanceDto>> CalculateBranchPerformanceAsync(DateTime startOfMonth, DateTime today)
    {
        var branchPerformance = await _orderService.GetBranchOrderSummaryAsync(startOfMonth, today);
        var result = new List<BranchPerformanceDto>();

        foreach (var bp in branchPerformance)
        {
            var branchExpenses = await GetExpensesForBranchInPeriodAsync(bp.BranchId, startOfMonth, today);
            result.Add(new BranchPerformanceDto
            {
                BranchId = bp.BranchId,
                BranchName = bp.BranchName,
                Revenue = bp.TotalRevenue,
                Profit = bp.TotalRevenue - branchExpenses,
                OrderCount = bp.TotalOrders
            });
        }

        return result;
    }

    private static decimal CalculateProfit(decimal revenue, decimal expenses)
    {
        return revenue - expenses;
    }

    private static (DateTime fromDate, DateTime toDate) ValidateAndNormalizeDates(DateTime? fromDate, DateTime? toDate)
    {
        var normalizedFromDate = fromDate ?? DateTime.Today.AddDays(-30);
        var normalizedToDate = toDate ?? DateTime.Today;
        return (normalizedFromDate, normalizedToDate);
    }

    private static PagedList<RevenueReportDto> CreatePagedResult(List<RevenueReportDto> data, int pageNumber, int pageSize)
    {
        var totalCount = data.Count;
        var pagedData = data
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<RevenueReportDto>
        {
            Items = pagedData,
            TotalRecords = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    private static RevenueReportDto CreateRevenueReportDto(decimal totalRevenue, decimal totalExpense, DateTime reportDate, long? branchId, string branchName)
    {
        return new RevenueReportDto
        {
            TotalRevenue = totalRevenue,
            TotalExpenses = totalExpense,
            TotalProfit = totalRevenue,
            NetProfit = totalRevenue - totalExpense,
            ReportDate = reportDate,
            BranchId = branchId,
            BranchName = branchName
        };
    }

    private Specification<Order> CreateOrderSpecification(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var spec = new Specification<Order>(o =>
            o.CreatedAt.Date >= fromDate.Date &&
            o.CreatedAt.Date <= toDate.Date &&
            o.Status!.Id == (long)OrderStatusEnum.Delivered);

        if (branchId.HasValue)
        {
            spec = new Specification<Order>(o =>
                o.CreatedAt.Date >= fromDate.Date &&
                o.CreatedAt.Date <= toDate.Date &&
                o.Status!.Id == (long)OrderStatusEnum.Delivered &&
                o.BranchId == branchId.Value);
        }

        spec.Includes.Add(o => o.Branch!);
        return spec;
    }

    private Specification<Order> CreateTopProductsOrderSpecification(DateTime fromDate, DateTime toDate)
    {
        var spec = new Specification<Order>(o =>
            o.CreatedAt >= fromDate &&
            o.CreatedAt <= toDate &&
            o.Status!.Id == (long)OrderStatusEnum.Delivered);

        spec.Includes.Add(o => o.OrderDetails!);
        spec.IncludeStrings.Add("OrderDetails.Product");
        return spec;
    }
}

// Helper classes for better organization
internal class ReportingPeriods
{
    public DateTime Today { get; set; }
    public DateTime StartOfMonth { get; set; }
    public DateTime StartOfYear { get; set; }
}

internal class DashboardSummaryData
{
    public OrderSummaryDto TodayOrderSummary { get; set; } = null!;
    public OrderSummaryDto MonthlyOrderSummary { get; set; } = null!;
    public OrderSummaryDto YearlyOrderSummary { get; set; } = null!;
    public decimal TodayExpenses { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal YearlyExpenses { get; set; }
    public List<TopSellingProductDto> TopProducts { get; set; } = null!;
    public List<LowStockIngredientDto> UnderstockIngredientsDto { get; set; } = null!;
}

internal static class ExpenseCalculationHelper
{
    public static decimal CalculateDailyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime targetDate)
    {
        var targetDateOnly = DateOnly.FromDateTime(targetDate);
        return expenses
            .Where(e => targetDateOnly >= e.StartDate && targetDateOnly <= (e.EndDate ?? e.StartDate))
            .Sum(e => CalculateProportionalExpense(e, 1));
    }

    public static decimal CalculateWeeklyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime weekStart, DateTime weekEnd)
    {
        decimal totalExpense = 0;
        for (var date = weekStart; date <= weekEnd; date = date.AddDays(1))
        {
            totalExpense += CalculateDailyExpenseAllocation(expenses, date);
        }
        return totalExpense;
    }

    public static decimal CalculatePeriodExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime periodStart, DateTime periodEnd)
    {
        var periodStartDateOnly = DateOnly.FromDateTime(periodStart);
        var periodEndDateOnly = DateOnly.FromDateTime(periodEnd);

        return expenses
            .Where(e => !(e.EndDate < periodStartDateOnly || e.StartDate > periodEndDateOnly))
            .Sum(e =>
            {
                var expenseStart = e.StartDate > periodStartDateOnly ? e.StartDate : periodStartDateOnly;
                var expenseEnd = (e.EndDate.HasValue ? (e.EndDate.Value < periodEndDateOnly ? e.EndDate.Value : periodEndDateOnly) : periodEndDateOnly);
                var overlapDays = (expenseEnd.DayNumber - expenseStart.DayNumber) + 1;
                
                return CalculateProportionalExpense(e, overlapDays);
            });
    }

    private static decimal CalculateProportionalExpense(BranchExpense expense, int days)
    {
        var totalExpenseDays = ((expense.EndDate ?? expense.StartDate).DayNumber - expense.StartDate.DayNumber) + 1;
        return (expense.Amount * days) / totalExpenseDays;
    }
}