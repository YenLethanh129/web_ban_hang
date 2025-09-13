using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Services.EmployeeServices;

public interface IEmployeeManagementService
{
    Task<PagedList<EmployeeDto>> GetEmployeesAsync(GetEmployeesInput? input = null);
    Task<EmployeeDto?> GetEmployeeByIdAsync(long id);
    Task AddEmployeeAsync(CreateEmployeeInput employee);
    Task UpdateEmployeeAsync(UpdateEmployeeInput employee);
    Task DeleteEmployeeAsync(long id);
    Task<List<PositionDto>> GetAllPositionsAsync();

}
public class EmployeeManagementService : BaseTransactionalService, IEmployeeManagementService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
    public EmployeeManagementService(
        IUnitOfWork unitOfWork,
        IEmployeeRepository employeeRepository,
        IBranchRepository branchRepository,
        IMapper mapper,
        IEmployeeSalaryRepository employeeSalaryRepository
        ) : base(unitOfWork)
    {
        _branchRepository = branchRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _employeeSalaryRepository = employeeSalaryRepository;
    }

    public async Task AddEmployeeAsync(CreateEmployeeInput employee)
    {
        _ = await _branchRepository.GetAsync(employee.BranchId) 
            ?? throw new ArgumentException($"Branch with ID {employee.BranchId} does not exist.");
        

        var newEmployee = new Employee
        {
            BranchId = employee.BranchId,
            FullName = employee.FullName,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            PositionId = employee.PositionId,
            HireDate = employee.HireDate,
            CreatedAt = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var createdEmployee = await _employeeRepository.AddAsync(newEmployee);
        await _unitOfWork.SaveChangesAsync();
        if (createdEmployee == null)
        {
            throw new Exception("Failed to create employee.");
        }
    }

    public async Task DeleteEmployeeAsync(long id)
    {
        var employee = await _employeeRepository.GetAsync(id);
        if (employee == null)
        {
            throw new ArgumentException($"Employee with ID {id} does not exist.");
        }
        _employeeRepository.Remove(employee);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<EmployeeDto>> GetEmployeesAsync(GetEmployeesInput? input)
    {
        input ??= new GetEmployeesInput();
        var spec = EmployeeSpecifications.BySearchCriteria(input);
        spec.IncludeStrings.Add("Position");

        var employees = await _employeeRepository.GetAllWithSpecAsync(spec);

        var totalRecords = employees.Count();

        var items = employees
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .Select(e => new EmployeeDto
            {
                Status = e.Status!,
                Id = e.Id,
                BranchId = e.BranchId,
                FullName = e.FullName,
                Phone = e.PhoneNumber!,
                Email = e.Email!,
                PositionId = e.PositionId,
                PositionName = e.Position.Name ?? string.Empty,
                HireDate = e.HireDate ?? default,
                ResignDate = e.ResignDate,
                CreatedAt = e.CreatedAt,
                LastModified = e.LastModified
            })
            .ToList();

        return new PagedList<EmployeeDto>
        {
            Items = items,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize,
            TotalRecords = totalRecords
        };
    }


    public async Task<EmployeeDto?> GetEmployeeByIdAsync(long id)
    {
        var employee = await _employeeRepository.GetAsync(id);
        if (employee == null) return null;
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeInput employee)
    {
        if (employee == null || employee.EmployeeId <= 0)
        {
            throw new ArgumentException("Invalid employee data.");
        }

        var existingEmployee = await _employeeRepository.GetAsync(employee.EmployeeId) ?? throw new ArgumentException($"Employee with ID {employee.EmployeeId} does not exist.");
        
        if (employee.BranchId.HasValue)
        {
            _ = await _branchRepository.GetAsync(employee.BranchId.Value)
                ?? throw new ArgumentException($"Branch with ID {employee.BranchId.Value} does not exist.");
            existingEmployee.BranchId = employee.BranchId.Value;
        }

        if (existingEmployee == null)
        {
            throw new ArgumentException($"Employee with ID {employee.EmployeeId} does not exist.");
        }

        await ExecuteInTransactionAsync(async () =>
        {
            if (employee.BaseSalary.HasValue && !string.IsNullOrEmpty(employee.SalaryType))
            {
                var currentSalary = await _employeeSalaryRepository.GetCurrentSalaryAsync(existingEmployee.Id);
                var newSalary = new EmployeeSalary
                {
                    EmployeeId = existingEmployee.Id,
                    BaseSalary = employee.BaseSalary.Value,
                    SalaryType = employee.SalaryType,
                    EffectiveDate = DateTime.UtcNow,
                    CreatedAt = currentSalary != null ? currentSalary.CreatedAt : DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };
                await _employeeSalaryRepository.UpdateSalaryAsync(newSalary);
            }

            
            existingEmployee.FullName = employee.FullName ?? existingEmployee.FullName;
            existingEmployee.PhoneNumber = employee.Phone ?? existingEmployee.PhoneNumber;
            existingEmployee.Email = employee.Email ?? existingEmployee.Email;
            existingEmployee.PositionId = employee.PositionId ?? existingEmployee.PositionId;
            existingEmployee.HireDate = employee.HireDate ?? existingEmployee.HireDate;
            existingEmployee.ResignDate = employee.ResignDate ?? existingEmployee.ResignDate;
            existingEmployee.LastModified = DateTime.UtcNow;
            existingEmployee.Status = employee.Status ?? existingEmployee.Status;

            _employeeRepository.Update(existingEmployee);
        });


        await _unitOfWork.SaveChangesAsync();

    }
    public async Task<List<PositionDto>> GetAllPositionsAsync()
    {
        var positions = await _unitOfWork.Repository<EmployeePosition>().GetAllAsync();
        return _mapper.Map<List<PositionDto>>(positions);
    }
}
