using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.PayrollDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Branches;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services.EmployeeServices;

public interface IPayrollService
{
    Task<PagedList<PayrollDto>> GetPayrollsAsync(GetPayrollsInput input);
    Task<PayrollDto?> GetPayrollAsync(long id);
    Task<PayrollDto> CreatePayrollAsync(CreatePayrollInput input);
    Task<PayrollDto> UpdatePayrollAsync(UpdatePayrollInput input);
    Task<bool> DeletePayrollAsync(long id);
    Task<PayrollSummaryDto> GetPayrollSummaryAsync(int month, int year, long? branchId = null);
    Task<List<PayrollDto>> GenerateMonthlyPayrollAsync(int month, int year, long? branchId = null);
    Task<SalaryCalculationDto> CalculateSalaryAsync(long employeeId, int month, int year);
    Task<MonthlyPayrollReportDto> GetMonthlyPayrollReportAsync(int month, int year, long? branchId = null);
    Task<List<PayrollDto>> GetEmployeePayrollHistoryAsync(long employeeId, int? year = null, int? fromMonth = null, int? toMonth = null);
    
    // Employee Salary Management
    Task<PagedList<EmployeeSalaryDto>> GetEmployeeSalariesAsync(DefaultInput input);
    Task<EmployeeSalaryDto?> GetCurrentSalaryAsync(long employeeId);
    Task<EmployeeSalaryDto> CreateEmployeeSalaryAsync(CreateEmployeeSalaryInput input);
    Task<EmployeeSalaryDto> UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryInput input);
    Task<List<EmployeeSalaryDto>> GetSalaryHistoryAsync(long employeeId);
}

public class PayrollService : BaseTransactionalService, IPayrollService
{
    private readonly IPayrollRepository _payrollRepository;
    private readonly IEmployeeSalaryRepository _salaryRepository;
    private readonly IEmployeeShiftRepository _shiftRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PayrollService> _logger;

    public PayrollService(
        IUnitOfWork unitOfWork,
        IPayrollRepository payrollRepository,
        IEmployeeSalaryRepository salaryRepository,
        IEmployeeShiftRepository shiftRepository,
        IRepository<Employee> employeeRepository,
        IMapper mapper,
        ILogger<PayrollService> logger)
        : base(unitOfWork)
    {
        _payrollRepository = payrollRepository;
        _salaryRepository = salaryRepository;
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<PayrollDto>> GetPayrollsAsync(GetPayrollInput input)
    {
        try
        {
            var payrolls = new List<EmployeePayroll>();

            if (input.Month.HasValue && input.Year.HasValue)
            {
                payrolls = await _payrollRepository.GetPayrollsByMonthAsync(
                    input.Month.Value, input.Year.Value, input.BranchId);
            }
            else if (input.EmployeeId.HasValue)
            {
                payrolls = await _payrollRepository.GetPayrollsByEmployeeAsync(
                    input.EmployeeId.Value, input.Year);
            }
            else
            {
                // Get all payrolls (consider adding date range filtering)
                var allPayrolls = await _payrollRepository.GetAllAsync();
                payrolls = allPayrolls.ToList();
            }

            var totalRecords = payrolls.Count;
            var pagedPayrolls = payrolls
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var payrollDtos = _mapper.Map<List<PayrollDto>>(pagedPayrolls);

            return new PagedList<PayrollDto>
            {
                Items = payrollDtos,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payrolls");
            throw;
        }
    }

    public async Task<PayrollDto?> GetPayrollByIdAsync(long id)
    {
        try
        {
            var payroll = await _payrollRepository.GetAsync(id);
            return payroll != null ? _mapper.Map<PayrollDto>(payroll) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payroll by id {Id}", id);
            throw;
        }
    }

    public async Task<PayrollDto> CreatePayrollAsync(CreatePayrollInput input)
    {
        try
        {
            // Check if payroll already exists
            var existingPayroll = await _payrollRepository.GetPayrollByEmployeeAndMonthAsync(
                input.EmployeeId, input.Month, input.Year);

            if (existingPayroll != null)
            {
                throw new InvalidOperationException("Bảng lương cho nhân viên trong tháng này đã tồn tại");
            }

            // Get employee and current salary
            var employee = await _employeeRepository.GetAsync(input.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại");
            }

            var currentSalary = await _salaryRepository.GetCurrentSalaryAsync(input.EmployeeId);
            if (currentSalary == null)
            {
                throw new InvalidOperationException("Nhân viên chưa có thông tin lương cơ bản");
            }

            // Calculate working hours
            var totalWorkingHours = await _shiftRepository.GetTotalWorkingHoursAsync(
                input.EmployeeId, input.Month, input.Year);

            // Calculate salary components
            var calculations = CalculateSalary(currentSalary, totalWorkingHours, input.Bonus, input.Penalty, input.AdditionalAllowance);

            var payroll = new EmployeePayroll
            {
                EmployeeId = input.EmployeeId,
                Month = input.Month,
                Year = input.Year,
                TotalWorkingHours = totalWorkingHours,
                BaseSalary = calculations.BaseSalary,
                Allowance = calculations.Allowance,
                Bonus = calculations.Bonus,
                Penalty = calculations.Penalty,
                GrossSalary = calculations.GrossSalary,
                TaxAmount = calculations.TaxAmount,
                NetSalary = calculations.NetSalary
            };

            await _payrollRepository.AddAsync(payroll);
            await _payrollRepository.SaveChangesAsync();

            _logger.LogInformation("Created payroll for employee {EmployeeId} for {Month}/{Year}", 
                input.EmployeeId, input.Month, input.Year);

            return _mapper.Map<PayrollDto>(payroll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payroll");
            throw;
        }
    }

    public async Task<PayrollSummaryDto> GetPayrollSummaryAsync(int month, int year, long? branchId = null)
    {
        try
        {
            var payrolls = await _payrollRepository.GetPayrollsByMonthAsync(month, year, branchId);
            var payrollDtos = _mapper.Map<List<PayrollDto>>(payrolls);

            return new PayrollSummaryDto
            {
                Month = month,
                Year = year,
                TotalEmployees = payrolls.Count,
                TotalGrossSalary = payrolls.Sum(p => p.GrossSalary ?? 0),
                TotalNetSalary = payrolls.Sum(p => p.NetSalary ?? 0),
                TotalTax = payrolls.Sum(p => p.TaxAmount ?? 0),
                AverageSalary = payrolls.Count > 0 ? payrolls.Average(p => p.NetSalary ?? 0) : 0,
                Payrolls = payrollDtos
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payroll summary");
            throw;
        }
    }

    public async Task<List<PayrollDto>> GenerateMonthlyPayrollAsync(int month, int year, long? branchId = null)
    {
        try
        {
            // Get all active employees in the branch
            var allEmployees = await _employeeRepository.GetAllAsync();
            var employees = allEmployees.Where(e => e.Status == "ACTIVE");

            if (branchId.HasValue)
            {
                employees = employees.Where(e => e.BranchId == branchId.Value);
            }

            var generatedPayrolls = new List<PayrollDto>();

            foreach (var employee in employees)
            {
                try
                {
                    // Check if payroll already exists
                    var existingPayroll = await _payrollRepository.GetPayrollByEmployeeAndMonthAsync(
                        employee.Id, month, year);

                    if (existingPayroll != null)
                    {
                        _logger.LogWarning("Payroll already exists for employee {EmployeeId} for {Month}/{Year}", 
                            employee.Id, month, year);
                        continue;
                    }

                    var input = new CreatePayrollInput
                    {
                        EmployeeId = employee.Id,
                        Month = month,
                        Year = year
                    };

                    var payroll = await CreatePayrollAsync(input);
                    generatedPayrolls.Add(payroll);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating payroll for employee {EmployeeId}", employee.Id);
                    // Continue with other employees
                }
            }

            _logger.LogInformation("Generated {Count} payrolls for {Month}/{Year}", 
                generatedPayrolls.Count, month, year);

            return generatedPayrolls;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly payroll");
            throw;
        }
    }

    public async Task<bool> DeletePayrollAsync(long id)
    {
        try
        {
            var payroll = await _payrollRepository.GetAsync(id);
            if (payroll == null)
            {
                return false;
            }

            _payrollRepository.Remove(payroll);
            await _payrollRepository.SaveChangesAsync();

            _logger.LogInformation("Deleted payroll {PayrollId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payroll {PayrollId}", id);
            throw;
        }
    }

    #region Employee Salary Management

    public async Task<PagedList<EmployeeSalaryDto>> GetEmployeeSalariesAsync(DefaultInput input)
    {
        try
        {
            var salaries = await _salaryRepository.GetAllAsync();
            var totalRecords = salaries.Count();

            var pagedSalaries = salaries
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var salaryDtos = _mapper.Map<List<EmployeeSalaryDto>>(pagedSalaries);

            return new PagedList<EmployeeSalaryDto>
            {
                Items = salaryDtos,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee salaries");
            throw;
        }
    }

    public async Task<EmployeeSalaryDto?> GetCurrentSalaryAsync(long employeeId)
    {
        try
        {
            var salary = await _salaryRepository.GetCurrentSalaryAsync(employeeId);
            return salary != null ? _mapper.Map<EmployeeSalaryDto>(salary) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current salary for employee {EmployeeId}", employeeId);
            throw;
        }
    }

    public async Task<EmployeeSalaryDto> CreateEmployeeSalaryAsync(CreateEmployeeSalaryInput input)
    {
        try
        {
            var employee = await _employeeRepository.GetAsync(input.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại");
            }

            var salary = _mapper.Map<EmployeeSalary>(input);
            await _salaryRepository.AddAsync(salary);
            await _payrollRepository.SaveChangesAsync();

            _logger.LogInformation("Created salary for employee {EmployeeId}", input.EmployeeId);

            return _mapper.Map<EmployeeSalaryDto>(salary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee salary");
            throw;
        }
    }

    public async Task<EmployeeSalaryDto> UpdateEmployeeSalaryAsync(UpdateEmployeeSalaryInput input)
    {
        try
        {
            var salary = await _salaryRepository.GetAsync(input.Id);
            if (salary == null)
            {
                throw new ArgumentException("Thông tin lương không tồn tại");
            }

            _mapper.Map(input, salary);
            await _payrollRepository.SaveChangesAsync();

            _logger.LogInformation("Updated salary {SalaryId}", input.Id);

            return _mapper.Map<EmployeeSalaryDto>(salary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee salary {SalaryId}", input.Id);
            throw;
        }
    }

    public async Task<List<EmployeeSalaryDto>> GetSalaryHistoryAsync(long employeeId)
    {
        try
        {
            var salaryHistory = await _salaryRepository.GetSalaryHistoryAsync(employeeId);
            return _mapper.Map<List<EmployeeSalaryDto>>(salaryHistory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting salary history for employee {EmployeeId}", employeeId);
            throw;
        }
    }

    #endregion

    #region New Methods for Complete API

    public async Task<PagedList<PayrollDto>> GetPayrollsAsync(GetPayrollsInput input)
    {
        try
        {
            var payrolls = new List<EmployeePayroll>();

            if (input.Month.HasValue && input.Year.HasValue)
            {
                payrolls = await _payrollRepository.GetPayrollsByMonthAsync(
                    input.Month.Value, input.Year.Value, input.BranchId);
            }
            else if (input.EmployeeId.HasValue)
            {
                payrolls = await _payrollRepository.GetPayrollsByEmployeeAsync(
                    input.EmployeeId.Value, input.Year);
            }
            else
            {
                var allPayrolls = await _payrollRepository.GetAllAsync();
                payrolls = allPayrolls.ToList();
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                payrolls = payrolls.Where(p => 
                    (p.Employee?.FullName?.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Employee?.Email?.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }

            var totalRecords = payrolls.Count;
            var pagedPayrolls = payrolls
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var payrollDtos = _mapper.Map<List<PayrollDto>>(pagedPayrolls);

            return new PagedList<PayrollDto>
            {
                Items = payrollDtos,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payrolls with new input");
            throw;
        }
    }

    public async Task<PayrollDto?> GetPayrollAsync(long id)
    {
        try
        {
            var payroll = await _payrollRepository.GetAsync(id);
            return payroll != null ? _mapper.Map<PayrollDto>(payroll) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payroll by id {Id}", id);
            throw;
        }
    }

    public async Task<PayrollDto> UpdatePayrollAsync(UpdatePayrollInput input)
    {
        try
        {
            var payroll = await _payrollRepository.GetAsync(input.Id);
            if (payroll == null)
            {
                throw new ArgumentException("Bảng lương không tồn tại");
            }

            // Update bonus and penalty if provided
            if (input.Bonus.HasValue) payroll.Bonus = input.Bonus.Value;
            if (input.Penalty.HasValue) payroll.Penalty = input.Penalty.Value;
            if (input.AdditionalAllowance.HasValue) 
                payroll.Allowance = (payroll.Allowance ?? 0) + input.AdditionalAllowance.Value;

            // Recalculate salary if values changed
            var currentSalary = await _salaryRepository.GetCurrentSalaryAsync(payroll.EmployeeId);
            if (currentSalary != null)
            {
                var calculations = CalculateSalary(
                    currentSalary, 
                    payroll.TotalWorkingHours ?? 0, 
                    payroll.Bonus, 
                    payroll.Penalty, 
                    0);

                payroll.GrossSalary = calculations.GrossSalary;
                payroll.TaxAmount = calculations.TaxAmount;
                payroll.NetSalary = calculations.NetSalary;
            }

            await _payrollRepository.SaveChangesAsync();

            _logger.LogInformation("Updated payroll {PayrollId}", input.Id);
            return _mapper.Map<PayrollDto>(payroll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payroll {PayrollId}", input.Id);
            throw;
        }
    }

    public async Task<SalaryCalculationDto> CalculateSalaryAsync(long employeeId, int month, int year)
    {
        try
        {
            var employee = await _employeeRepository.GetAsync(employeeId);
            if (employee == null)
            {
                throw new ArgumentException("Nhân viên không tồn tại");
            }

            var currentSalary = await _salaryRepository.GetCurrentSalaryAsync(employeeId);
            if (currentSalary == null)
            {
                throw new InvalidOperationException("Nhân viên chưa có thông tin lương cơ bản");
            }

            var totalWorkingHours = await _shiftRepository.GetTotalWorkingHoursAsync(employeeId, month, year);
            var calculations = CalculateSalary(currentSalary, totalWorkingHours);

            var calculationNotes = new List<string>();
            if (currentSalary.SalaryType == "HOURLY")
            {
                calculationNotes.Add($"Lương theo giờ: {currentSalary.BaseSalary:C} x {totalWorkingHours} giờ");
            }
            else
            {
                calculationNotes.Add($"Lương cố định tháng: {currentSalary.BaseSalary:C}");
            }

            if (calculations.Allowance > 0)
                calculationNotes.Add($"Phụ cấp: {calculations.Allowance:C}");
            if (calculations.Bonus > 0)
                calculationNotes.Add($"Thưởng: {calculations.Bonus:C}");
            if (calculations.Penalty > 0)
                calculationNotes.Add($"Phạt: -{calculations.Penalty:C}");
            if (calculations.TaxAmount > 0)
                calculationNotes.Add($"Thuế: -{calculations.TaxAmount:C} ({currentSalary.TaxRate ?? 0}%)");

            return new SalaryCalculationDto
            {
                EmployeeId = employeeId,
                EmployeeName = employee.FullName,
                Month = month,
                Year = year,
                BaseSalary = calculations.BaseSalary,
                TotalWorkingHours = totalWorkingHours,
                Allowance = calculations.Allowance,
                Bonus = calculations.Bonus,
                Penalty = calculations.Penalty,
                GrossSalary = calculations.GrossSalary,
                TaxRate = currentSalary.TaxRate ?? 0,
                TaxAmount = calculations.TaxAmount,
                NetSalary = calculations.NetSalary,
                CalculationNotes = calculationNotes
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating salary for employee {EmployeeId}", employeeId);
            throw;
        }
    }

    public async Task<MonthlyPayrollReportDto> GetMonthlyPayrollReportAsync(int month, int year, long? branchId = null)
    {
        try
        {
            var payrolls = await _payrollRepository.GetPayrollsByMonthAsync(month, year, branchId);
            var payrollDtos = _mapper.Map<List<PayrollDto>>(payrolls);

            string? branchName = null;
            if (branchId.HasValue)
            {
                var branchRepository = _unitOfWork.Repository<Branch>();
                var branch = await branchRepository.GetAsync(branchId.Value);
                branchName = branch?.Name;
            }

            return new MonthlyPayrollReportDto
            {
                Month = month,
                Year = year,
                BranchName = branchName,
                TotalEmployees = payrolls.Count,
                TotalBaseSalary = payrolls.Sum(p => p.BaseSalary ?? 0),
                TotalAllowance = payrolls.Sum(p => p.Allowance ?? 0),
                TotalBonus = payrolls.Sum(p => p.Bonus ?? 0),
                TotalPenalty = payrolls.Sum(p => p.Penalty ?? 0),
                TotalGrossSalary = payrolls.Sum(p => p.GrossSalary ?? 0),
                TotalTax = payrolls.Sum(p => p.TaxAmount ?? 0),
                TotalNetSalary = payrolls.Sum(p => p.NetSalary ?? 0),
                AverageNetSalary = payrolls.Count > 0 ? payrolls.Average(p => p.NetSalary ?? 0) : 0,
                PayrollDetails = payrollDtos,
                GeneratedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monthly payroll report");
            throw;
        }
    }

    public async Task<List<PayrollDto>> GetEmployeePayrollHistoryAsync(
        long employeeId, int? year = null, int? fromMonth = null, int? toMonth = null)
    {
        try
        {
            var payrolls = await _payrollRepository.GetPayrollsByEmployeeAsync(employeeId, year);
            
            if (fromMonth.HasValue && toMonth.HasValue)
            {
                payrolls = payrolls.Where(p => p.Month >= fromMonth.Value && p.Month <= toMonth.Value).ToList();
            }

            return _mapper.Map<List<PayrollDto>>(payrolls.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payroll history for employee {EmployeeId}", employeeId);
            throw;
        }
    }

    #endregion

    #region Private Methods

    private static SalaryCalculation CalculateSalary(
        EmployeeSalary currentSalary, 
        decimal totalWorkingHours, 
        decimal? bonus = null, 
        decimal? penalty = null, 
        decimal? additionalAllowance = null)
    {
        var calculation = new SalaryCalculation();

        // Base salary calculation
        if (currentSalary.SalaryType == "HOURLY")
        {
            calculation.BaseSalary = currentSalary.BaseSalary * totalWorkingHours;
        }
        else
        {
            calculation.BaseSalary = currentSalary.BaseSalary;
        }

        // Add allowances
        calculation.Allowance = (currentSalary.Allowance ?? 0) + (additionalAllowance ?? 0);

        // Add bonus and subtract penalty
        calculation.Bonus = (currentSalary.Bonus ?? 0) + (bonus ?? 0);
        calculation.Penalty = (currentSalary.Penalty ?? 0) + (penalty ?? 0);

        // Calculate gross salary
        calculation.GrossSalary = calculation.BaseSalary + calculation.Allowance + calculation.Bonus - calculation.Penalty;

        // Calculate tax
        var taxRate = (currentSalary.TaxRate ?? 0) / 100;
        calculation.TaxAmount = calculation.GrossSalary * taxRate;

        // Calculate net salary
        calculation.NetSalary = calculation.GrossSalary - calculation.TaxAmount;

        return calculation;
    }

    #endregion
}

public class SalaryCalculation
{
    public decimal BaseSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Bonus { get; set; }
    public decimal Penalty { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetSalary { get; set; }
}
