using Dashboard.Winform.Events;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace Dashboard.Winform.ViewModels
{
    public class ProductManagementModel : IManagableModel, INotifyPropertyChanged
    {
        #region Fields
        private string _searchText = string.Empty;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private int _totalPages = 0;
        #endregion

        #region Properties
        public BindingList<ProductViewModel> Products { get; set; } = new();
        public BindingList<CategoryViewModel> Categories { get; set; } = new();
        public BindingList<TaxViewModel> Taxes { get; set; } = new();
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

    public class RecipeManagementModel : IManagableModel, INotifyPropertyChanged
    {
        #region Fields
        private string _searchText = string.Empty;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private int _totalPages = 0;
        #endregion

        #region Properties
        public BindingList<RecipeViewModel> Recipes { get; set; } = new();
        public BindingList<ProductViewModel> Products { get; set; } = new();
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
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public long? CategoryId { get; set; }
        public long? TaxId { get; set; }
        public string? TaxName { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status => IsActive ? "ACTIVE" : "INACTIVE";
        public int SoldQuantity { get; set; }
    }

    public class ProductDetailViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public long? TaxId { get; set; }
        public string? TaxName { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public BindingList<ProductImageViewModel> ProductImages { get; set; } = new();
        public BindingList<RecipeViewModel> Recipes { get; set; } = new();
        public BindingList<ProductRecipeViewModel> ProductRecipes { get; set; } = new();
    }

    public class ProductImageViewModel
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RecipeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ServingSize { get; set; } = 1;
        public string Unit { get; set; } = "portion";
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status => IsActive ? "ACTIVE" : "INACTIVE";
    }

    public class RecipeDetailViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ServingSize { get; set; } = 1;
        public string Unit { get; set; } = "portion";
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public BindingList<RecipeIngredientViewModel> RecipeIngredients { get; set; } = new();
    }

    public class RecipeIngredientViewModel
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal? WastePercentage { get; set; } = 0;
        public decimal ActualQuantityNeeded => Quantity * (1 + (WastePercentage ?? 0) / 100);
        public string? Notes { get; set; }
        public bool IsOptional { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ProductRecipeViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class TaxViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}