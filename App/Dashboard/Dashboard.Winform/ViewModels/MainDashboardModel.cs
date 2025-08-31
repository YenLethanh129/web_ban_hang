using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class MainDashboardModel : INotifyPropertyChanged
    {
        // Revenue Properties
        private decimal _todayRevenue;
        private decimal _monthlyRevenue;
        private decimal _yearlyRevenue;

        // Profit Properties
        private decimal _todayProfit;
        private decimal _monthlyProfit;
        private decimal _yearlyProfit;

        // Expenses Properties
        private decimal _todayExpenses;
        private decimal _monthlyExpenses;
        private decimal _yearlyExpenses;

        // Order Properties
        private int _totalOrders;
        private int _pendingOrders;

        private int _customerCount;
        private int _supplierCount;
        private int _productCount;
        private List<UnderstockProductViewModel> _understockProducts = [];

        // Collections
        private List<TopProductViewModel> _topProducts = [];
        private List<BranchPerformanceViewModel> _branchPerformance = [];

        public decimal TodayRevenue
        {
            get => _todayRevenue;
            set
            {
                _todayRevenue = value;
                OnPropertyChanged(nameof(TodayRevenue));
                OnPropertyChanged(nameof(TodayRevenueFormatted));
            }
        }

        public decimal MonthlyRevenue
        {
            get => _monthlyRevenue;
            set
            {
                _monthlyRevenue = value;
                OnPropertyChanged(nameof(MonthlyRevenue));
                OnPropertyChanged(nameof(MonthlyRevenueFormatted));
            }
        }

        public decimal YearlyRevenue
        {
            get => _yearlyRevenue;
            set
            {
                _yearlyRevenue = value;
                OnPropertyChanged(nameof(YearlyRevenue));
                OnPropertyChanged(nameof(YearlyRevenueFormatted));
            }
        }

        public decimal TodayProfit
        {
            get => _todayProfit;
            set
            {
                _todayProfit = value;
                OnPropertyChanged(nameof(TodayProfit));
                OnPropertyChanged(nameof(TodayProfitFormatted));
            }
        }

        public decimal MonthlyProfit
        {
            get => _monthlyProfit;
            set
            {
                _monthlyProfit = value;
                OnPropertyChanged(nameof(MonthlyProfit));
                OnPropertyChanged(nameof(MonthlyProfitFormatted));
            }
        }

        public decimal YearlyProfit
        {
            get => _yearlyProfit;
            set
            {
                _yearlyProfit = value;
                OnPropertyChanged(nameof(YearlyProfit));
                OnPropertyChanged(nameof(YearlyProfitFormatted));
            }
        }

        public decimal TodayExpenses
        {
            get => _todayExpenses;
            set
            {
                _todayExpenses = value;
                OnPropertyChanged(nameof(TodayExpenses));
                OnPropertyChanged(nameof(TodayExpensesFormatted));
            }
        }

        public decimal MonthlyExpenses
        {
            get => _monthlyExpenses;
            set
            {
                _monthlyExpenses = value;
                OnPropertyChanged(nameof(MonthlyExpenses));
                OnPropertyChanged(nameof(MonthlyExpensesFormatted));
            }
        }

        public decimal YearlyExpenses
        {
            get => _yearlyExpenses;
            set
            {
                _yearlyExpenses = value;
                OnPropertyChanged(nameof(YearlyExpenses));
                OnPropertyChanged(nameof(YearlyExpensesFormatted));
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

        public string TodayRevenueFormatted => _todayRevenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string MonthlyRevenueFormatted => _monthlyRevenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string YearlyRevenueFormatted => _yearlyRevenue.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));

        public string TodayProfitFormatted => _todayProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string MonthlyProfitFormatted => _monthlyProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string YearlyProfitFormatted => _yearlyProfit.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));

        public string TodayExpensesFormatted => _todayExpenses.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string MonthlyExpensesFormatted => _monthlyExpenses.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public string YearlyExpensesFormatted => _yearlyExpenses.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        public decimal TodayProfitMargin => TodayRevenue > 0 ? (TodayProfit / TodayRevenue) * 100 : 0;
        public decimal MonthlyProfitMargin => MonthlyRevenue > 0 ? (MonthlyProfit / MonthlyRevenue) * 100 : 0;
        public decimal YearlyProfitMargin => YearlyRevenue > 0 ? (YearlyProfit / YearlyRevenue) * 100 : 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    public int MinimumStock { get; set; }
    public int MaximumStock { get; set; }
    public decimal UnitPrice { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime LastRestockDate { get; set; }
    public string Location { get; set; } = string.Empty;

    // Calculated properties
    public int StockDeficit => Math.Max(0, MinimumStock - CurrentStock);
    public double StockPercentage => MaximumStock > 0 ? (double)CurrentStock / MaximumStock * 100 : 0;
    public bool IsCritical => CurrentStock <= (MinimumStock * 0.5);
    public bool IsLowStock => CurrentStock <= MinimumStock && !IsCritical;
    public int RecommendedOrderQuantity => Math.Max(0, MaximumStock - CurrentStock);
    public decimal EstimatedRestockCost => RecommendedOrderQuantity * UnitPrice;

    // Formatted properties
    public string CurrentStockFormatted => CurrentStock.ToString("N0");
    public string MinimumStockFormatted => MinimumStock.ToString("N0");
    public string MaximumStockFormatted => MaximumStock.ToString("N0");
    public string UnitPriceFormatted => UnitPrice.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
    public string EstimatedRestockCostFormatted => EstimatedRestockCost.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
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
