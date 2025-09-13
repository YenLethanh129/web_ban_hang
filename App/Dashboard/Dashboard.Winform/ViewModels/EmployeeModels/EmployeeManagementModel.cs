using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Winform.ViewModels.EmployeeModels
{
    public class EmployeeManagementModel : IManagableModel
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems = 0;

        private BindingList<EmployeeViewModel> _employees = [];
        private EmployeeViewModel? _selectedEmployee;
        private string _searchText = string.Empty;
        private BindingList<string> _statuses = ["All", "Active", "Inactive"];
        private BindingList<PositionViewModel> _positions = new();
        public BindingList<PositionViewModel> Positions
        {
            get => _positions;
            set
            {
                if (_positions != value)
                {
                    _positions = value;
                    OnPropertyChanged(nameof(Positions));
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
        public BindingList<EmployeeViewModel> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
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

        public EmployeeViewModel? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    OnPropertyChanged(nameof(SelectedEmployee));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void MapPositionsToEmployees()
        {
            if (Positions == null || Positions.Count == 0 || Employees == null)
                return;

            var positionDict = Positions.ToDictionary(p => p.Id, p => p.Name);

            foreach (var emp in Employees)
            {
                if (positionDict.TryGetValue(emp.PositionId, out var posName))
                {
                    emp.PositionName = posName;
                }
                else
                {
                    emp.PositionName = "Không rõ";
                }
            }
        }

    }

    public class EmployeeViewModel
    {
        public long Id { get; set; }
        public long BranchId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public long PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class PositionViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? BaseSalary { get; set; }

        public string DisplayText
        {
            get
            {
                if (BaseSalary.HasValue)
                    return $"{Name} (Lương cơ bản: {BaseSalary:N0})";
                return Name;
            }
        }
    }
}
