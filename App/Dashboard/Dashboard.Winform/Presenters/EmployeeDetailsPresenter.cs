using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Dtos.PayrollDtos;
using Dashboard.BussinessLogic.Services.BranchServices;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace Dashboard.Winform.Presenters
{
    public interface IEmployeeDetailsPresenter
    {
        EmployeeDetailViewModel Model { get; }
        event EventHandler? OnDataLoaded;
        event EventHandler<string>? OnError;
        event EventHandler? OnEmployeeSaved;
        Task LoadEmployeeDetailsAsync(long employeeId);
        Task LoadLookupsAsync();
        Task SaveEmployeeAsync(EmployeeDetailViewModel employee);
        Task DeleteEmployeeAsync(long employeeId);
        void RaiseDataLoaded();
    }

    public class EmployeeDetailsPresenter : IEmployeeDetailsPresenter
    {
        private readonly IEmployeeManagementService _employeeService;
        private readonly IPayrollService _payrollService;
        private readonly IEmployeeShiftService _shiftService;
        private readonly IBranchService _branchService;
        private readonly IRoleManagementService _roleService;
        private readonly IUserManagementService _userService;
        private readonly IMapper _mapper;

        private static List<BranchDto>? _cachedBranches;
        private static List<PositionDto>? _cachedPositions;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly SemaphoreSlim _instanceSemaphore = new(1, 1);

        public EmployeeDetailViewModel Model { get; }

        public event EventHandler? OnDataLoaded;
        public event EventHandler<string>? OnError;
        public event EventHandler? OnEmployeeSaved;

        public EmployeeDetailsPresenter(
            IEmployeeManagementService employeeService,
            IPayrollService payrollService,
            IEmployeeShiftService shiftService,
            IBranchService branchService,
            IRoleManagementService roleService,
            IUserManagementService userService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _payrollService = payrollService;
            _shiftService = shiftService;
            _branchService = branchService;
            _roleService = roleService;
            _userService = userService;
            _mapper = mapper;
            Model = new EmployeeDetailViewModel();
        }

        public void RaiseDataLoaded()
        {
            OnDataLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadLookupsAsync()
        {
            await _instanceSemaphore.WaitAsync();
            try
            {
                await LoadCachedDataAsync();
                RaiseDataLoaded();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error loading lookups: {ex.Message}");
            }
            finally
            {
                _instanceSemaphore.Release();
            }
        }

        private async Task LoadCachedDataAsync()
        {
            if (_cachedBranches == null)
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (_cachedBranches == null)
                    {
                        var input = new GetBranchesInput
                        {
                            PageNumber = 1,
                            PageSize = int.MaxValue
                        };
                        var allBranchesPaged = await _branchService.GetBranchesAsync(input);
                        _cachedBranches = allBranchesPaged.Items.ToList();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            if (_cachedPositions == null)
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (_cachedPositions == null)
                    {
                        var positions = await _employeeService.GetAllPositionsAsync();
                        _cachedPositions = [.. positions];
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            Model.ExistingBranches = _mapper.Map<List<BranchViewModel>>(_cachedBranches!);
            Model.ExistingPositions = _mapper.Map<List<PositionViewModel>>(_cachedPositions);

            if (Model.Statuses == null || Model.Statuses.Count == 0)
            {
                Model.Statuses = ["Active", "Inactive", "Resigned"];
            }
        }

        public async Task LoadEmployeeDetailsAsync(long employeeId)
        {
            await _instanceSemaphore.WaitAsync();
            try
            {
                var employeeDto = await _employeeService.GetEmployeeByIdAsync(employeeId);
                if (employeeDto == null)
                {
                    OnError?.Invoke(this, $"Employee with ID {employeeId} not found.");
                    return;
                }

                // I dunno know why automapper just not working here
                Model.Id = employeeDto.Id;
                Model.FullName = employeeDto.FullName;
                Model.Phone = employeeDto.Phone;
                Model.Email = employeeDto.Email;
                Model.HireDate = employeeDto.HireDate;
                Model.ResignDate = employeeDto.ResignDate;
                Model.Status = employeeDto.Status!;
                Model.PositionId = employeeDto.PositionId;
                Model.PositionName = employeeDto.PositionName;
                Model.CreatedAt = employeeDto.CreatedAt;
                Model.UpdatedAt = employeeDto.LastModified;
                Model.BranchId = employeeDto.BranchId;

                var branchDto = _cachedBranches?.FirstOrDefault(b => b.Id == Model.BranchId);
                var positionDto = _cachedPositions?.FirstOrDefault(p => p.Id == Model.PositionId);

                if (branchDto != null)
                {
                    Model.BranchName = branchDto.Name;
                    Model.BranchAddress = branchDto.Address;
                    Model.BranchPhone = branchDto.Phone;
                    Model.BranchManager = branchDto.Manager;
                }

                var salariesInput = new DefaultInput { PageNumber = 1, PageSize = 50 };
                var salariesPaged = await _payrollService.GetEmployeeSalariesAsync(salariesInput);
                var salaries = salariesPaged.Items.Where(s => s.EmployeeId == employeeId).ToList();

                var salaryViewModels = _mapper.Map<List<EmployeeSalaryViewModel>>(salaries);
                Model.Salaries = new BindingList<EmployeeSalaryViewModel>(salaryViewModels);

                var payrolls = await _payrollService.GetEmployeePayrollHistoryAsync(employeeId);
                Model.Payrolls = new BindingList<PayrollViewModel>(_mapper.Map<List<PayrollViewModel>>(payrolls));

                var shiftsInput = new GetEmployeeShiftsInput
                {
                    EmployeeId = employeeId,
                    PageNumber = 1,
                    PageSize = 50
                };
                var shiftsPaged = await _shiftService.GetShiftsAsync(shiftsInput);
                Model.Shifts = new BindingList<EmployeeShiftViewModel>(_mapper.Map<List<EmployeeShiftViewModel>>(shiftsPaged.Items));

                var userAccount = await _userService.GetUserByEmployeeIdAsync(employeeId);
                if (userAccount != null)
                {
                    Model.HasAccount = true;
                    Model.Username = userAccount.Username ?? string.Empty;
                    Model.Role = userAccount.RoleName ?? string.Empty;
                }
                else
                {
                    Model.HasAccount = false;
                }

                RaiseDataLoaded();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error loading employee details: {ex.Message}");
            }
            finally
            {
                _instanceSemaphore.Release();
            }
        }

        public async Task SaveEmployeeAsync(EmployeeDetailViewModel employee)
        {
            await _instanceSemaphore.WaitAsync();
            try
            {
                if (employee.Id == 0)
                {
                    var createInput = _mapper.Map<CreateEmployeeInput>(employee);
                    await _employeeService.AddEmployeeAsync(createInput);
                }
                else
                {
                    var updateInput = _mapper.Map<UpdateEmployeeInput>(employee);
                    await _employeeService.UpdateEmployeeAsync(updateInput);
                }

                OnEmployeeSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error saving employee: {ex.Message}");
                throw; 
            }
            finally
            {
                _instanceSemaphore.Release();
            }
        }

        public async Task DeleteEmployeeAsync(long employeeId)
        {
            await _instanceSemaphore.WaitAsync();
            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeId);

                OnEmployeeSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Error deleting employee: {ex.Message}");
                throw;
            }
            finally
            {
                _instanceSemaphore.Release();
            }
        }
    }
}