using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Events
{
    public class SuppliersLoadedEventArgs : EventArgs
    {
        public List<SupplierViewModel> Suppliers { get; set; } = new();
        public int TotalCount { get; set; }
    }
}