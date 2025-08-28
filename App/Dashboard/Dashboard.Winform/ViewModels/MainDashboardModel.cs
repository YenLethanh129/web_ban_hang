using Dashboard.BussinessLogic.Services;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.Winform.ViewModels
{

    public struct RevenueByDate
    {
        public DateTime Date { get; set; }
        public decimal TotalOfAmount { get; set; }
    }
    public class MainDashboardModel
    {
        private DateTime StartDate;
        private DateTime EndDate;
        private int NumberDays;

        private readonly IServiceProvider _serviceProvider;
        private readonly IProductService _productService;

        public int NumberOfCustomers { get; private set; }
        public int NumberOfSuppliers { get; private set; }
        public int NumberOfProducts { get; private set; }
        public List<Product> TopProducts { get; private set; } = [];
        public List<KeyValuePair<string, int>> Understocks { get; private set; } = [];
        public List<RevenueByDate> GrossRevenue { get; private set; } = [];
        public int NumberOfOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }

        public MainDashboardModel(IServiceProvider serviceProvider, IProductService productService)
        {
            _serviceProvider = serviceProvider;
            _productService = productService;
        }

        private void GetNumberItems()
        {
            var products = _productService.GetProductsAsync(new BussinessLogic.Dtos.ProductDtos.GetProductsInput
            {
                PageNumber = 1,
                PageSize = int.MaxValue
            }).Result;

            NumberOfCustomers = products.TotalRecords;
            NumberOfSuppliers = products.TotalRecords;
            NumberOfProducts = products.TotalRecords;

        }
        public void LoadData(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            NumberDays = (EndDate - StartDate).Days + 1;
            GetNumberItems();
        }
    }
}
