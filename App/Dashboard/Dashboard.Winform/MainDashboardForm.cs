using Dashboard.BussinessLogic.Services;

namespace Dashboard.Winform
{
    public partial class MainDashboardForm : Form
    {
        private readonly IProductService _productService;
        private readonly IServiceProvider _serviceProvider;

        public MainDashboardForm(IServiceProvider serviceProvider, IProductService productService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _productService = productService;


        }


    }
}
