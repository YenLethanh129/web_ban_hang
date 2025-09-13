using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        private List<UserViewModel> _users = new();
        private List<RoleViewModel> _roles = new();
        private List<PermissionViewModel> _permissions = new();
        private UserViewModel? _selectedUser;
        private RoleViewModel? _selectedRole;
        private string _searchText = string.Empty;
        private bool _isActiveFilter = true;

        public List<UserViewModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
                OnPropertyChanged(nameof(FilteredUsers));
            }
        }

        public List<RoleViewModel> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        public List<PermissionViewModel> Permissions
        {
            get => _permissions;
            set
            {
                _permissions = value;
                OnPropertyChanged(nameof(Permissions));
            }
        }

        public UserViewModel? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public RoleViewModel? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                OnPropertyChanged(nameof(RolePermissions));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(FilteredUsers));
            }
        }

        public bool IsActiveFilter
        {
            get => _isActiveFilter;
            set
            {
                _isActiveFilter = value;
                OnPropertyChanged(nameof(IsActiveFilter));
                OnPropertyChanged(nameof(FilteredUsers));
            }
        }

        public List<UserViewModel> FilteredUsers
        {
            get
            {
                var filtered = Users.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    filtered = filtered.Where(u =>
                        u.Fullname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        u.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        u.RoleName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                if (IsActiveFilter)
                {
                    filtered = filtered.Where(u => u.IsActive);
                }

                return filtered.ToList();
            }
        }

        public List<PermissionViewModel> RolePermissions
        {
            get
            {
                if (SelectedRole == null) return new List<PermissionViewModel>();
                return Permissions.Where(p => p.IsAssignedToRole).ToList();
            }
        }

        public int TotalUsers => Users.Count;
        public int ActiveUsers => Users.Count(u => u.IsActive);
        public int InactiveUsers => Users.Count(u => !u.IsActive);
        public int TotalRoles => Roles.Count;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UserViewModel : INotifyPropertyChanged
    {
        private long _id;
        private string _fullname = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _address = string.Empty;
        private DateTime? _dateOfBirth;
        private bool _isActive;
        private long _roleId;
        private string _roleName = string.Empty;
        private string _employeeName = string.Empty;
        private DateTime _createdAt;
        private DateTime? _updatedAt;

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Fullname
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(Fullname));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(DateOfBirthFormatted));
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public long RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged(nameof(RoleId));
            }
        }

        public string RoleName
        {
            get => _roleName;
            set
            {
                _roleName = value;
                OnPropertyChanged(nameof(RoleName));
            }
        }

        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
                OnPropertyChanged(nameof(CreatedAtFormatted));
            }
        }

        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged(nameof(UpdatedAt));
                OnPropertyChanged(nameof(UpdatedAtFormatted));
            }
        }

        public string StatusText => IsActive ? "Hoạt động" : "Không hoạt động";
        public string DateOfBirthFormatted => DateOfBirth?.ToString("dd/MM/yyyy") ?? "";
        public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
        public string UpdatedAtFormatted => UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RoleViewModel : INotifyPropertyChanged
    {
        private long _id;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int _userCount;
        private DateTime _createdAt;
        private DateTime? _updatedAt;

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int UserCount
        {
            get => _userCount;
            set
            {
                _userCount = value;
                OnPropertyChanged(nameof(UserCount));
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
                OnPropertyChanged(nameof(CreatedAtFormatted));
            }
        }

        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged(nameof(UpdatedAt));
                OnPropertyChanged(nameof(UpdatedAtFormatted));
            }
        }

        public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
        public string UpdatedAtFormatted => UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PermissionViewModel : INotifyPropertyChanged
    {
        private long _id;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _resource = string.Empty;
        private string _action = string.Empty;
        private bool _isAssignedToRole;

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Resource
        {
            get => _resource;
            set
            {
                _resource = value;
                OnPropertyChanged(nameof(Resource));
            }
        }

        public string Action
        {
            get => _action;
            set
            {
                _action = value;
                OnPropertyChanged(nameof(Action));
            }
        }

        public bool IsAssignedToRole
        {
            get => _isAssignedToRole;
            set
            {
                _isAssignedToRole = value;
                OnPropertyChanged(nameof(IsAssignedToRole));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}