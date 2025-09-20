using Dashboard.DataAccess.Models.Entities.Orders;
using System.ComponentModel;

namespace Dashboard.Winform.ViewModels.InventoryModels
{
    public enum InventoryTabType
    {
        Transactions,
        Requests,
        TransferExecution
    }

    public class InventoryManagementModel : IManagableModel
    {
        // Transactions
        private int _transactionCurrentPage = 1;
        private int _transactionPageSize = 10;
        private int _transactionTotalItems = 0;
        private BindingList<InventoryTransactionViewModel> _transactions = new();
        private string _transactionSearchText = string.Empty;

        // Requests
        private int _requestCurrentPage = 1;
        private int _requestPageSize = 10;
        private int _requestTotalItems = 0;
        private BindingList<InventoryRequestViewModel> _requests = new();
        private string _requestSearchText = string.Empty;

        // Transfer execution
        private BindingList<IngredientInventoryViewModel> _availableIngredients = new();
        private BindingList<TransferItemViewModel> _transferItems = new();

        // Shared data
        private BindingList<BranchSimpleViewModel> _branches = new();
        private BindingList<string> _transactionTypes = new(["Tất cả", "IN", "OUT", "TRANSFER", "ADJUSTMENT"]);
        private BindingList<string> _requestStatuses = new(["Tất cả", "PENDING", "APPROVED", "REJECTED", "COMPLETED"]);
        private BindingList<string> _transferTypes = new(["Tất cả", "OUT", "IN"]);

        #region Transaction Properties
        public int TransactionCurrentPage
        {
            get => _transactionCurrentPage;
            set
            {
                if (_transactionCurrentPage != value)
                {
                    _transactionCurrentPage = value;
                    OnPropertyChanged(nameof(TransactionCurrentPage));
                }
            }
        }

        public int TransactionPageSize
        {
            get => _transactionPageSize;
            set
            {
                if (_transactionPageSize != value)
                {
                    _transactionPageSize = value;
                    OnPropertyChanged(nameof(TransactionPageSize));
                    OnPropertyChanged(nameof(TransactionTotalPages));
                    TransactionCurrentPage = 1;
                }
            }
        }

        public int TransactionTotalItems
        {
            get => _transactionTotalItems;
            set
            {
                if (_transactionTotalItems != value)
                {
                    _transactionTotalItems = value;
                    OnPropertyChanged(nameof(TransactionTotalItems));
                    OnPropertyChanged(nameof(TransactionTotalPages));
                }
            }
        }

        public int TransactionTotalPages =>
            TransactionTotalItems == 0 ? 0 : (int)Math.Ceiling((double)TransactionTotalItems / TransactionPageSize);

        public BindingList<InventoryTransactionViewModel> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public string TransactionSearchText
        {
            get => _transactionSearchText;
            set
            {
                if (_transactionSearchText != value)
                {
                    _transactionSearchText = value;
                    OnPropertyChanged(nameof(TransactionSearchText));
                }
            }
        }
        #endregion

        #region Request Properties
        public int RequestCurrentPage
        {
            get => _requestCurrentPage;
            set
            {
                if (_requestCurrentPage != value)
                {
                    _requestCurrentPage = value;
                    OnPropertyChanged(nameof(RequestCurrentPage));
                }
            }
        }

        public int RequestPageSize
        {
            get => _requestPageSize;
            set
            {
                if (_requestPageSize != value)
                {
                    _requestPageSize = value;
                    OnPropertyChanged(nameof(RequestPageSize));
                    OnPropertyChanged(nameof(RequestTotalPages));
                    RequestCurrentPage = 1;
                }
            }
        }

        public int RequestTotalItems
        {
            get => _requestTotalItems;
            set
            {
                if (_requestTotalItems != value)
                {
                    _requestTotalItems = value;
                    OnPropertyChanged(nameof(RequestTotalItems));
                    OnPropertyChanged(nameof(RequestTotalPages));
                }
            }
        }

        public int RequestTotalPages =>
            RequestTotalItems == 0 ? 0 : (int)Math.Ceiling((double)RequestTotalItems / RequestPageSize);

        public BindingList<InventoryRequestViewModel> Requests
        {
            get => _requests;
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        public string RequestSearchText
        {
            get => _requestSearchText;
            set
            {
                if (_requestSearchText != value)
                {
                    _requestSearchText = value;
                    OnPropertyChanged(nameof(RequestSearchText));
                }
            }
        }
        #endregion

        #region Transfer Properties
        public BindingList<IngredientInventoryViewModel> AvailableIngredients
        {
            get => _availableIngredients;
            set
            {
                _availableIngredients = value;
                OnPropertyChanged(nameof(AvailableIngredients));
            }
        }

        public BindingList<TransferItemViewModel> TransferItems
        {
            get => _transferItems;
            set
            {
                _transferItems = value;
                OnPropertyChanged(nameof(TransferItems));
            }
        }
        #endregion

        #region Shared Properties
        public BindingList<BranchSimpleViewModel> Branches
        {
            get => _branches;
            set
            {
                if (_branches != value)
                {
                    _branches = value;
                    OnPropertyChanged(nameof(Branches));
                }
            }
        }

        public BindingList<string> TransactionTypes
        {
            get => _transactionTypes;
            set
            {
                if (_transactionTypes != value)
                {
                    _transactionTypes = value;
                    OnPropertyChanged(nameof(TransactionTypes));
                }
            }
        }

        public BindingList<string> RequestStatuses
        {
            get => _requestStatuses;
            set
            {
                if (_requestStatuses != value)
                {
                    _requestStatuses = value;
                    OnPropertyChanged(nameof(RequestStatuses));
                }
            }
        }

        public BindingList<string> TransferTypes
        {
            get => _transferTypes;
            set
            {
                if (_transferTypes != value)
                {
                    _transferTypes = value;
                    OnPropertyChanged(nameof(TransferTypes));
                }
            }
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class InventoryTransactionViewModel
    {
        public long Id { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty; // IN, OUT, TRANSFER, ADJUSTMENT
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public string? ReferenceType { get; set; } // ORDER, PURCHASE, TRANSFER, ADJUSTMENT
        public long? ReferenceId { get; set; }
        public string? ReferenceCode { get; set; }
        public string? Notes { get; set; }
        public DateTime MovementDate { get; set; }
        public string? EmployeeName { get; set; }

        public string TransactionTypeText => TransactionType switch
        {
            "IN" => "Nhập",
            "OUT" => "Xuất",
            "TRANSFER" => "Chuyển kho",
            "ADJUSTMENT" => "Điều chỉnh",
            _ => TransactionType
        };

        public string ReferenceTypeText => ReferenceType switch
        {
            "ORDER" => "Đơn hàng",
            "PURCHASE" => "Mua hàng",
            "TRANSFER" => "Chuyển kho",
            "ADJUSTMENT" => "Điều chỉnh",
            _ => ReferenceType ?? ""
        };
    }

    public class InventoryRequestViewModel
    {
        public long Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string Status { get; set; } = string.Empty; // PENDING, APPROVED, REJECTED, COMPLETED
        public int TotalItems { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
        public List<InventoryRequestDetailViewModel> Details { get; set; } = new();

        public string StatusText => Status switch
        {
            "PENDING" => "Chờ duyệt",
            "APPROVED" => "Đã duyệt",
            "REJECTED" => "Từ chối",
            "COMPLETED" => "Hoàn thành",
            _ => Status
        };

        public string StatusColor => Status switch
        {
            "PENDING" => "#FFA500", // Orange
            "APPROVED" => "#00FF00", // Green
            "REJECTED" => "#FF0000", // Red
            "COMPLETED" => "#0000FF", // Blue
            _ => "#000000"
        };
    }

    public class InventoryRequestDetailViewModel
    {
        public long Id { get; set; }
        public long TransferRequestId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public decimal TransferredQuantity { get; set; }
        public string Status { get; set; } = string.Empty; // PENDING, APPROVED, REJECTED, TRANSFERRED
        public string? Note { get; set; }
        public decimal RemainingQuantity => (ApprovedQuantity ?? RequestedQuantity) - TransferredQuantity;

        public string StatusText => Status switch
        {
            "PENDING" => "Chờ duyệt",
            "APPROVED" => "Đã duyệt",
            "REJECTED" => "Từ chối",
            "TRANSFERRED" => "Đã chuyển",
            _ => Status
        };
    }

    public class IngredientInventoryViewModel
    {
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal AvailableQuantity { get; set; }
        public decimal SafetyStock { get; set; }
        public decimal? MaximumStock { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }

        public string StockStatus
        {
            get
            {
                if (AvailableQuantity <= SafetyStock)
                    return "Thiếu hàng";
                if (MaximumStock.HasValue && AvailableQuantity >= MaximumStock.Value)
                    return "Quá tồn";
                return "Bình thường";
            }
        }

        public string StockStatusColor => StockStatus switch
        {
            "Thiếu hàng" => "#FF0000", // Red
            "Quá tồn" => "#FFA500", // Orange
            "Bình thường" => "#00FF00", // Green
            _ => "#000000"
        };
    }

    public class TransferItemViewModel
    {
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal TransferQuantity { get; set; }
        public long? ToBranchId { get; set; }
        public string ToBranchName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime? RequestedDate { get; set; }
        public string TransferType { get; set; } = "OUT"; // OUT: từ warehouse đến branch, IN: từ branch về warehouse
        public string Status { get; set; } = "PENDING"; // PENDING, COMPLETED, CANCELLED

        public string TransferTypeText => TransferType switch
        {
            "OUT" => "Xuất kho",
            "IN" => "Nhập kho",
            _ => TransferType
        };

        public string StatusText => Status switch
        {
            "PENDING" => "Chờ xử lý",
            "COMPLETED" => "Hoàn thành",
            "CANCELLED" => "Đã hủy",
            _ => Status
        };
    }

    public class BranchSimpleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class IngredientSimpleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    // Purchase Order related ViewModels
    public class PurchaseOrderViewModel
    {
        public long Id { get; set; }
        public string PurchaseOrderCode { get; set; } = string.Empty;
        public long? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public long? BranchId { get; set; }
        public string? BranchName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmountBeforeTax { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal? TotalAmountAfterTax { get; set; }
        public decimal? FinalAmount { get; set; }
        public string? Note { get; set; }
        public List<PurchaseOrderDetailViewModel> Details { get; set; } = new();

        public string StatusText => Status switch
        {
            "DRAFT" => "Nháp",
            "PENDING" => "Chờ duyệt",
            "APPROVED" => "Đã duyệt",
            "ORDERED" => "Đã đặt hàng",
            "RECEIVED" => "Đã nhận hàng",
            "CANCELLED" => "Đã hủy",
            _ => Status ?? ""
        };
    }

    public class PurchaseOrderDetailViewModel
    {
        public long Id { get; set; }
        public long PurchaseOrderId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // Goods Received Note ViewModels
    public class GoodsReceivedNoteViewModel
    {
        public long Id { get; set; }
        public string GrnCode { get; set; } = string.Empty;
        public long? PurchaseOrderId { get; set; }
        public string? PurchaseOrderCode { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public DateTime? ReceivedDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalQuantityRejected { get; set; }
        public string? DeliveryNoteNumber { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DriverName { get; set; }
        public string? Note { get; set; }
        public List<GoodsReceivedDetailViewModel> Details { get; set; } = new();

        public decimal? TotalQuantityReceived { get; set; }
        public decimal? TotalQuantityOrdered { get; set; }

        public string StatusText => Status switch
        {
            "DRAFT" => "Nháp",
            "PENDING" => "Chờ xử lý",
            "COMPLETED" => "Hoàn thành",
            "CANCELLED" => "Đã hủy",
            _ => Status ?? ""
        };
    }

    public class GoodsReceivedDetailViewModel
    {
        public long Id { get; set; }
        public long GrnId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
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

        public decimal AcceptedQuantity => ReceivedQuantity - (RejectedQuantity ?? 0);
        public decimal VarianceQuantity => ReceivedQuantity - OrderedQuantity;
        public decimal VariancePercentage => OrderedQuantity == 0 ? 0 : (VarianceQuantity / OrderedQuantity) * 100;

        public string QualityStatusText => QualityStatus switch
        {
            "GOOD" => "Tốt",
            "DAMAGED" => "Hỏng",
            "EXPIRED" => "Hết hạn",
            "DEFECTIVE" => "Lỗi",
            _ => QualityStatus ?? ""
        };
    }
}

