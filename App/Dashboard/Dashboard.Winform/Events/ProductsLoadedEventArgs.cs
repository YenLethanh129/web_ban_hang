using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Events
{
    public class ProductsLoadedEventArgs : EventArgs
    {
        public List<ProductViewModel> Products { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
