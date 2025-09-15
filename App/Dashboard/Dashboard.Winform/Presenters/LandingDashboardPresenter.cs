using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ReportDtos;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Services;
using Dashboard.BussinessLogic.Services.Customers;
using Dashboard.BussinessLogic.Services.ReportServices;
using Dashboard.BussinessLogic.Services.SupplierServices;
using Dashboard.Winform.ViewModels;
using System.ComponentModel;

namespace Dashboard.Winform.Presenters
{
    public interface ILandingDashboardPresenter
    {
        LandingDashboardModel Model { get; }
        event EventHandler? OnDataLoaded;
        Task LoadDashboardDataAsync(DateTime? startDate = null, DateTime? endDate = null);
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

        public async Task LoadDashboardDataAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var today = DateTime.Now;
            var start = startDate ?? DateTime.Today.AddDays(-7);
            var end = endDate ?? today;

            var (previousStart, previousEnd) = CalculatePreviousPeriod(start, end);

            var dashboardSummary = await _reportingService.GetDashboardSummaryAsync(start, end);
            var previousDashboardSummary = await _reportingService.GetDashboardSummaryAsync(previousStart, previousEnd);

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
            Model.SupplierCount = await _supplierManagementService.GetSuppliersAsync(new GetSuppliersInput())
                                                .ContinueWith(t => t.Result.TotalRecords);

            Model.TopProducts = _mapper.Map<List<TopProductViewModel>>(dashboardSummary.TopProducts);
            Model.GrossRevenueList = _mapper.Map<List<FinancialReportByDateViewModel>>(dashboardSummary.FinacialReports);
            Model.StartDate = start;
            Model.EndDate = end;

            var daysDiff = (end - start).Days;
            Model.PeriodDescription = daysDiff switch
            {
                0 => "Hôm nay",
                <= 7 => "7 ngày qua",
                <= 30 => "30 ngày qua",
                _ => "Khoảng thời gian tùy chọn"
            };

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

            Model.UnderstockProducts.RaiseListChangedEvents = false;
            Model.UnderstockProducts.Clear();

            foreach (var product in understockProducts)
            {
                Model.UnderstockProducts.Add(product);
            }

            Model.UnderstockProducts.RaiseListChangedEvents = true;
            Model.UnderstockProducts.ResetBindings();

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