using Dashboard.Winform.Events;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.ViewModels.InventoryModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters
{
    public interface IInventoryManagementPresenter
    {
        IManagableModel Model { get; }
        event EventHandler<InventoryDataLoadedEventArgs> OnDataLoaded;

        // Data loading methods
        Task LoadDataAsync(InventoryTabType tabType, int page = 1, int pageSize = 10);
        Task RefreshCacheAsync(InventoryTabType tabType);

        // Search and filtering
        Task SearchAsync(InventoryTabType tabType, string searchText);
        Task FilterTransactionsAsync(string? transactionType, long branchId);
        Task FilterRequestsAsync(string? status, long branchId);
        Task FilterTransfersAsync(string? transferType, long branchId);

        // Pagination
        Task GoToNextPageAsync(InventoryTabType tabType);
        Task GoToPreviousPageAsync(InventoryTabType tabType);
        Task ChangePageSizeAsync(InventoryTabType tabType, int pageSize);

        // Sorting
        Task SortByAsync(InventoryTabType tabType, string sortBy);

        // Transaction management
        Task<InventoryTransactionDetailViewModel> GetTransactionDetailsAsync(long transactionId);
        Task<long> CreateTransactionAsync(CreateTransactionViewModel model);
        Task UpdateTransactionAsync(long id, UpdateTransactionViewModel model);
        Task DeleteTransactionAsync(long id);

        // Request management
        Task<InventoryRequestDetailViewModel> GetRequestDetailsAsync(long requestId);
        Task<long> CreateRequestAsync(CreateRequestViewModel model);
        Task UpdateRequestAsync(long id, UpdateRequestViewModel model);
        Task ApproveRequestAsync(long requestId);
        Task RejectRequestAsync(long requestId, string reason);
        Task CompleteRequestAsync(long requestId);

        // Transfer execution
        Task<List<IngredientInventoryViewModel>> GetAvailableIngredientsAsync(long branchId);
        Task<List<TransferItemViewModel>> ImportFromExcelAsync(string filePath);
        Task ExecuteTransferAsync(List<TransferItemViewModel> transferItems);
        Task SaveTransferAsync(List<TransferItemViewModel> transferItems);

        // Purchase order management
        Task<List<PurchaseOrderViewModel>> GetPurchaseOrdersAsync(long branchId, int page = 1, int pageSize = 10);
        Task<PurchaseOrderDetailViewModel> GetPurchaseOrderDetailsAsync(long purchaseOrderId);
        Task<long> CreatePurchaseOrderAsync(CreatePurchaseOrderViewModel model);
        Task UpdatePurchaseOrderAsync(long id, UpdatePurchaseOrderViewModel model);
        Task ApprovePurchaseOrderAsync(long purchaseOrderId);

        // Goods received note management
        Task<List<GoodsReceivedNoteViewModel>> GetGoodsReceivedNotesAsync(long branchId, int page = 1, int pageSize = 10);
        Task<GoodsReceivedNoteDetailViewModel> GetGoodsReceivedNoteDetailsAsync(long grnId);
        Task<long> CreateGoodsReceivedNoteAsync(CreateGoodsReceivedNoteViewModel model);
        Task UpdateGoodsReceivedNoteAsync(long id, UpdateGoodsReceivedNoteViewModel model);

        // Lookup data
        Task<List<BranchSimpleViewModel>> GetBranchesAsync();
        Task<List<IngredientSimpleViewModel>> GetIngredientsAsync(long? categoryId = null);
        Task<List<SupplierSimpleViewModel>> GetSuppliersAsync();
    }

    // Detail ViewModels for complete transaction/request information
    public class InventoryTransactionDetailViewModel : InventoryTransactionViewModel
    {
        public string? EmployeeFullName { get; set; }
        public string? ReferenceDescription { get; set; }
        public List<string> AttachedDocuments { get; set; } = new();
        public List<InventoryMovementHistoryViewModel> RelatedMovements { get; set; } = new();
    }

    public class InventoryRequestDetailViewModel : InventoryRequestViewModel
    {
        public string? ApprovedByFullName { get; set; }
        public string? RequestedByFullName { get; set; }
        public decimal TotalRequestedQuantity => Details.Sum(d => d.RequestedQuantity);
        public decimal TotalApprovedQuantity => Details.Sum(d => d.ApprovedQuantity ?? 0);
        public decimal CompletionPercentage => TotalApprovedQuantity == 0 ? 0 : (Details.Sum(d => d.TransferredQuantity) / TotalApprovedQuantity) * 100;
    }

    public class PurchaseOrderDetailViewModel : PurchaseOrderViewModel
    {
        public string? EmployeeFullName { get; set; }
        public string? SupplierAddress { get; set; }
        public string? SupplierPhone { get; set; }
        public List<GoodsReceivedNoteViewModel> RelatedGRNs { get; set; } = new();
        public bool CanEdit => Status == "DRAFT" || Status == "PENDING";
        public bool CanApprove => Status == "PENDING";
        public bool CanReceive => Status == "APPROVED" || Status == "ORDERED";
    }

    public class GoodsReceivedNoteDetailViewModel : GoodsReceivedNoteViewModel
    {
        public string? WarehouseStaffName { get; set; }
        public string? SupplierAddress { get; set; }
        public string? SupplierPhone { get; set; }
        public decimal TotalAcceptedQuantity => Details.Sum(d => d.AcceptedQuantity);
        public decimal OverallVariancePercentage => TotalQuantityOrdered == 0 ? 0 : ((TotalQuantityReceived ?? 0) - (TotalQuantityOrdered ?? 0)) / (TotalQuantityOrdered ?? 0) * 100;
        public bool HasQualityIssues => Details.Any(d => d.QualityStatus != "GOOD");
    }

    public class CreateTransactionViewModel
    {
        public string TransactionType { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public long IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public string? ReferenceType { get; set; }
        public long? ReferenceId { get; set; }
        public string? ReferenceCode { get; set; }
        public string? Notes { get; set; }
        public long? EmployeeId { get; set; }
    }

    public class UpdateTransactionViewModel : CreateTransactionViewModel
    {
        public long Id { get; set; }
    }

    public class CreateRequestViewModel
    {
        public long BranchId { get; set; }
        public DateTime RequiredDate { get; set; }
        public string? Note { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public List<CreateRequestDetailViewModel> Details { get; set; } = new();
    }

    public class CreateRequestDetailViewModel
    {
        public long IngredientId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateRequestViewModel : CreateRequestViewModel
    {
        public long Id { get; set; }
    }

    public class CreatePurchaseOrderViewModel
    {
        public long? SupplierId { get; set; }
        public long? BranchId { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? Note { get; set; }
        public List<CreatePurchaseOrderDetailViewModel> Details { get; set; } = new();
    }

    public class CreatePurchaseOrderDetailViewModel
    {
        public long IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxPrice { get; set; }
    }

    public class UpdatePurchaseOrderViewModel : CreatePurchaseOrderViewModel
    {
        public long Id { get; set; }
    }

    public class CreateGoodsReceivedNoteViewModel
    {
        public long? PurchaseOrderId { get; set; }
        public long? InvoiceId { get; set; }
        public long SupplierId { get; set; }
        public long BranchId { get; set; }
        public long? WarehouseStaffId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? DeliveryNoteNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DriverName { get; set; }
        public string? Note { get; set; }
        public List<CreateGoodsReceivedDetailViewModel> Details { get; set; } = new();
    }

    public class CreateGoodsReceivedDetailViewModel
    {
        public long IngredientId { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal? RejectedQuantity { get; set; }
        public string? QualityStatus { get; set; }
        public string? RejectionReason { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? BatchNumber { get; set; }
        public string? StorageLocation { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateGoodsReceivedNoteViewModel : CreateGoodsReceivedNoteViewModel
    {
        public long Id { get; set; }
    }

    // Additional helper ViewModels
    public class InventoryMovementHistoryViewModel
    {
        public DateTime MovementDate { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public string? ReferenceCode { get; set; }
        public string? Notes { get; set; }
    }

    public class SupplierSimpleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Excel import models
    public class ExcelImportResult<T>
    {
        public List<T> SuccessfulItems { get; set; } = new();
        public List<ExcelImportError> Errors { get; set; } = new();
        public int TotalProcessed { get; set; }
        public int SuccessCount => SuccessfulItems.Count;
        public int ErrorCount => Errors.Count;
    }

    public class ExcelImportError
    {
        public int RowNumber { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    // Filter and search models
    public class InventoryFilterModel
    {
        public InventoryTabType TabType { get; set; }
        public string? SearchText { get; set; }
        public long? BranchId { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<long>? IngredientIds { get; set; }
        public List<long>? CategoryIds { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // Inventory summary models
    public class InventorySummaryViewModel
    {
        public int TotalIngredients { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int OverStockItems { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int PendingRequests { get; set; }
        public int PendingTransfers { get; set; }
        public List<TopMovingIngredientViewModel> TopMovingIngredients { get; set; } = new();
    }

    public class TopMovingIngredientViewModel
    {
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalMovement { get; set; }
        public decimal AverageMovement { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}

namespace Dashboard.Winform.Events
{
    public class InventoryDataLoadedEventArgs : EventArgs
    {
        public InventoryTabType TabType { get; set; }
        public List<InventoryTransactionViewModel>? Transactions { get; set; }
        public List<InventoryRequestViewModel>? Requests { get; set; }
        public List<IngredientInventoryViewModel>? AvailableIngredients { get; set; }
        public List<BranchSimpleViewModel>? Branches { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public InventoryDataLoadedEventArgs(InventoryTabType tabType)
        {
            TabType = tabType;
        }
    }

    public class InventoryTransactionCreatedEventArgs : EventArgs
    {
        public long TransactionId { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public long IngredientId { get; set; }

        public InventoryTransactionCreatedEventArgs(long transactionId, string transactionCode, string transactionType, long branchId, long ingredientId)
        {
            TransactionId = transactionId;
            TransactionCode = transactionCode;
            TransactionType = transactionType;
            BranchId = branchId;
            IngredientId = ingredientId;
        }
    }

    public class InventoryRequestStatusChangedEventArgs : EventArgs
    {
        public long RequestId { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime StatusChangedDate { get; set; }

        public InventoryRequestStatusChangedEventArgs(long requestId, string requestNumber, string oldStatus, string newStatus, long branchId)
        {
            RequestId = requestId;
            RequestNumber = requestNumber;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            BranchId = branchId;
            StatusChangedDate = DateTime.Now;
        }
    }

    public class TransferExecutedEventArgs : EventArgs
    {
        public List<TransferItemViewModel> TransferItems { get; set; } = new();
        public long FromBranchId { get; set; }
        public List<long> ToBranchIds { get; set; } = new();
        public DateTime ExecutedDate { get; set; }
        public string? ExecutedBy { get; set; }
        public int TotalItemsTransferred { get; set; }

        public TransferExecutedEventArgs(List<TransferItemViewModel> transferItems, long fromBranchId)
        {
            TransferItems = transferItems;
            FromBranchId = fromBranchId;
            ToBranchIds = transferItems.Select(t => t.ToBranchId ?? 0).Distinct().ToList();
            ExecutedDate = DateTime.Now;
            TotalItemsTransferred = transferItems.Count;
        }
    }

    public class ExcelImportCompletedEventArgs : EventArgs
    {
        public string FilePath { get; set; } = string.Empty;
        public int TotalProcessed { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<ExcelImportError> Errors { get; set; } = new();
        public DateTime ImportedDate { get; set; }

        public ExcelImportCompletedEventArgs(string filePath, int totalProcessed, int successCount, int errorCount)
        {
            FilePath = filePath;
            TotalProcessed = totalProcessed;
            SuccessCount = successCount;
            ErrorCount = errorCount;
            ImportedDate = DateTime.Now;
        }
    }

    public class InventoryLevelChangedEventArgs : EventArgs
    {
        public long BranchId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal OldQuantity { get; set; }
        public decimal NewQuantity { get; set; }
        public decimal ChangeAmount => NewQuantity - OldQuantity;
        public string ChangeType { get; set; } = string.Empty; // IN, OUT, ADJUSTMENT
        public DateTime ChangedDate { get; set; }
        public string? Reason { get; set; }

        public InventoryLevelChangedEventArgs(long branchId, long ingredientId, string ingredientName, decimal oldQuantity, decimal newQuantity, string changeType)
        {
            BranchId = branchId;
            IngredientId = ingredientId;
            IngredientName = ingredientName;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            ChangeType = changeType;
            ChangedDate = DateTime.Now;
        }
    }
}