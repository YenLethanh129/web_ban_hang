using Dashboard.Winform.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dashboard.Winform.ViewModels.RBACModels;

public class RolePermissionManagementModel : ViewModelBase, IManagableModel
{
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalItems = 0;
    private string _searchText = string.Empty;
    private bool _isLoading = false;

    private BindingList<RoleViewModel> _roles;
    private BindingList<PermissionViewModel> _permissions;
    private RoleViewModel? _selectedRole;
    private PermissionViewModel? _selectedPermission;

    public RolePermissionManagementModel()
    {
        _roles = new BindingList<RoleViewModel>();
        _permissions = new BindingList<PermissionViewModel>();

        // Enable change notifications for better performance
        _roles.AllowNew = true;
        _roles.AllowEdit = true;
        _roles.AllowRemove = true;
        _roles.RaiseListChangedEvents = true;

        _permissions.AllowNew = true;
        _permissions.AllowEdit = true;
        _permissions.AllowRemove = true;
        _permissions.RaiseListChangedEvents = true;
    }

    public int TotalItems
    {
        get => _totalItems;
        set
        {
            if (SetProperty(ref _totalItems, value))
            {
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
                OnPropertyChanged(nameof(HasPreviousPage));
                OnPropertyChanged(nameof(HasNextPage));
            }
        }
    }

    public int TotalPages
    {
        get => _totalItems == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
    }

    public int ItemsStart
    {
        get => TotalItems == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;
    }

    public int ItemsEnd
    {
        get => TotalItems == 0 ? 0 : Math.Min(CurrentPage * PageSize, TotalItems);
    }

    public bool HasPreviousPage
    {
        get => CurrentPage > 1;
    }

    public bool HasNextPage
    {
        get => CurrentPage < TotalPages;
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (SetProperty(ref _currentPage, value))
            {
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
                OnPropertyChanged(nameof(HasPreviousPage));
                OnPropertyChanged(nameof(HasNextPage));
            }
        }
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (SetProperty(ref _pageSize, value))
            {
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ItemsStart));
                OnPropertyChanged(nameof(ItemsEnd));
                OnPropertyChanged(nameof(HasPreviousPage));
                OnPropertyChanged(nameof(HasNextPage));
                CurrentPage = 1; // Reset to first page when page size changes
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public BindingList<RoleViewModel> Roles
    {
        get => _roles;
        set => SetProperty(ref _roles, value);
    }

    public BindingList<PermissionViewModel> Permissions
    {
        get => _permissions;
        set => SetProperty(ref _permissions, value);
    }

    public RoleViewModel? SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value);
    }

    public PermissionViewModel? SelectedPermission
    {
        get => _selectedPermission;
        set => SetProperty(ref _selectedPermission, value);
    }

    // Utility methods for pagination
    public string PaginationText => $"Hiển thị {ItemsStart}-{ItemsEnd} trong tổng số {TotalItems} mục";

    public void ResetPagination()
    {
        CurrentPage = 1;
        TotalItems = 0;
    }

    public void UpdatePagination(int totalItems, int currentPage = -1)
    {
        TotalItems = totalItems;
        if (currentPage > 0 && currentPage <= TotalPages)
        {
            CurrentPage = currentPage;
        }
        else if (CurrentPage > TotalPages && TotalPages > 0)
        {
            CurrentPage = TotalPages;
        }
    }
}

// Role ViewModel with optimized property change notifications
public class RoleViewModel : ViewModelBase
{
    private long _id;
    private string _name = string.Empty;
    private string? _description;
    private DateTime _createdAt;
    private DateTime? _updatedAt;
    private int _permissionCount;
    private List<PermissionViewModel> _permissions = new List<PermissionViewModel>();

    public long Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => SetProperty(ref _updatedAt, value);
    }

    public int PermissionCount
    {
        get => _permissionCount;
        set => SetProperty(ref _permissionCount, value);
    }

    public List<PermissionViewModel> Permissions
    {
        get => _permissions;
        set => SetProperty(ref _permissions, value);
    }

    // Display properties for UI
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
    public string UpdatedAtFormatted => UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa cập nhật";
    public string PermissionCountText => $"{PermissionCount} quyền";

    // For comparison and search optimization
    public override bool Equals(object? obj)
    {
        return obj is RoleViewModel other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}

// Permission ViewModel with optimized property change notifications
public class PermissionViewModel : ViewModelBase
{
    private long _id;
    private string _name = string.Empty;
    private string? _description;
    private string _resource = string.Empty;
    private string _action = string.Empty;
    private DateTime _createdAt;
    private DateTime? _updatedAt;
    private bool _isAssigned; // For role assignment

    public long Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string Resource
    {
        get => _resource;
        set => SetProperty(ref _resource, value);
    }

    public string Action
    {
        get => _action;
        set => SetProperty(ref _action, value);
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    public DateTime? UpdatedAt
    {
        get => _updatedAt;
        set => SetProperty(ref _updatedAt, value);
    }

    public bool IsAssigned
    {
        get => _isAssigned;
        set => SetProperty(ref _isAssigned, value);
    }

    // Display properties for UI
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
    public string UpdatedAtFormatted => UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa cập nhật";
    public string FullPermissionName => $"{Resource}:{Action}";
    public string AssignmentStatus => IsAssigned ? "Đã gán" : "Chưa gán";

    // For comparison and search optimization
    public override bool Equals(object? obj)
    {
        return obj is PermissionViewModel other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    // Search optimization method
    public bool MatchesSearchTerm(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return true;

        var term = searchTerm.ToLowerInvariant();
        return Id.ToString().Contains(term) ||
               Name.ToLowerInvariant().Contains(term) ||
               Resource.ToLowerInvariant().Contains(term) ||
               Action.ToLowerInvariant().Contains(term) ||
               (Description?.ToLowerInvariant().Contains(term) ?? false);
    }
}

// Role Detail ViewModel for Add/Edit with validation
public class RoleDetailViewModel : ViewModelBase
{
    private long _id;
    private string _name = string.Empty;
    private string? _description;
    private List<PermissionViewModel> _allPermissions = new List<PermissionViewModel>();
    private List<long> _assignedPermissionIds = new List<long>();
    private bool _isDirty = false;

    public long Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                IsDirty = true;
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (SetProperty(ref _description, value))
            {
                IsDirty = true;
            }
        }
    }

    public List<PermissionViewModel> AllPermissions
    {
        get => _allPermissions;
        set => SetProperty(ref _allPermissions, value);
    }

    public List<long> AssignedPermissionIds
    {
        get => _assignedPermissionIds;
        set
        {
            if (SetProperty(ref _assignedPermissionIds, value))
            {
                IsDirty = true;
                OnPropertyChanged(nameof(AssignedPermissionsCount));
            }
        }
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => SetProperty(ref _isDirty, value);
    }

    // Validation properties
    public bool IsValid => !string.IsNullOrWhiteSpace(Name);
    public string ValidationMessage => IsValid ? string.Empty : "Tên role không được để trống";

    // Display properties
    public bool IsNewRole => Id == 0;
    public string ActionText => IsNewRole ? "Thêm Role" : "Sửa Role";
    public int AssignedPermissionsCount => AssignedPermissionIds.Count;

    // Helper methods
    public void MarkAsClean()
    {
        IsDirty = false;
    }

    public List<PermissionViewModel> GetAssignedPermissions()
    {
        var assignedIds = new HashSet<long>(AssignedPermissionIds);
        return AllPermissions.Where(p => assignedIds.Contains(p.Id)).ToList();
    }

    public List<PermissionViewModel> GetUnassignedPermissions()
    {
        var assignedIds = new HashSet<long>(AssignedPermissionIds);
        return AllPermissions.Where(p => !assignedIds.Contains(p.Id)).ToList();
    }
}

// Permission Detail ViewModel for Add/Edit with validation
public class PermissionDetailViewModel : ViewModelBase
{
    private long _id;
    private string _name = string.Empty;
    private string? _description;
    private string _resource = string.Empty;
    private string _action = string.Empty;
    private bool _isDirty = false;

    public long Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                IsDirty = true;
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (SetProperty(ref _description, value))
            {
                IsDirty = true;
            }
        }
    }

    public string Resource
    {
        get => _resource;
        set
        {
            if (SetProperty(ref _resource, value))
            {
                IsDirty = true;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(FullPermissionName));
            }
        }
    }

    public string Action
    {
        get => _action;
        set
        {
            if (SetProperty(ref _action, value))
            {
                IsDirty = true;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(FullPermissionName));
            }
        }
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => SetProperty(ref _isDirty, value);
    }

    // Validation properties
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) &&
                           !string.IsNullOrWhiteSpace(Resource) &&
                           !string.IsNullOrWhiteSpace(Action);

    public string ValidationMessage
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Name))
                return "Tên quyền không được để trống";
            if (string.IsNullOrWhiteSpace(Resource))
                return "Resource không được để trống";
            if (string.IsNullOrWhiteSpace(Action))
                return "Action không được để trống";
            return string.Empty;
        }
    }

    // Display properties
    public bool IsNewPermission => Id == 0;
    public string ActionText => IsNewPermission ? "Thêm Permission" : "Sửa Permission";
    public string FullPermissionName => $"{Resource}:{Action}";

    // Helper methods
    public void MarkAsClean()
    {
        IsDirty = false;
    }

    public PermissionViewModel ToPermissionViewModel()
    {
        return new PermissionViewModel
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Resource = Resource,
            Action = Action
        };
    }
}


// Search and Sort options for better maintainability
public static class RoleSortOptions
{
    public const string Id = "id";
    public const string Name = "name";
    public const string CreatedAt = "createdat";
    public const string PermissionCount = "permissioncount";

    public static readonly string[] ValidOptions = { Id, Name, CreatedAt, PermissionCount };

    public static bool IsValid(string sortBy) => ValidOptions.Contains(sortBy?.ToLower());
}

public static class PermissionSortOptions
{
    public const string Id = "id";
    public const string Name = "name";
    public const string Resource = "resource";
    public const string Action = "action";
    public const string CreatedAt = "createdat";

    public static readonly string[] ValidOptions = { Id, Name, Resource, Action, CreatedAt };

    public static bool IsValid(string sortBy) => ValidOptions.Contains(sortBy?.ToLower());
}