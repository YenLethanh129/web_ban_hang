using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class LandingDashboardModel : INotifyPropertyChanged
    {
        // Main financial data
        private decimal _totalRevenue;
        private decimal _totalProfit;
        private decimal _totalExpenses;

        // Order Properties
        private int _totalOrders;
        private int _pendingOrders;

        // Counts
        private int _customerCount;
        private int _supplierCount;
        private int _productCount;
        
        // Current period info
        private DateTime _startDate;
        private DateTime _endDate;
        private string _periodDescription = "Tháng này";

        // Collections
        private List<UnderstockProductViewModel> _understockProducts = [];
        private List<TopProductViewModel> _topProducts = [];
        private List<BranchPerformanceViewModel> _branchPerformance = [];

        // Main Properties with unified approach
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                _totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
                OnPropertyChanged(nameof(TotalRevenueFormatted));
                OnPropertyChanged(nameof(ProfitMargin));
            }
        }

        public decimal TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
                OnPropertyChanged(nameof(TotalProfitFormatted));
                OnPropertyChanged(nameof(ProfitMargin));
                OnPropertyChanged(nameof(NetProfit));
            }
        }

        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set
            {
                _totalExpenses = value;
                OnPropertyChanged(nameof(TotalExpenses));
                OnPropertyChanged(nameof(TotalExpensesFormatted));
                OnPropertyChanged(nameof(NetProfit));
            }
        }

        public string PeriodDescription
        {
            get => _periodDescription;
            set
            {
                _periodDescription = value;
                OnPropertyChanged(nameof(PeriodDescription));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(PeriodDisplay));
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(PeriodDisplay));
            }
        }

        public int TotalOrders
        {
            get => _totalOrders;
            set
            {
                _totalOrders = value;
                OnPropertyChanged(nameof(TotalOrders));
            }
        }

        public int PendingOrders
        {
            get => _pendingOrders;
            set
            {
                _pendingOrders = value;
                OnPropertyChanged(nameof(PendingOrders));
            }
        }
        public int CustomerCount
        {
            get => _customerCount;
            set
            {
                _customerCount = value;
                OnPropertyChanged(nameof(CustomerCount));
            }
        }

        public int SupplierCount
        {
            get => _supplierCount;
            set
            {
                _supplierCount = value;
                OnPropertyChanged(nameof(SupplierCount));
            }
        }

        public int ProductCount
        {
            get => _productCount;
            set
            {
                _productCount = value;
                OnPropertyChanged(nameof(ProductCount));
            }
        }

        public List<UnderstockProductViewModel> UnderstockProducts
        {
            get => _understockProducts;
            set
            {
                _understockProducts = value;
                OnPropertyChanged(nameof(UnderstockProducts));
            }
        }
        public List<TopProductViewModel> TopProducts
        {
            get => _topProducts;
            set
            {
                _topProducts = value;
                OnPropertyChanged(nameof(TopProducts));
            }
        }

        public List<BranchPerformanceViewModel> BranchPerformance
        {
            get => _branchPerformance;
            set
            {
                _branchPerformance = value;
                OnPropertyChanged(nameof(BranchPerformance));
            }
        }

        // Formatted Properties
        public string TotalRevenueFormatted => _totalRevenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string TotalProfitFormatted => _totalProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string TotalExpensesFormatted => _totalExpenses.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        
        // Calculated Properties
        public decimal ProfitMargin => TotalRevenue > 0 ? (TotalProfit / TotalRevenue) * 100 : 0;
        public decimal NetProfit => TotalRevenue - TotalExpenses;
        public string NetProfitFormatted => NetProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string ProfitMarginFormatted => ProfitMargin.ToString("F2") + "%";
        
        // Period Display
        public string PeriodDisplay => StartDate != default && EndDate != default 
            ? $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}" 
            : PeriodDescription;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper methods for easy data updates
        public void UpdateFinancialData(decimal revenue, decimal profit, decimal expenses)
        {
            TotalRevenue = revenue;
            TotalProfit = profit;
            TotalExpenses = expenses;
        }

        public void UpdateOrderData(int totalOrders, int pendingOrders)
        {
            TotalOrders = totalOrders;
            PendingOrders = pendingOrders;
        }

        public void UpdateCountsData(int customers, int suppliers, int products)
        {
            CustomerCount = customers;
            SupplierCount = suppliers;
            ProductCount = products;
        }

        public void SetPeriod(DateTime start, DateTime end, string description = "")
        {
            StartDate = start;
            EndDate = end;
            if (!string.IsNullOrEmpty(description))
                PeriodDescription = description;
        }

        // Method to clear all data
        public void ClearData()
        {
            TotalRevenue = 0;
            TotalProfit = 0;
            TotalExpenses = 0;
            TotalOrders = 0;
            PendingOrders = 0;
            CustomerCount = 0;
            SupplierCount = 0;
            ProductCount = 0;
            UnderstockProducts.Clear();
            TopProducts.Clear();
            BranchPerformance.Clear();
        }
    }
}

public class TopProductViewModel
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
    public string RevenueFormatted => Revenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
}

public class BranchPerformanceViewModel
{
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal Profit { get; set; }
    public int OrderCount { get; set; }
    public string RevenueFormatted => Revenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
    public string ProfitFormatted => Profit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
}

public class RevenueReportViewModel
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetProfit { get; set; }
    public DateTime ReportDate { get; set; }
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string TotalRevenueFormatted => TotalRevenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
    public string NetProfitFormatted => NetProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
    public string ReportDateFormatted => ReportDate.ToString("dd/MM/yyyy");
}
public class UnderstockProductViewModel
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int SafetyStock { get; set; }
    public int MaximumStock { get; set; }
    public DateTime LastRestockDate { get; set; }
    public string Location { get; set; } = string.Empty;

    // Calculated properties
    public int StockDeficit => Math.Max(0, SafetyStock - CurrentStock);
    public double StockPercentage => MaximumStock > 0 ? (double)CurrentStock / MaximumStock * 100 : 0;
    public bool IsCritical => CurrentStock <= (SafetyStock * 0.5);
    public bool IsLowStock => CurrentStock <= SafetyStock && !IsCritical;
    public int RecommendedOrderQuantity => Math.Max(0, MaximumStock - CurrentStock);
    // Formatted properties
    public string CurrentStockFormatted => CurrentStock.ToString("N0");
    public string SafetyStockFormatted => SafetyStock.ToString("N0");
    public string MaximumStockFormatted => MaximumStock.ToString("N0");
    public string StockPercentageFormatted => StockPercentage.ToString("F1") + "%";
    public string LastRestockDateFormatted => LastRestockDate.ToString("dd/MM/yyyy");

    public string StockStatus
    {
        get
        {
            if (IsCritical) return "Nguy hiểm";
            if (IsLowStock) return "Thấp";
            return "Bình thường";
        }
    }

    public string StockStatusColor
    {
        get
        {
            if (IsCritical) return "Red";
            if (IsLowStock) return "Orange";
            return "Green";
        }
    }

    public string UrgencyLevel
    {
        get
        {
            if (IsCritical) return "Khẩn cấp";
            if (IsLowStock) return "Ưu tiên";
            return "Bình thường";
        }
    }
}
