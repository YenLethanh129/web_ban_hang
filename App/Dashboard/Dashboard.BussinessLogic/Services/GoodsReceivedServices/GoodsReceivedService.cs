using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.GoodsReceivedDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.FinacialAndReports;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services.GoodsReceivedServices;

public interface IGoodsReceivedService
{
    Task ProcessGoodsReceivedAsync(ProcessGoodsReceivedInput input);
    Task ExportGoodsToBranchAsync(ExportGoodsToBranchInput input);
    Task UpdateCogsSummaryAsync(long branchId, DateTime receivedDate);
}

public class GoodsReceivedService : BaseTransactionalService, IGoodsReceivedService
{
    public GoodsReceivedService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task ProcessGoodsReceivedAsync(ProcessGoodsReceivedInput input)
    {
        var grn = new GoodsReceivedNote
        {
            GrnCode = input.GrnCode,
            SupplierId = input.SupplierId,
            BranchId = input.BranchId,
            WarehouseStaffId = input.WarehouseStaffId,
            StatusId = 1,
            TotalQuantityOrdered = input.GoodsReceivedDetails.Sum(x => x.OrderedQuantity),
            TotalQuantityReceived = input.GoodsReceivedDetails.Sum(x => x.ReceivedQuantity),
            TotalQuantityRejected = input.GoodsReceivedDetails.Sum(x => x.RejectedQuantity),
            LastModified = DateTime.UtcNow
        };

        await _unitOfWork.Repository<GoodsReceivedNote>().AddAsync(grn);
        await _unitOfWork.SaveChangesAsync();

        foreach (var detail in input.GoodsReceivedDetails)
        {
            var grnDetail = new GoodsReceivedDetail
            {
                GrnId = grn.Id,
                IngredientId = detail.IngredientId,
                OrderedQuantity = detail.OrderedQuantity,
                ReceivedQuantity = detail.ReceivedQuantity,
                RejectedQuantity = detail.RejectedQuantity,
                QualityStatus = detail.QualityStatus,
                CreatedAt = DateTime.UtcNow,
                //LastModified = DateTime.UtcNow
            };

            await _unitOfWork.Repository<GoodsReceivedDetail>().AddAsync(grnDetail);

            var warehouseSpec = WarehouseInventorySpecifications.ByIngredient(detail.IngredientId);
            var warehouseStock = await _unitOfWork.Repository<IngredientWarehouse>().GetWithSpecAsync(warehouseSpec);

            if (warehouseStock == null)
                throw new InvalidOperationException($"Warehouse inventory for ingredient ID {detail.IngredientId} not found");

            warehouseStock.Quantity += detail.ReceivedQuantity;
            //warehouseStock.LastModified = DateTime.UtcNow;
            _unitOfWork.Repository<IngredientWarehouse>().Update(warehouseStock);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExportGoodsToBranchAsync(ExportGoodsToBranchInput input)
    {
        var warehouseSpec = WarehouseInventorySpecifications.ByIngredient(input.IngredientId);
        var warehouseStock = await _unitOfWork.Repository<IngredientWarehouse>().GetWithSpecAsync(warehouseSpec);

        if (warehouseStock == null || warehouseStock.Quantity < input.QuantityToExport)
        {
            throw new InvalidOperationException("Not enough.");
        }

        // Cập nhật kho tổng
        warehouseStock.Quantity -= input.QuantityToExport;
        //warehouseStock.LastModified = DateTime.UtcNow;
        _unitOfWork.Repository<IngredientWarehouse>().Update(warehouseStock);

        // Cập nhật kho chi nhánh
        var branchWarehouseSpec = BranchInventorySpecifications.ByBranchAndIngredient(input.BranchId, input.IngredientId);
        var branchWarehouseStock = await _unitOfWork.Repository<BranchIngredientInventory>().GetWithSpecAsync(branchWarehouseSpec);

        if (branchWarehouseStock == null)
            throw new InvalidOperationException($"Branch inventory for ingredient ID {input.IngredientId} not found");

        branchWarehouseStock.Quantity += input.QuantityToExport;
        branchWarehouseStock.LastModified = DateTime.UtcNow;
        _unitOfWork.Repository<BranchIngredientInventory>().Update(branchWarehouseStock);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCogsSummaryAsync(long branchId, DateTime receivedDate)
    {
        var startOfMonth = new DateTime(receivedDate.Year, receivedDate.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1);

        var spec = GoodsReceivedNoteSpecification.ByBranchAndDate(branchId, startOfMonth, endOfMonth);
        var grns = await _unitOfWork.Repository<GoodsReceivedNote>().GetAllWithSpecAsync(spec);

        var totalPurchaseOrders = grns.Count();
        int totalIngredients = grns.SelectMany(x => x.GoodsReceivedDetails)
            .Select(x => x.IngredientId)
            .Distinct()
            .Count();
        decimal? expenseBeforeTax = grns.SelectMany(x => x.GoodsReceivedDetails)
            .Sum(x => x.UnitPrice * x.ReceivedQuantity);

        decimal taxRate = 0.1m; // Giả lập tax
        decimal? taxAmount = expenseBeforeTax * taxRate;
        decimal? expenseAfterTax = expenseBeforeTax + taxAmount;

        string periodValue = receivedDate.ToString("yyyy-MM");

        var existingSummary = await _unitOfWork.Repository<CogsSummary>()
            .GetAllWithSpecAsync(new Specification<CogsSummary>(x => x.BranchId == branchId && x.PeriodValue == periodValue));

        if (!existingSummary.Any())
        {
            var summary = new CogsSummary
            {
                BranchId = branchId,
                PeriodType = "MONTHLY",
                PeriodValue = periodValue,
                TotalPurchaseOrders = totalPurchaseOrders,
                TotalIngredients = totalIngredients,
                ExpenseBeforeTax = expenseBeforeTax ?? 0,
                ExpenseAfterTax = expenseAfterTax ?? 0,
                TaxAmount = taxAmount ?? 0,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            await _unitOfWork.Repository<CogsSummary>().AddAsync(summary);
        }
        else
        {
            var summary = existingSummary.First();
            summary.TotalPurchaseOrders = totalPurchaseOrders;
            summary.TotalIngredients = totalIngredients;
            summary.ExpenseBeforeTax = expenseBeforeTax ?? 0;
            summary.ExpenseAfterTax = expenseAfterTax ?? 0;
            summary.TaxAmount = taxAmount ?? 0;
            summary.LastModified = DateTime.UtcNow;

            _unitOfWork.Repository<CogsSummary>().Update(summary);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
