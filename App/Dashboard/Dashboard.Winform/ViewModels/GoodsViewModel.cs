using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class GoodsManagementModel : IManagableModel, INotifyPropertyChanged
    {
        #region Fields
        private string _searchText = string.Empty;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private int _totalPages = 0;
        #endregion

        #region Properties
        //public BindingList<ProductViewModel> Products { get; set; } = new();
        //public BindingList<CategoryViewModel> Categories { get; set; } = new();
        //public BindingList<TaxViewModel> Taxes { get; set; } = new();
        public List<string> Statuses { get; set; } = new() { "All", "ACTIVE", "INACTIVE" };

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                OnPropertyChanged(nameof(TotalItems));
                UpdateTotalPages();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }
        #endregion

        #region Methods
        private void UpdateTotalPages()
        {
            TotalPages = TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
