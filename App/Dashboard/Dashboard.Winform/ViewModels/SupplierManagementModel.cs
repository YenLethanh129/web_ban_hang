using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class SupplierManagementModel : IManagableModel
    {
        #region Fields
        private string _searchText = string.Empty;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private int _totalPages = 0;
        #endregion

        #region Properties
        public BindingList<SupplierViewModel> Suppliers { get; set; } = new();
        public BindingList<SupplierTypeViewModel> SupplierTypes { get; set; } = new();
        public List<string> Statuses { get; set; } = new() { "All", "ACTIVE", "INACTIVE" };

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                OnPropertyChanged(nameof(TotalItems));
                UpdateTotalPages();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }
        #endregion

        #region Methods
        private void UpdateTotalPages()
        {
            TotalPages = TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class SupplierViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        //public string? TaxCode { get; set; }
        //public string? BankAccount { get; set; }
        //public string? BankName { get; set; }
        public bool IsActive { get; set; }
        //public long? SupplierTypeId { get; set; }
        //public string SupplierTypeName { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //public string Status => IsActive ? "ACTIVE" : "INACTIVE";
        public int TotalOrders { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
    }

    public class SupplierDetailViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? TaxCode { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public bool IsActive { get; set; }
        public long? SupplierTypeId { get; set; }
        public string? SupplierTypeName { get; set; }
        public string? Note { get; set; }

        public BindingList<SupplierProductViewModel>? SupplierProducts { get; set; }
        public BindingList<PaymentTermViewModel>? PaymentTerms { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ProductsModified { get; set; } = false;
    }

    public class SupplierProductViewModel
    {
        public long Id { get; set; }
        public long? SupplierId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal SupplierPrice { get; set; }
        public string? SupplierProductCode { get; set; }
        public int MinOrderQuantity { get; set; } = 1;
        public int LeadTimeDays { get; set; } = 0;
        public bool IsPreferred { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentTermViewModel
    {
        public long Id { get; set; }
        public long? SupplierId { get; set; }
        public string TermName { get; set; } = string.Empty;
        public int DaysNet { get; set; } = 30;
        public decimal? DiscountPercentage { get; set; }
        public int? DiscountDays { get; set; }
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }

    public class SupplierTypeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }


    public class PurchaseOrderDetailViewModel
    {
        public long Id { get; set; }
        public string? OrderNumber { get; set; }
        public long SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? Notes { get; set; }
        public long? PaymentTermId { get; set; }
        public string? PaymentTermName { get; set; }

        public BindingList<PurchaseOrderItemViewModel> OrderItems { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ItemsModified { get; set; } = false;
    }

    public class PurchaseOrderItemViewModel
    {
        public long Id { get; set; }
        public long? PurchaseOrderId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class SupplierContactViewModel
    {
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}