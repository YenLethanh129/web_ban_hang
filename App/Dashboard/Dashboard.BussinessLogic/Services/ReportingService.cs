using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
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
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public ReportingService(
        IUnitOfWork unitOfWork,
        IOrderService orderService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _mapper = mapper;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        try
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var todayOrderSummary = await _orderService.GetOrderSummaryAsync(today, today);
            var monthlyOrderSummary = await _orderService.GetOrderSummaryAsync(startOfMonth, today);
            var yearlyOrderSummary = await _orderService.GetOrderSummaryAsync(startOfYear, today);

            var todayExpenses = await GetExpensesForPeriodAsync(today, today);
            var monthlyExpenses = await GetExpensesForPeriodAsync(startOfMonth, today);
            var yearlyExpenses = await GetExpensesForPeriodAsync(startOfYear, today);

            var topProducts = await GetTopSellingProductsAsync(startOfMonth, today);

            var branchPerformance = await _orderService.GetBranchOrderSummaryAsync(startOfMonth, today);

            return new DashboardSummaryDto
            {
                TodayRevenue = todayOrderSummary.TotalRevenue,
                MonthlyRevenue = monthlyOrderSummary.TotalRevenue,
                YearlyRevenue = yearlyOrderSummary.TotalRevenue,
                TodayProfit = todayOrderSummary.TotalRevenue - todayExpenses,
                MonthlyProfit = monthlyOrderSummary.TotalRevenue - monthlyExpenses,
                YearlyProfit = yearlyOrderSummary.TotalRevenue - yearlyExpenses,
                TodayExpenses = todayExpenses,
                MonthlyExpenses = monthlyExpenses,
                YearlyExpenses = yearlyExpenses,
                TotalOrders = yearlyOrderSummary.TotalOrders,
                PendingOrders = yearlyOrderSummary.PendingOrders,
                TopProducts = topProducts,
                BranchPerformance = [.. branchPerformance.Select(bp => new BranchPerformanceDto
                {
                    BranchId = bp.BranchId,
                    BranchName = bp.BranchName,
                    Revenue = bp.TotalRevenue,
                    Profit = bp.TotalRevenue - GetExpensesForBranchInPeriodAsync(bp.BranchId, startOfMonth, today).Result,
                    OrderCount = bp.TotalOrders
                })]
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Error generating dashboard summary", ex);
        }
    }

    public async Task<PagedList<RevenueReportDto>> GetRevenueReportAsync(GetRevenueReportInput input)
    {
        try
        {
            var fromDate = input.FromDate ?? DateTime.Today.AddDays(-30);
            var toDate = input.ToDate ?? DateTime.Today;

            var orderSpecification = new Specification<Order>(o =>
                o.CreatedAt.Date >= fromDate.Date &&
                o.CreatedAt.Date <= toDate.Date &&
                o.Status!.Id == (long)OrderStatusEnum.Delivered &&
                o.BranchId == input.BranchId
            );
            orderSpecification.Includes.Add(o => o.Branch!);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(orderSpecification, true);

            var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, input.BranchId);

            var revenueData = GroupOrdersByPeriodWithExpenses(orders, expenses, input.Period, fromDate, toDate);

            var totalCount = revenueData.Count();
            var pagedData = revenueData
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            return new PagedList<RevenueReportDto>
            {
                Items = pagedData,
                TotalRecords = totalCount,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Error generating revenue report", ex);
        }
    }

    public async Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception("Error generating profit analysis", ex);
        }
    }

    public async Task<List<RevenueReportDto>> GetRevenueComparisonAsync(DateTime fromDate, DateTime toDate, ReportPeriodEnum period)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception("Error generating revenue comparison", ex);
        }
    }

    private async Task<decimal> GetExpensesForPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        try
        {
            var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, branchId);
            return expenses.Sum(e => e.Amount);
        }
        catch
        {
            return 0;
        }
    }

    private async Task<decimal> GetExpensesForBranchInPeriodAsync(long branchId, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var expenses = await GetExpensesInPeriodAsync(fromDate, toDate, branchId);
            return expenses.Sum(e => e.Amount);
        }
        catch
        {
            return 0;
        }
    }

    private async Task<IEnumerable<BranchExpense>> GetExpensesInPeriodAsync(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        var fromDateOnly = DateOnly.FromDateTime(fromDate);
        var toDateOnly = DateOnly.FromDateTime(toDate);

        var specification = new Specification<BranchExpense>(e =>
            (e.StartDate <= toDateOnly && (e.EndDate ?? e.StartDate) >= fromDateOnly) &&
            (!branchId.HasValue || e.BranchId == branchId.Value)
        );

        return await _unitOfWork.Repository<BranchExpense>().GetAllWithSpecAsync(specification, true);
    }

    private async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime fromDate, DateTime toDate, int limit = 5)
    {
        try
        {
            var orderSpecification = new Specification<Order>(o =>
                o.CreatedAt >= fromDate &&
                o.CreatedAt <= toDate &&
                o.Status!.Id == (long)OrderStatusEnum.Delivered
            );
            orderSpecification.Includes.Add(o => o.OrderDetails!);
            orderSpecification.Includes.Add(o => o.OrderDetails!.Select(od => od.Product!));

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(orderSpecification, true);

            var topProducts = orders
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

            return topProducts;
        }
        catch
        {
            return new List<TopSellingProductDto>();
        }
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
        var result = new List<RevenueReportDto>();

        var orderGroups = orders
            .Where(o => o.CreatedAt.Date >= fromDate.Date && o.CreatedAt.Date <= toDate.Date)
            .GroupBy(o => new { o.BranchId, BranchName = o.Branch!.Name });

        foreach (var branchGroup in orderGroups)
        {
            var startDateOnly = DateOnly.FromDateTime(fromDate);
            var endDateOnly = DateOnly.FromDateTime(toDate);
            var branchExpenses = expenses
                .Where(e => e.BranchId == branchGroup.Key.BranchId
                            && e.StartDate >= startDateOnly
                            && e.EndDate <= endDateOnly);

            var totalExpense = branchExpenses.Sum(e => e.Amount);

            var totalRevenue = branchGroup.Sum(o => o.TotalMoney ?? 0);

            result.Add(new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpense,
                TotalProfit = totalRevenue,
                NetProfit = totalRevenue - totalExpense,
                ReportDate = fromDate,
                BranchId = branchGroup.Key.BranchId,
                BranchName = branchGroup.Key.BranchName
            });
        }

        return result.OrderByDescending(r => r.ReportDate).ToList();
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
                var dailyExpense = CalculateDailyExpenseAllocation(branchExpenses, date);

                var totalRevenue = branchGroup.Sum(o => o.TotalMoney ?? 0);

                result.Add(new RevenueReportDto
                {
                    TotalRevenue = totalRevenue,
                    TotalExpenses = dailyExpense,
                    TotalProfit = totalRevenue,
                    NetProfit = totalRevenue - dailyExpense,
                    ReportDate = date,
                    BranchId = branchGroup.Key.BranchId,
                    BranchName = branchGroup.Key.BranchName
                });
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

        return [.. orderGroups.Select(g =>
        {
            var weekStart = GetDateFromWeek(g.Key.Year, g.Key.Week);
            var weekEnd = weekStart.AddDays(6);

            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var weeklyExpense = CalculateWeeklyExpenseAllocation(branchExpenses, weekStart, weekEnd);

            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = weeklyExpense,
                TotalProfit = totalRevenue,
                NetProfit = totalRevenue - weeklyExpense,
                ReportDate = weekStart,
                BranchId = g.Key.BranchId,
                BranchName = g.Key.BranchName
            };
        })
        .OrderByDescending(r => r.ReportDate)];
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

        return [.. orderGroups.Select(g =>
        {
            var monthStart = new DateTime(g.Key.Year, g.Key.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var monthlyExpense = CalculateMonthlyExpenseAllocation(branchExpenses, monthStart, monthEnd);

            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = monthlyExpense,
                TotalProfit = totalRevenue,
                NetProfit = totalRevenue - monthlyExpense,
                ReportDate = monthStart,
                BranchId = g.Key.BranchId,
                BranchName = g.Key.BranchName
            };
        })
        .OrderByDescending(r => r.ReportDate)];
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

        return [.. orderGroups.Select(g =>
        {
            var yearStart = new DateTime(g.Key.Year, 1, 1);
            var yearEnd = new DateTime(g.Key.Year, 12, 31);

            var branchExpenses = expenses.Where(e => e.BranchId == g.Key.BranchId);
            var yearlyExpense = CalculateYearlyExpenseAllocation(branchExpenses, yearStart, yearEnd);

            var totalRevenue = g.Sum(o => o.TotalMoney ?? 0);

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = yearlyExpense,
                TotalProfit = totalRevenue,
                NetProfit = totalRevenue - yearlyExpense,
                ReportDate = yearStart,
                BranchId = g.Key.BranchId,
                BranchName = g.Key.BranchName
            };
        })
        .OrderByDescending(r => r.ReportDate)];
    }

    private static decimal CalculateDailyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime targetDate)
    {
        var targetDateOnly = DateOnly.FromDateTime(targetDate);
        return expenses
            .Where(e =>
                targetDateOnly >= e.StartDate &&
                targetDateOnly <= (e.EndDate ?? e.StartDate)
            )
            .Sum(e =>
            {
                var endDate = e.EndDate ?? e.StartDate;
                var totalDays = (endDate.DayNumber - e.StartDate.DayNumber) + 1;
                return e.Amount / totalDays;
            });
    }

    private static decimal CalculateWeeklyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime weekStart, DateTime weekEnd)
    {
        decimal totalExpense = 0;

        for (var date = weekStart; date <= weekEnd; date = date.AddDays(1))
        {
            totalExpense += CalculateDailyExpenseAllocation(expenses, date);
        }

        return totalExpense;
    }

    private static decimal CalculateMonthlyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime monthStart, DateTime monthEnd)
    {
        var monthStartDateOnly = DateOnly.FromDateTime(monthStart);
        var monthEndDateOnly = DateOnly.FromDateTime(monthEnd);

        return expenses
            .Where(e => !(e.EndDate < monthStartDateOnly || e.StartDate > monthEndDateOnly))
            .Sum(e =>
            {
                var expenseStart = e.StartDate > monthStartDateOnly ? e.StartDate : monthStartDateOnly;
                var expenseEnd = (e.EndDate.HasValue ? (e.EndDate.Value < monthEndDateOnly ? e.EndDate.Value : monthEndDateOnly) : monthEndDateOnly);
                var overlapDays = (expenseEnd.DayNumber - expenseStart.DayNumber) + 1;
                var totalExpenseDays = ((e.EndDate ?? e.StartDate).DayNumber - e.StartDate.DayNumber) + 1;

                return (e.Amount * overlapDays) / totalExpenseDays;
            });
    }

    private static decimal CalculateYearlyExpenseAllocation(IEnumerable<BranchExpense> expenses, DateTime yearStart, DateTime yearEnd)
    {
        var yearStartDateOnly = DateOnly.FromDateTime(yearStart);
        var yearEndDateOnly = DateOnly.FromDateTime(yearEnd);

        return expenses
            .Where(e => !(e.EndDate < yearStartDateOnly || e.StartDate > yearEndDateOnly))
            .Sum(e =>
            {
                var expenseStart = e.StartDate > yearStartDateOnly ? e.StartDate : yearStartDateOnly;
                var expenseEnd = (e.EndDate.HasValue ? (e.EndDate.Value < yearEndDateOnly ? e.EndDate.Value : yearEndDateOnly) : yearEndDateOnly);
                var overlapDays = (expenseEnd.DayNumber - expenseStart.DayNumber) + 1;
                var totalExpenseDays = ((e.EndDate ?? e.StartDate).DayNumber - e.StartDate.DayNumber) + 1;

                return (e.Amount * overlapDays) / totalExpenseDays;
            });
    }

    private static int GetWeekOfYear(DateTime date)
    {
        return ISOWeek.GetWeekOfYear(date);
    }

    private static DateTime GetDateFromWeek(int year, int week)
    {
        return ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
    }
}