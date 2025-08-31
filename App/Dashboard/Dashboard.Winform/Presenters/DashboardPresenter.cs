using Dashboard.BussinessLogic.Services;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Presenters
{
    public interface IDashboardPresenter
    {
        MainDashboardModel Model { get; }
        event EventHandler? OnDataLoaded;
        event EventHandler<string>? OnError;
        Task LoadDashboardDataAsync();
        Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate);
    }

    public class DashboardPresenter : IDashboardPresenter
    {
        private readonly IOrderService _orderService;
        private readonly IReportingService _reportingService;

        public MainDashboardModel Model { get; }

        public event EventHandler? OnDataLoaded;
        public event EventHandler<string>? OnError;

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
            try
            {
                var today = DateTime.Now;
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var startOfYear = new DateTime(today.Year, 1, 1);

                var orderSummary = await _orderService.GetOrderSummaryAsync(startOfMonth, today);

                Model.TotalOrders = orderSummary.TotalOrders;
                Model.PendingOrders = orderSummary.PendingOrders;
                Model.MonthlyRevenue = orderSummary.TotalRevenue;

                OnDataLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error loading dashboard data: {ex.Message}");
            }
        }
        
        public async Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var orderSummary = await _orderService.GetOrderSummaryAsync(startDate, endDate);
                Model.TotalOrders = orderSummary.TotalOrders;
                Model.PendingOrders = orderSummary.PendingOrders;
                Model.MonthlyRevenue = orderSummary.TotalRevenue;
                OnDataLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error loading dashboard data: {ex.Message}");
            }
        }
    }
}