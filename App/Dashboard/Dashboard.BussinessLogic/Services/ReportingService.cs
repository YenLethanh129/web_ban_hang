//using Microsoft.EntityFrameworkCore;
//using Dashboard.DataAccess.Context;
//using Dashboard.DataAccess.Models.Entities;

//namespace Dashboard.BussinessLogic.Services;

//public interface IReportingService
//{
//    Task<List<SalesSummaryView>> GetSalesSummaryAsync(long? branchId = null, int? year = null, int? month = null);
//    Task<List<ExpensesSummaryView>> GetExpensesSummaryAsync(long? branchId = null, int? year = null, int? month = null);
//    Task<List<ProfitSummaryView>> GetProfitSummaryAsync(long? branchId = null, int? year = null, int? month = null);
//    Task<List<InventoryStatusView>> GetInventoryStatusAsync(long? branchId = null, string? stockStatus = null);
//    Task<List<EmployeePayrollView>> GetEmployeePayrollAsync(long? branchId = null);
//    Task<List<ProductWithPricesView>> GetProductsWithPricesAsync(long? categoryId = null);
//}

//public class ReportingService : IReportingService
//{
//    private readonly WebbanhangDbContext _context;

//    public ReportingService(WebbanhangDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<SalesSummaryView>> GetSalesSummaryAsync(long? branchId = null, int? year = null, int? month = null)
//    {
//        var query = _context.SalesSummaryViews.AsQueryable();

//        if (branchId.HasValue)
//            query = query.Where(x => x.BranchId == branchId);
        
//        if (year.HasValue)
//            query = query.Where(x => x.Year == year);
        
//        if (month.HasValue)
//            query = query.Where(x => x.Month == month);

//        return await query
//            .OrderByDescending(x => x.Year)
//            .ThenByDescending(x => x.Month)
//            .ThenBy(x => x.BranchId)
//            .ToListAsync();
//    }

//    public async Task<List<ExpensesSummaryView>> GetExpensesSummaryAsync(long? branchId = null, int? year = null, int? month = null)
//    {
//        var query = _context.ExpensesSummaryViews.AsQueryable();

//        if (branchId.HasValue)
//            query = query.Where(x => x.BranchId == branchId);
        
//        if (year.HasValue)
//            query = query.Where(x => x.Year == year);
        
//        if (month.HasValue)
//            query = query.Where(x => x.Month == month);

//        return await query
//            .OrderByDescending(x => x.Year)
//            .ThenByDescending(x => x.Month)
//            .ThenBy(x => x.BranchId)
//            .ToListAsync();
//    }

//    public async Task<List<ProfitSummaryView>> GetProfitSummaryAsync(long? branchId = null, int? year = null, int? month = null)
//    {
//        var query = _context.ProfitSummaryViews.AsQueryable();

//        if (branchId.HasValue)
//            query = query.Where(x => x.BranchId == branchId);
        
//        if (year.HasValue)
//            query = query.Where(x => x.Year == year);
        
//        if (month.HasValue)
//            query = query.Where(x => x.Month == month);

//        return await query
//            .OrderByDescending(x => x.Year)
//            .ThenByDescending(x => x.Month)
//            .ThenBy(x => x.BranchId)
//            .ToListAsync();
//    }

//    public async Task<List<InventoryStatusView>> GetInventoryStatusAsync(long? branchId = null, string? stockStatus = null)
//    {
//        var query = _context.InventoryStatusViews.AsQueryable();

//        if (branchId.HasValue)
//            query = query.Where(x => x.BranchId == branchId);
        
//        if (!string.IsNullOrEmpty(stockStatus))
//            query = query.Where(x => x.StockStatus == stockStatus);

//        return await query
//            .OrderBy(x => x.BranchName)
//            .ThenBy(x => x.LocationName)
//            .ThenBy(x => x.IngredientName)
//            .ToListAsync();
//    }

//    public async Task<List<EmployeePayrollView>> GetEmployeePayrollAsync(long? branchId = null)
//    {
//        var query = _context.EmployeePayrollViews.AsQueryable();

//        if (branchId.HasValue)
//            query = query.Where(x => x.Employee.BranchId == branchId);

//        return await query
//            .OrderBy(x => x.BranchName)
//            .ThenBy(x => x.FullName)
//            .ToListAsync();
//    }

//    public async Task<List<ProductWithPricesView>> GetProductsWithPricesAsync(long? categoryId = null)
//    {
//        var query = _context.ProductWithPricesViews.AsQueryable();

//        if (categoryId.HasValue)
//            query = query.Where(x => x.Product.CategoryId == categoryId);

//        return await query
//            .Where(x => x.IsActive)
//            .OrderBy(x => x.CategoryName)
//            .ThenBy(x => x.Name)
//            .ToListAsync();
//    }
//}
