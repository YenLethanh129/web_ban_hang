using AutoMapper;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Services;

public interface ISupplierPerformanceService
{
    Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformancesAsync(long supplierId);
    Task<IEnumerable<SupplierPerformanceDto>> GetPerformancesByPeriodAsync(string evaluationPeriod, string periodValue);
    Task<SupplierPerformanceDto?> GetSupplierPerformanceAsync(long supplierId, string evaluationPeriod, string periodValue);
    Task<SupplierPerformanceDto> CreateOrUpdatePerformanceAsync(long supplierId, string evaluationPeriod, string periodValue);
    Task<IEnumerable<SupplierPerformanceDto>> GetTopPerformersAsync(int topCount = 10);
    Task<IEnumerable<SupplierPerformanceDto>> GetRecentPerformanceAsync(DateTime fromDate);
    Task<bool> DeletePerformanceAsync(long id);
    Task<bool> CalculatePerformanceMetricsAsync(long supplierId, string evaluationPeriod, string periodValue);
}

public class SupplierPerformanceService : BaseTransactionalService, ISupplierPerformanceService
{
    private readonly IMapper _mapper;

    public SupplierPerformanceService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<SupplierPerformanceDto>> GetSupplierPerformancesAsync(long supplierId)
    {
        var specification = SupplierPerformanceSpecifications.BySupplier(supplierId);
        var performances = await _unitOfWork.Repository<SupplierPerformance>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierPerformanceDto>>(performances.OrderByDescending(p => p.CreatedAt));
    }

    public async Task<IEnumerable<SupplierPerformanceDto>> GetPerformancesByPeriodAsync(string evaluationPeriod, string periodValue)
    {
        var specification = SupplierPerformanceSpecifications.ByPeriod(evaluationPeriod, periodValue);
        var performances = await _unitOfWork.Repository<SupplierPerformance>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierPerformanceDto>>(performances.OrderByDescending(p => p.OverallRating));
    }

    public async Task<SupplierPerformanceDto?> GetSupplierPerformanceAsync(long supplierId, string evaluationPeriod, string periodValue)
    {
        var specification = SupplierPerformanceSpecifications.BySupplierAndPeriod(supplierId, evaluationPeriod, periodValue);
        var performance = await _unitOfWork.Repository<SupplierPerformance>()
            .GetWithSpecAsync(specification);

        return performance != null ? _mapper.Map<SupplierPerformanceDto>(performance) : null;
    }

    public async Task<SupplierPerformanceDto> CreateOrUpdatePerformanceAsync(long supplierId, string evaluationPeriod, string periodValue)
    {
        // Validate supplier exists
        await ValidateSupplierAsync(supplierId);

        // Validate period parameters
        if (string.IsNullOrEmpty(evaluationPeriod))
            throw new ArgumentException("Evaluation period is required");

        if (string.IsNullOrEmpty(periodValue))
            throw new ArgumentException("Period value is required");

        // Check if performance record already exists
        var existingPerformance = await GetSupplierPerformanceAsync(supplierId, evaluationPeriod, periodValue);
        
        if (existingPerformance != null)
        {
            // Update existing performance
            await CalculatePerformanceMetricsAsync(supplierId, evaluationPeriod, periodValue);
            return await GetSupplierPerformanceAsync(supplierId, evaluationPeriod, periodValue)
                ?? throw new InvalidOperationException("Failed to retrieve updated performance");
        }
        else
        {
            // Create new performance record
            var performance = new SupplierPerformance
            {
                SupplierId = supplierId,
                EvaluationPeriod = evaluationPeriod,
                PeriodValue = periodValue
            };

            await _unitOfWork.Repository<SupplierPerformance>().AddAsync(performance);
            await _unitOfWork.SaveChangesAsync();

            // Calculate metrics
            await CalculatePerformanceMetricsAsync(supplierId, evaluationPeriod, periodValue);

            return await GetSupplierPerformanceAsync(supplierId, evaluationPeriod, periodValue)
                ?? throw new InvalidOperationException("Failed to retrieve created performance");
        }
    }

    public async Task<IEnumerable<SupplierPerformanceDto>> GetTopPerformersAsync(int topCount = 10)
    {
        var specification = SupplierPerformanceSpecifications.TopPerformers(topCount);
        var performances = await _unitOfWork.Repository<SupplierPerformance>()
            .GetAllWithSpecAsync(specification, true);

        var topPerformers = performances
            .Where(p => p.OverallRating.HasValue)
            .OrderByDescending(p => p.OverallRating)
            .Take(topCount)
            .ToList();

        return _mapper.Map<IEnumerable<SupplierPerformanceDto>>(topPerformers);
    }

    public async Task<IEnumerable<SupplierPerformanceDto>> GetRecentPerformanceAsync(DateTime fromDate)
    {
        var specification = SupplierPerformanceSpecifications.RecentPerformance(fromDate);
        var performances = await _unitOfWork.Repository<SupplierPerformance>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierPerformanceDto>>(performances.OrderByDescending(p => p.CreatedAt));
    }

    public async Task<bool> DeletePerformanceAsync(long id)
    {
        var existingPerformance = await _unitOfWork.Repository<SupplierPerformance>().GetAsync(id);
        if (existingPerformance == null)
            throw new InvalidOperationException($"Supplier performance with ID {id} not found");

        _unitOfWork.Repository<SupplierPerformance>().Remove(existingPerformance);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CalculatePerformanceMetricsAsync(long supplierId, string evaluationPeriod, string periodValue)
    {
        try
        {
            // Get the performance record
            var specification = SupplierPerformanceSpecifications.BySupplierAndPeriod(supplierId, evaluationPeriod, periodValue);
            var performance = await _unitOfWork.Repository<SupplierPerformance>()
                .GetWithSpecAsync(specification);

            if (performance == null)
                return false;

            // Calculate date range based on evaluation period
            var (startDate, endDate) = GetDateRangeForPeriod(evaluationPeriod, periodValue);

            // Get supplier with related data
            var supplierSpec = SupplierSpecifications.ById(supplierId);
            var supplier = await _unitOfWork.Repository<Supplier>()
                .GetWithSpecAsync(supplierSpec);

            if (supplier == null)
                return false;

            // Calculate metrics
            var purchaseOrders = supplier.IngredientPurchaseOrders
                .Where(po => po.CreatedAt >= startDate && po.CreatedAt <= endDate)
                .ToList();

            var totalOrders = purchaseOrders.Count;
            var totalAmount = purchaseOrders.Sum(po => po.FinalAmount ?? 0);

            // Calculate delivery performance (simplified - would need actual delivery data)
            var deliveredOrders = purchaseOrders.Count(po => po.Status != null && po.Status.Name == "Delivered"); // Check Status.Name
            var onTimeDeliveries = (int)(deliveredOrders * 0.9); // Simplified calculation

            // Calculate quality score (simplified - would need actual quality data)
            var qualityScore = totalOrders > 0 ? (int)(85 + (totalAmount / totalOrders / 1000 * 5)) : 0; // Simplified calculation
            qualityScore = Math.Min(qualityScore, 100);

            // Calculate overall rating
            var onTimeRate = totalOrders > 0 ? (decimal)onTimeDeliveries / totalOrders : 0;
            var qualityRate = qualityScore / 100m;
            var overallRating = (onTimeRate * 0.4m + qualityRate * 0.4m + 0.2m) * 100; // Simplified calculation

            // Update performance record
            performance.TotalOrders = totalOrders;
            performance.TotalAmount = totalAmount;
            performance.OnTimeDeliveries = onTimeDeliveries;
            performance.QualityScore = qualityScore;
            performance.OverallRating = overallRating;

            _unitOfWork.Repository<SupplierPerformance>().Remove(performance);
            _unitOfWork.Repository<SupplierPerformance>().Add(performance);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> ValidateSupplierAsync(long supplierId)
    {
        var supplier = await _unitOfWork.Repository<Supplier>().GetAsync(supplierId);
        if (supplier == null)
            throw new InvalidOperationException($"Supplier with ID {supplierId} not found");

        return true;
    }

    private static (DateTime startDate, DateTime endDate) GetDateRangeForPeriod(string evaluationPeriod, string periodValue)
    {
        var currentDate = DateTime.Now;
        
        return evaluationPeriod.ToLower() switch
        {
            "monthly" => GetMonthlyDateRange(periodValue),
            "quarterly" => GetQuarterlyDateRange(periodValue),
            "yearly" => GetYearlyDateRange(periodValue),
            _ => (currentDate.AddMonths(-1), currentDate)
        };
    }

    private static (DateTime startDate, DateTime endDate) GetMonthlyDateRange(string periodValue)
    {
        // periodValue format: "2024-01"
        if (DateTime.TryParse($"{periodValue}-01", out var date))
        {
            var startDate = new DateTime(date.Year, date.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }
        
        var currentDate = DateTime.Now;
        return (new DateTime(currentDate.Year, currentDate.Month, 1), 
                currentDate);
    }

    private static (DateTime startDate, DateTime endDate) GetQuarterlyDateRange(string periodValue)
    {
        // periodValue format: "2024-Q1"
        var parts = periodValue.Split('-');
        if (parts.Length == 2 && int.TryParse(parts[0], out var year) && parts[1].StartsWith("Q"))
        {
            var quarter = int.Parse(parts[1].Substring(1));
            var startMonth = (quarter - 1) * 3 + 1;
            var startDate = new DateTime(year, startMonth, 1);
            var endDate = startDate.AddMonths(3).AddDays(-1);
            return (startDate, endDate);
        }

        var currentDate = DateTime.Now;
        var currentQuarter = (currentDate.Month - 1) / 3 + 1;
        var quarterStartMonth = (currentQuarter - 1) * 3 + 1;
        return (new DateTime(currentDate.Year, quarterStartMonth, 1), 
                new DateTime(currentDate.Year, quarterStartMonth + 2, DateTime.DaysInMonth(currentDate.Year, quarterStartMonth + 2)));
    }

    private static (DateTime startDate, DateTime endDate) GetYearlyDateRange(string periodValue)
    {
        // periodValue format: "2024"
        if (int.TryParse(periodValue, out var year))
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            return (startDate, endDate);
        }

        var currentDate = DateTime.Now;
        return (new DateTime(currentDate.Year, 1, 1), 
                new DateTime(currentDate.Year, 12, 31));
    }
}
