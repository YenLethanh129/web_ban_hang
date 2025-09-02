using Dashboard.BussinessLogic.Services;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Presenters
{
    public interface IDashboardPresenter
    {
        MainDashboardModel Model { get; }
        event EventHandler? OnDataLoaded;
        Task LoadDashboardDataAsync();
        Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate);
    }

    public class DashboardPresenter : IDashboardPresenter
    {
        private readonly IOrderService _orderService;
        private readonly IReportingService _reportingService;

        public MainDashboardModel Model { get; }

        public event EventHandler? OnDataLoaded;

        public DashboardPresenter(
            IOrderService orderService,
            IReportingService reportingService)
        {
            _orderService = orderService;
            _reportingService = reportingService;
            Model = new MainDashboardModel();
        }

        public async Task LoadDashboardDataAsync()
        {
            var today = DateTime.Now;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var orderSummary = await _orderService.GetOrderSummaryAsync(startOfMonth, today);
            var dashboardSummary = await _reportingService.GetDashboardSummaryAsync();

            Model.TotalOrders = orderSummary.TotalOrders;
            Model.PendingOrders = orderSummary.PendingOrders;
            Model.TotalRevenue = orderSummary.TotalRevenue;
            Model.StartDate = startOfMonth;
            Model.EndDate = today;
            Model.PeriodDescription = "Tháng này";

            // Map understock ingredients to understock products
            if (dashboardSummary.UnderstockIngredients != null)
            {
                var understockProducts = dashboardSummary.UnderstockIngredients.Select(ingredient => new UnderstockProductViewModel
                {
                    ProductId = ingredient.Id,
                    ProductName = ingredient.Name,
                    ProductCode = $"ING-{ingredient.Id:D6}",
                    Category = ingredient.CategoryName,
                    CurrentStock = ingredient.InStockQuantity,
                    SafetyStock = 20, // Default - could be improved to get from InventoryThreshold
                    MaximumStock = 200, // Default - could be improved to get from InventoryThreshold
                    UnitPrice = ingredient.CostPerUnit,
                    Supplier = "Chưa xác định",
                    LastRestockDate = ingredient.UpdatedAt ?? ingredient.CreatedAt,
                    Location = "Kho chính"
                }).ToList();

                Model.UnderstockProducts = understockProducts;
            }

            OnDataLoaded?.Invoke(this, EventArgs.Empty);

        }
        
        public async Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate)
        {

            var orderSummary = await _orderService.GetOrderSummaryAsync(startDate, endDate);
            var dashboardSummary = await _reportingService.GetDashboardSummaryAsync();
            
            Model.TotalOrders = orderSummary.TotalOrders;
            Model.PendingOrders = orderSummary.PendingOrders;
            Model.TotalRevenue = orderSummary.TotalRevenue;
            Model.StartDate = startDate;
            Model.EndDate = endDate;
            
            // Set period description based on date range
            var daysDiff = (endDate - startDate).Days;
            if (daysDiff == 0)
                Model.PeriodDescription = "Hôm nay";
            else if (daysDiff <= 7)
                Model.PeriodDescription = "7 ngày qua";
            else if (daysDiff <= 30)
                Model.PeriodDescription = "30 ngày qua";
            else
                Model.PeriodDescription = "Khoảng thời gian tùy chọn";

            // Map understock ingredients to understock products
            if (dashboardSummary.UnderstockIngredients != null)
            {
                var understockProducts = dashboardSummary.UnderstockIngredients.Select(ingredient => new UnderstockProductViewModel
                {
                    ProductId = ingredient.Id,
                    ProductName = ingredient.Name,
                    ProductCode = $"ING-{ingredient.Id:D6}",
                    Category = ingredient.CategoryName,
                    CurrentStock = ingredient.InStockQuantity,
                    SafetyStock = 20, // Default - could be improved to get from InventoryThreshold
                    MaximumStock = 200, // Default - could be improved to get from InventoryThreshold
                    UnitPrice = ingredient.CostPerUnit,
                    Supplier = "Chưa xác định",
                    LastRestockDate = ingredient.UpdatedAt ?? ingredient.CreatedAt,
                    Location = "Kho chính"
                }).ToList();

                Model.UnderstockProducts = understockProducts;
            }
            
            OnDataLoaded?.Invoke(this, EventArgs.Empty);

        }
    }
}