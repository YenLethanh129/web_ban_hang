using Dashboard.DataAccess.Context;
using Dashboard.StockWorker.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.StockWorker.Services
{
    public class PurchaseEnrichmentService
    {
        private readonly WebbanhangDbContext _db;

        public PurchaseEnrichmentService(WebbanhangDbContext db)
        {
            _db = db;
        }

        public async Task EnrichAsync(List<StockAlert> alerts, CancellationToken cancellationToken = default)
        {
            if (alerts == null || !alerts.Any())
                return;

            var ingredientIds = alerts.Select(a => a.IngredientId).Distinct().ToList();

            var lastPricesList = await _db.SupplierIngredientPrices
                .Where(p => ingredientIds.Contains(p.IngredientId))
                .GroupBy(p => p.IngredientId)
                .Select(g => g
                    .OrderByDescending(x => x.EffectiveDate)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault())
                .ToListAsync(cancellationToken);

            var lastPrices = lastPricesList
                .Where(x => x != null)
                .ToDictionary(x => x!.IngredientId, x => x!);

            var lastPoDetails = await _db.IngredientPurchaseOrderDetails
                .Where(d => ingredientIds.Contains(d.IngredientId))
                .Include(d => d.PurchaseOrder)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync(cancellationToken);

            foreach (var alert in alerts)
            {
                var lastDetail = lastPoDetails.FirstOrDefault(d => d.IngredientId == alert.IngredientId);
                if (lastDetail != null && lastDetail.PurchaseOrder != null)
                {
                    var po = lastDetail.PurchaseOrder;
                    if (po.Supplier != null)
                    {
                        alert.SupplierName = po.Supplier.Name;
                        var contact = new List<string>();
                        if (!string.IsNullOrEmpty(po.Supplier.Phone)) contact.Add(po.Supplier.Phone);
                        if (!string.IsNullOrEmpty(po.Supplier.Email)) contact.Add(po.Supplier.Email);
                        alert.SupplierContact = contact.Any() ? string.Join(" / ", contact) : null;
                    }

                    alert.LastPurchasePrice = lastDetail.UnitPrice != 0 ? lastDetail.UnitPrice : alert.LastPurchasePrice;
                    alert.LastRestockDate = po.OrderDate != default ? po.OrderDate : alert.LastRestockDate;
                }

                if (!alert.LastPurchasePrice.HasValue && lastPrices.TryGetValue(alert.IngredientId, out var sip) && sip != null)
                {
                    alert.LastPurchasePrice = sip.Price;
                }
            }
        }
    }
}
