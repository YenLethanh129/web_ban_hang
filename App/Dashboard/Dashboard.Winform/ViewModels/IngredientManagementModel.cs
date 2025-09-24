using System;
using System.ComponentModel;
using System.Linq;

namespace Dashboard.Winform.ViewModels;
public class IngredientManagementModel : IManagableModel
{
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalItems = 0;

    private BindingList<IngredientViewModel> _ingredients = new BindingList<IngredientViewModel>();
    private IngredientViewModel? _selectedIngredient;
    private string _searchText = string.Empty;
    private BindingList<string> _statuses = new BindingList<string>(new[] { "All", "Active", "Inactive" });
    private BindingList<IngredientCategoryViewModel> _categories = new BindingList<IngredientCategoryViewModel>();

    public BindingList<IngredientCategoryViewModel> Categories
    {
        get => _categories;
        set
        {
            if (_categories != value)
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }

    public int TotalItems
    {
        get => _totalItems;
        set
        {
            if (_totalItems != value)
            {
                _totalItems = value;
                OnPropertyChanged(nameof(TotalItems));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
                CurrentPage = 1;
            }
        }
    }

    public int TotalPages
    {
        get => _totalItems == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
        // Keep TotalPages read-only from outside; remove any accidental stateful setter logic
    }

    public int ItemsStart
    {
        get => TotalItems == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;
    }

    public int ItemsEnd
    {
        get => TotalItems == 0 ? 0 : Math.Min(CurrentPage * PageSize, TotalItems);
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
            }
        }
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (_pageSize != value)
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
                CurrentPage = 1;
            }
        }
    }

    public BindingList<IngredientViewModel> Ingredients
    {
        get => _ingredients;
        set
        {
            _ingredients = value ?? new BindingList<IngredientViewModel>();
            OnPropertyChanged(nameof(Ingredients));
        }
    }

    public BindingList<string> Statuses
    {
        get => _statuses;
        set
        {
            if (_statuses != value)
            {
                _statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }

    public IngredientViewModel? SelectedIngredient
    {
        get => _selectedIngredient;
        set
        {
            if (_selectedIngredient != value)
            {
                _selectedIngredient = value;
                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void MapCategoriesToIngredients()
    {
        if (Categories == null || Categories.Count == 0 || Ingredients == null)
            return;

        var categoryDict = Categories.ToDictionary(c => c.Id, c => c.Name);

        foreach (var ingredient in Ingredients)
        {
            if (categoryDict.TryGetValue(ingredient.CategoryId, out var categoryName))
            {
                ingredient.CategoryName = categoryName;
            }
            else
            {
                ingredient.CategoryName = "Không rõ";
            }
        }
    }
}

public class IngredientViewModel
{
    private long _id = 1;
    private string _name = string.Empty;
    private string _unit = string.Empty;
    private decimal _costPerUnit = 0;
    private long _categoryId = 1;
    private string _categoryName = string.Empty;
    private string? _description;
    private bool _isActive = true;
    private long? _taxId;
    private DateTime _createdAt = DateTime.Now;
    private DateTime? _updatedAt;

    public long Id
    {
        get => _id;
        set => _id = value > 0 ? value : 1;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public string Unit
    {
        get => _unit;
        set => _unit = value ?? string.Empty;
    }

    public decimal CostPerUnit
    {
        get => _costPerUnit;
        set => _costPerUnit = value >= 0 ? value : 0;
    }

    public long CategoryId
    {
        get => _categoryId;
        set => _categoryId = value > 0 ? value : 1;
    }

    public string CategoryName
    {
        get => _categoryName;
        set => _categoryName = value ?? string.Empty;
    }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public long? TaxId
    {
        get => _taxId;
        set => _taxId = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value != default ? value : DateTime.Now;
    }

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value;
    }

    public string StatusText => IsActive ? "Hoạt động" : "Ngừng hoạt động";

    public IngredientViewModel()
    {
        Id = 1;
        Name = string.Empty;
        Unit = string.Empty;
        CostPerUnit = 0;
        CategoryId = 1;
        CategoryName = string.Empty;
        Description = null;
        IsActive = true;
        TaxId = null;
        CreatedAt = DateTime.Now;
        UpdatedAt = null;
        IsActive = true;
    }
}


public class IngredientDetailViewModel : IManagableModel
{
    private long _id;
    private string _name = string.Empty;
    private string _unit = string.Empty;
    private long _categoryId;
    private string _categoryName = string.Empty;
    private string? _description;
    private bool _isActive = true;
    private DateTime _createdAt;
    private DateTime? _updatedAt;
    private long? _taxId;

    public long? TaxId
    {
        get => _taxId;
        set
        {
            if (_taxId != value)
            {
                _taxId = value;
                OnPropertyChanged(nameof(TaxId));
            }
        }
    }

    public long Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value ?? string.Empty;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Unit
    {
        get => _unit; 
        set
        {
            if (_unit != value)
            {
                _unit = value ?? string.Empty;
                OnPropertyChanged(nameof(Unit));
            }
        }
    }

    public long CategoryId
    {
        get => _categoryId;
        set
        {
            if (_categoryId != value)
            {
                _categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }
    }

    public string CategoryName
    {
        get => _categoryName;
        set
        {
            if (_categoryName != value)
            {
                _categoryName = value ?? string.Empty;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set
        {
            if (_createdAt != value)
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
                OnPropertyChanged(nameof(CreatedAtFormatted));
            }
        }
    }

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set
        {
            if (_updatedAt != value)
            {
                _updatedAt = value;
                OnPropertyChanged(nameof(UpdatedAt));
                OnPropertyChanged(nameof(UpdatedAtFormatted));
            }
        }
    }

    public string Status => IsActive ? "Hoạt động" : "Ngừng hoạt động";
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
    public string UpdatedAtFormatted => UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa cập nhật";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// IngredientCategoryViewModel for dropdowns
public class IngredientCategoryViewModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public override string ToString() => Name;
}