using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Services;
using Dashboard.Winform.ViewModels;
using System.ComponentModel;

namespace Dashboard.Winform.Presenters
{
    public interface ILandingDashboardPresenter
    {
        LandingDashboardModel Model { get; }
        event EventHandler? OnDataLoaded;
        Task LoadDashboardDataAsync();
        Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate);
    }

    public class LandingDashboardPresenter : ILandingDashboardPresenter
    {
        private readonly IReportingService _reportingService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ISupplierManagementService _supplierManagementService;
        private readonly IMapper _mapper;
        public LandingDashboardModel Model { get; }

        public event EventHandler? OnDataLoaded;

        public LandingDashboardPresenter(
            IReportingService reportingService,
            IProductService productService,
            ICustomerService customerService,
            ISupplierManagementService supplierManagementService,
            IMapper mapper
            )
        {
            _reportingService = reportingService;
            _customerService = customerService;
            _productService = productService;
            _supplierManagementService = supplierManagementService;
            _mapper = mapper;
            Model = new LandingDashboardModel();
        }

        public async Task LoadDashboardDataAsync()
        {
            var today = DateTime.Now;
            var startOfYear = new DateTime(today.Year, 1, 1);

            var previousYearStart = startOfYear.AddYears(-1);
            var previousYearEnd = today.AddYears(-1);

            var dashboardSummary = await _reportingService.GetDashboardSummaryAsync();
            var previousYearSummary = await _reportingService.GetDashboardSummaryAsync(previousYearStart, previousYearEnd);

            Model.TotalOrders = dashboardSummary.TotalOrders;
            Model.PendingOrders = dashboardSummary.PendingOrders;
            Model.TotalRevenue = dashboardSummary.TotalRevenue;
            Model.TotalExpenses = dashboardSummary.TotalExpenses;
            
            Model.SetPreviousPeriodData(
                previousYearSummary.TotalRevenue,
                previousYearSummary.TotalExpenses,
                previousYearSummary.TotalOrders,
                previousYearSummary.PendingOrders
            );

            Model.SupplierCount = await _supplierManagementService.GetSuppliersAsync(new GetSuppliersInput()).ContinueWith(t => t.Result.TotalRecords);
            Model.CustomerCount = await _customerService.GetCountAsync();
            Model.ProductCount = await _productService.GetCountAsync();
            Model.TopProducts = _mapper.Map<List<TopProductViewModel>>(dashboardSummary.TopProducts);
            Model.GrossRevenueList = _mapper.Map<List<RevenueByDateViewModel>>(dashboardSummary.FinacialReports);
            Model.StartDate = startOfYear;
            Model.EndDate = today;
            Model.PeriodDescription = "Tổng";

            if (dashboardSummary.UnderstockIngredients != null)
            {
                var understockProducts = dashboardSummary.UnderstockIngredients.Select(ingredient => new UnderstockProductViewModel
                {
                    ProductId = ingredient.Id,
                    ProductName = ingredient.Name,
                    ProductCode = $"ING-{ingredient.Id:D6}",
                    Category = ingredient.CategoryName,
                    CurrentStock = ingredient.InStockQuantity,
                    SafetyStock = ingredient.SafetyStock,
                    MaximumStock = ingredient.MaximumStock,
                    LastRestockDate = ingredient.UpdatedAt ?? ingredient.CreatedAt,
                    Location = "Kho chính"
                }).ToList();

                Model.UnderstockProducts.Clear();
                foreach (var item in understockProducts)
                    Model.UnderstockProducts.Add(item);
            }

            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadDashboardDataAsync(DateTime startDate, DateTime endDate)
        {
            var (previousStartDate, previousEndDate) = CalculatePreviousPeriod(startDate, endDate);

            var dashboardSummary = await _reportingService.GetDashboardSummaryAsync(startDate, endDate);
            
            var previousDashboardSummary = await _reportingService.GetDashboardSummaryAsync(previousStartDate, previousEndDate);

            if (startDate != endDate)
            {
                Model.TopProducts = _mapper.Map<List<TopProductViewModel>>(dashboardSummary.TopProducts);
                Model.GrossRevenueList = _mapper.Map<List<RevenueByDateViewModel>>(dashboardSummary.FinacialReports);
            }
            
            Model.TotalOrders = dashboardSummary.TotalOrders;
            Model.PendingOrders = dashboardSummary.PendingOrders;
            Model.TotalRevenue = dashboardSummary.TotalRevenue;
            Model.TotalExpenses = dashboardSummary.TotalExpenses;
            
            Model.SetPreviousPeriodData(
                previousDashboardSummary.TotalRevenue,
                previousDashboardSummary.TotalExpenses,
                previousDashboardSummary.TotalOrders,
                previousDashboardSummary.PendingOrders
            );

            Model.CustomerCount = await _customerService.GetCountAsync();
            Model.ProductCount = await _productService.GetCountAsync();
            Model.StartDate = startDate;
            Model.EndDate = endDate;

            var daysDiff = (endDate - startDate).Days;
            if (daysDiff == 0)
                Model.PeriodDescription = "Hôm nay";
            else if (daysDiff <= 7)
                Model.PeriodDescription = "7 ngày qua";
            else if (daysDiff <= 30)
                Model.PeriodDescription = "30 ngày qua";
            else
                Model.PeriodDescription = "Khoảng thời gian tùy chọn";

            if (dashboardSummary.UnderstockIngredients != null)
            {
                var understockProducts = dashboardSummary.UnderstockIngredients.Select(ingredient => new UnderstockProductViewModel
                {
                    ProductId = ingredient.Id,
                    ProductName = ingredient.Name,
                    ProductCode = $"ING-{ingredient.Id:D6}",
                    Category = ingredient.CategoryName,
                    CurrentStock = ingredient.InStockQuantity,
                    SafetyStock = ingredient.SafetyStock,
                    MaximumStock = ingredient.MaximumStock,
                    LastRestockDate = ingredient.UpdatedAt ?? ingredient.CreatedAt,
                    Location = "Kho chính"
                }).ToList();

                Model.UnderstockProducts = new BindingList<UnderstockProductViewModel>(understockProducts);
            }

            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        private static (DateTime startDate, DateTime endDate) CalculatePreviousPeriod(DateTime currentStartDate, DateTime currentEndDate)
        {
            var periodLength = (currentEndDate - currentStartDate).Days;
            
            if (periodLength == 0)
            {
                var previousDay = currentStartDate.AddDays(-1);
                return (previousDay, previousDay);
            }
            else if (periodLength <= 7) 
            {
                var previousEndDate = currentStartDate.AddDays(-1);
                var previousStartDate = previousEndDate.AddDays(-periodLength);
                return (previousStartDate, previousEndDate);
            }
            else if (periodLength <= 30) 
            {
                var previousEndDate = currentStartDate.AddDays(-1);
                var previousStartDate = previousEndDate.AddDays(-periodLength);
                return (previousStartDate, previousEndDate);
            }
            else 
            {
                var previousEndDate = currentStartDate.AddDays(-1);
                var previousStartDate = previousEndDate.AddDays(-periodLength);
                return (previousStartDate, previousEndDate);
            }
        }
    }
}