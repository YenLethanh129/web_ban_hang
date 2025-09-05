using AutoMapper;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Services;

public interface ISupplierAnalyticsService
{
    Task<IEnumerable<SupplierSummaryDto>> GetSupplierSummariesAsync();
    Task<SupplierSummaryDto?> GetSupplierSummaryAsync(long supplierId);
    Task<IEnumerable<SupplierSummaryDto>> GetTopSuppliersByVolumeAsync(int topCount = 10);
    Task<IEnumerable<SupplierSummaryDto>> GetTopSuppliersByValueAsync(int topCount = 10);
    Task<IEnumerable<SupplierIngredientPriceDto>> GetPriceTrendsForIngredientAsync(long ingredientId, DateTime fromDate, DateTime toDate);
    Task<decimal> GetAveragePriceForIngredientAsync(long ingredientId);
    Task<IEnumerable<SupplierSummaryDto>> GetSuppliersWithoutRecentOrdersAsync(int daysThreshold = 90);
    Task<IEnumerable<SupplierSummaryDto>> GetActiveSuppliersByRegionAsync(string? region = null);
    Task<object> GetSupplierStatisticsAsync();
}

public class SupplierAnalyticsService : BaseTransactionalService, ISupplierAnalyticsService
{
    private readonly IMapper _mapper;

    public SupplierAnalyticsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<SupplierSummaryDto>> GetSupplierSummariesAsync()
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierSummaryDto>>(suppliers);
    }

    public async Task<SupplierSummaryDto?> GetSupplierSummaryAsync(long supplierId)
    {
        var specification = SupplierSpecifications.ById(supplierId);
        var supplier = await _unitOfWork.Repository<Supplier>()
            .GetWithSpecAsync(specification);

        return supplier != null ? _mapper.Map<SupplierSummaryDto>(supplier) : null;
    }

    public async Task<IEnumerable<SupplierSummaryDto>> GetTopSuppliersByVolumeAsync(int topCount = 10)
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        var suppliersList = suppliers.ToList();
        var topSuppliers = suppliersList
            .OrderByDescending(s => s.IngredientPurchaseOrders.Count)
            .Take(topCount)
            .ToList();

        return _mapper.Map<IEnumerable<SupplierSummaryDto>>(topSuppliers);
    }

    public async Task<IEnumerable<SupplierSummaryDto>> GetTopSuppliersByValueAsync(int topCount = 10)
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        var suppliersList = suppliers.ToList();
        var topSuppliers = suppliersList
            .OrderByDescending(s => s.IngredientPurchaseOrders.Sum(po => po.FinalAmount ?? 0))
            .Take(topCount)
            .ToList();

        return _mapper.Map<IEnumerable<SupplierSummaryDto>>(topSuppliers);
    }

    public async Task<IEnumerable<SupplierIngredientPriceDto>> GetPriceTrendsForIngredientAsync(long ingredientId, DateTime fromDate, DateTime toDate)
    {
        var specification = SupplierPriceSpecifications.ByIngredient(ingredientId);
        var prices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        var pricesList = prices.ToList();
        var filteredPrices = pricesList
            .Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate)
            .OrderBy(p => p.CreatedAt)
            .ToList();

        return _mapper.Map<IEnumerable<SupplierIngredientPriceDto>>(filteredPrices);
    }

    public async Task<decimal> GetAveragePriceForIngredientAsync(long ingredientId)
    {
        var specification = SupplierPriceSpecifications.ByIngredient(ingredientId);
        var prices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        var pricesList = prices.ToList();
        var now = DateTime.Now;
        var activePrices = pricesList
            .Where(p => (p.EffectiveDate == null || p.EffectiveDate <= now) &&
                       (p.ExpiredDate == null || p.ExpiredDate > now))
            .ToList();

        return activePrices.Count > 0 ? activePrices.Average(p => p.Price) : 0;
    }

    public async Task<IEnumerable<SupplierSummaryDto>> GetSuppliersWithoutRecentOrdersAsync(int daysThreshold = 90)
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        var suppliersList = suppliers.ToList();
        var thresholdDate = DateTime.Now.AddDays(-daysThreshold);
        var inactiveSuppliers = new List<Supplier>();

        foreach (var supplier in suppliersList)
        {
            var hasNoOrders = !supplier.IngredientPurchaseOrders.Any();
            var hasOnlyOldOrders = supplier.IngredientPurchaseOrders.All(po => po.CreatedAt < thresholdDate);
            
            if (hasNoOrders || hasOnlyOldOrders)
                inactiveSuppliers.Add(supplier);
        }

        return _mapper.Map<IEnumerable<SupplierSummaryDto>>(inactiveSuppliers);
    }

    public async Task<IEnumerable<SupplierSummaryDto>> GetActiveSuppliersByRegionAsync(string? region = null)
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        var suppliersList = suppliers.ToList();
        var activeSuppliers = new List<Supplier>();
        var now = DateTime.Now;

        foreach (var supplier in suppliersList)
        {
            // Filter by region if provided
            if (!string.IsNullOrEmpty(region))
            {
                if (supplier.Address == null || !supplier.Address.Contains(region, StringComparison.OrdinalIgnoreCase))
                    continue;
            }

            // Check if supplier has active prices
            var hasActivePrices = supplier.SupplierIngredientPrices.Any(sip => 
                (sip.EffectiveDate == null || sip.EffectiveDate <= now) &&
                (sip.ExpiredDate == null || sip.ExpiredDate > now));
                
            if (hasActivePrices)
                activeSuppliers.Add(supplier);
        }

        return _mapper.Map<IEnumerable<SupplierSummaryDto>>(activeSuppliers);
    }

    public async Task<object> GetSupplierStatisticsAsync()
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        var suppliersList = suppliers.ToList();
        var now = DateTime.Now;
        var last30Days = now.AddDays(-30);
        var last90Days = now.AddDays(-90);

        var totalSuppliers = suppliersList.Count;
        var activeSuppliers = 0;
        var suppliersWithRecentOrders = 0;
        var totalIngredients = 0;
        var totalPurchaseOrders = 0;
        var totalPurchaseValue = 0m;
        var recentOrdersCount = 0;
        var recentOrdersValue = 0m;
        var newSuppliersLast30Days = 0;
        var newSuppliersLast90Days = 0;
        var ordersCountLast90Days = 0;
        var ordersValueLast90Days = 0m;

        var ingredientIds = new HashSet<long>();

        foreach (var supplier in suppliersList)
        {
            // Check if supplier has active prices
            var hasActivePrices = supplier.SupplierIngredientPrices.Any(sip => 
                (sip.EffectiveDate == null || sip.EffectiveDate <= now) &&
                (sip.ExpiredDate == null || sip.ExpiredDate > now));
            if (hasActivePrices) activeSuppliers++;

            // Check if supplier has recent orders
            var hasRecentOrders = supplier.IngredientPurchaseOrders.Any(po => po.CreatedAt >= last30Days);
            if (hasRecentOrders) suppliersWithRecentOrders++;

            // Count ingredients
            foreach (var price in supplier.SupplierIngredientPrices)
            {
                ingredientIds.Add(price.IngredientId);
            }

            // Count orders and calculate values
            var supplierOrderCount = supplier.IngredientPurchaseOrders.Count;
            var supplierOrderValue = supplier.IngredientPurchaseOrders.Sum(po => po.FinalAmount ?? 0);
            
            totalPurchaseOrders += supplierOrderCount;
            totalPurchaseValue += supplierOrderValue;

            // Count recent orders
            foreach (var order in supplier.IngredientPurchaseOrders)
            {
                if (order.CreatedAt >= last30Days)
                {
                    recentOrdersCount++;
                    recentOrdersValue += order.FinalAmount ?? 0;
                }

                if (order.CreatedAt >= last90Days)
                {
                    ordersCountLast90Days++;
                    ordersValueLast90Days += order.FinalAmount ?? 0;
                }
            }

            // Count new suppliers
            if (supplier.CreatedAt >= last30Days) newSuppliersLast30Days++;
            if (supplier.CreatedAt >= last90Days) newSuppliersLast90Days++;
        }

        totalIngredients = ingredientIds.Count;
        var averageOrderValue = totalPurchaseOrders > 0 ? totalPurchaseValue / totalPurchaseOrders : 0;

        // Calculate top suppliers
        var supplierMetrics = suppliersList.Select(s => new
        {
            SupplierId = s.Id,
            SupplierName = s.Name,
            TotalValue = s.IngredientPurchaseOrders.Sum(po => po.FinalAmount ?? 0),
            TotalOrders = s.IngredientPurchaseOrders.Count
        }).ToList();

        var topSuppliersByValue = supplierMetrics
            .OrderByDescending(s => s.TotalValue)
            .Take(5)
            .ToList();

        var topSuppliersByOrders = supplierMetrics
            .OrderByDescending(s => s.TotalOrders)
            .Take(5)
            .ToList();

        return new
        {
            Overview = new
            {
                TotalSuppliers = totalSuppliers,
                ActiveSuppliers = activeSuppliers,
                InactiveSuppliers = totalSuppliers - activeSuppliers,
                SuppliersWithRecentOrders = suppliersWithRecentOrders,
                TotalIngredients = totalIngredients,
                ActivationRate = totalSuppliers > 0 ? Math.Round((decimal)activeSuppliers / totalSuppliers * 100, 2) : 0
            },
            PurchaseMetrics = new
            {
                TotalPurchaseOrders = totalPurchaseOrders,
                TotalPurchaseValue = totalPurchaseValue,
                AverageOrderValue = Math.Round(averageOrderValue, 2),
                RecentOrdersCount = recentOrdersCount,
                RecentOrdersValue = recentOrdersValue,
                RecentVsPreviousGrowth = CalculateGrowthRate(suppliersList, last30Days, last90Days)
            },
            TopPerformers = new
            {
                ByValue = topSuppliersByValue,
                ByVolume = topSuppliersByOrders
            },
            Trends = new
            {
                Last30Days = new
                {
                    NewSuppliers = newSuppliersLast30Days,
                    OrdersCount = recentOrdersCount,
                    OrdersValue = recentOrdersValue
                },
                Last90Days = new
                {
                    NewSuppliers = newSuppliersLast90Days,
                    OrdersCount = ordersCountLast90Days,
                    OrdersValue = ordersValueLast90Days
                }
            }
        };
    }

    private static decimal CalculateGrowthRate(List<Supplier> suppliers, DateTime recent30Days, DateTime recent90Days)
    {
        var current30DaysValue = 0m;
        var previous30DaysValue = 0m;

        foreach (var supplier in suppliers)
        {
            foreach (var order in supplier.IngredientPurchaseOrders)
            {
                if (order.CreatedAt >= recent30Days)
                {
                    current30DaysValue += order.FinalAmount ?? 0;
                }
                else if (order.CreatedAt >= recent90Days && order.CreatedAt < recent30Days)
                {
                    previous30DaysValue += order.FinalAmount ?? 0;
                }
            }
        }

        if (previous30DaysValue == 0)
            return current30DaysValue > 0 ? 100 : 0;

        return Math.Round((current30DaysValue - previous30DaysValue) / previous30DaysValue * 100, 2);
    }
}
