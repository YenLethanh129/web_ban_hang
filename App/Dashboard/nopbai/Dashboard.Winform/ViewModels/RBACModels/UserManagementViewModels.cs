using System.ComponentModel;

namespace Dashboard.Winform.ViewModels.RBACModels
{
    public class UserManagementModel : IManagableModel
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private BindingList<UserViewModel> _users = [];
        private UserViewModel? _selectedUser;
        private string _searchText = string.Empty;
        private BindingList<string> _statuses = ["Tất cả", "Hoạt động", "Không hoạt động"];
        private BindingList<RoleViewModel> _roles = new();

        public BindingList<RoleViewModel> Roles
        {
            get => _roles;
            set
            {
                if (_roles != value)
                {
                    _roles = value;
                    OnPropertyChanged(nameof(Roles));
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
                }
            }
        }

        public int TotalPages
        {
            get => _totalItems == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
            set
            {
                if (_totalItems != value)
                {
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
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

        public BindingList<UserViewModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
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

        public UserViewModel? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class UserViewModel
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public long? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string StatusText => IsActive ? "Hoạt động" : "Không hoạt động";
    }

    public class UserDetailViewModel
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public bool IsActive { get; set; } = true;
        public long RoleId { get; set; }
        public RoleViewModel? Role { get; set; }
        public List<RoleViewModel> AssignedRoles { get; set; } = new();
        public long? EmployeeId { get; set; }
        public EmployeeSimpleViewModel? Employee { get; set; }
    }

    public class EmployeeSimpleViewModel
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public long PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public string DisplayText
        {
            get
            {
                var parts = new List<string> { FullName };
                if (!string.IsNullOrEmpty(PhoneNumber))
                    parts.Add($"SĐT: {PhoneNumber}");
                if (!string.IsNullOrEmpty(PositionName))
                    parts.Add($"Chức vụ: {PositionName}");
                return string.Join(" - ", parts);
            }
        }
    }

    public class UserRoleAssignmentViewModel
    {
        public long UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? EmployeeName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<RoleViewModel> AssignedRoles { get; set; } = new();
        public List<RoleViewModel> AvailableRoles { get; set; } = new();
    }
}