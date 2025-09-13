using AutoMapper;
using Dashboard.BussinessLogic.Dtos.AuthDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.BussinessLogic.Services.RBACServices;

public interface IUserManagementService
{
    Task<UserDto?> CreateUserAsync(CreateUserInput input);
    Task<UserDto?> UpdateUserAsync(UpdateUserInput input);
    Task<bool> DeleteUserAsync(long userId);
    Task<UserDto?> GetUserByIdAsync(long userId);
    Task<List<UserDto>> GetUsersAsync(GetUsersInput input);
    Task<bool> ActivateUserAsync(long userId);
    Task<bool> DeactivateUserAsync(long userId);
    Task<bool> ResetPasswordAsync(long userId, string newPassword);
    Task<bool> AssignEmployeeToUserAsync(long userId, long employeeId);
    Task<List<UserDto>> GetUsersWithoutEmployeeAsync();
    Task<UserDto?> GetUserByEmployeeIdAsync(long employeeId);
}

public class UserManagementService : BaseTransactionalService, IUserManagementService
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<UserManagementService> logger) : base(unitOfWork)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserInput input)
    {
        try
        {
            // Check if user with phone number already exists
            var existingUserSpec = new Specification<User>(u => u.PhoneNumber == input.PhoneNumber);
            var existingUser = await _unitOfWork.Repository<User>().GetWithSpecAsync(existingUserSpec);

            if (existingUser != null)
            {
                _logger.LogWarning("User with phone number {PhoneNumber} already exists", input.PhoneNumber);
                return null;
            }

            // Validate role
            var roleSpec = new Specification<Role>(r => r.Id == input.RoleId);
            var role = await _unitOfWork.Repository<Role>().GetWithSpecAsync(roleSpec);

            if (role == null || !IsValidWinFormsRole(role.Name))
            {
                _logger.LogWarning("Invalid role for user creation: {RoleId}", input.RoleId);
                return null;
            }

            // Hash password
            var hashedPassword = HashPassword(input.Password);

            var user = new User
            {
                Fullname = input.FullName,
                PhoneNumber = input.PhoneNumber,
                Address = input.Address,
                Password = hashedPassword,
                RoleId = input.RoleId,
                EmployeeId = input.EmployeeId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now
            };

            var createdUser = await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            if (createdUser == null) return null;

            _logger.LogInformation("User created successfully: {UserId}", createdUser.Id);

            // Return user with role information
            return await GetUserByIdAsync(createdUser.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return null;
        }
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserInput input)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == input.Id);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", input.Id);
                return null;
            }

            // Check if phone number is being changed and if it already exists
            if (input.PhoneNumber != user.PhoneNumber)
            {
                var existingUserSpec = new Specification<User>(u => u.PhoneNumber == input.PhoneNumber && u.Id != input.Id);
                var existingUser = await _unitOfWork.Repository<User>().GetWithSpecAsync(existingUserSpec);

                if (existingUser != null)
                {
                    _logger.LogWarning("Phone number {PhoneNumber} is already in use", input.PhoneNumber);
                    return null;
                }
            }

            // Validate role if being changed
            if (input.RoleId.HasValue && input.RoleId != user.RoleId)
            {
                var roleSpec = new Specification<Role>(r => r.Id == input.RoleId);
                var role = await _unitOfWork.Repository<Role>().GetWithSpecAsync(roleSpec);

                if (role == null || !IsValidWinFormsRole(role.Name))
                {
                    _logger.LogWarning("Invalid role for user update: {RoleId}", input.RoleId);
                    return null;
                }
            }

            // Update user properties
            user.Fullname = input.FullName ?? user.Fullname;
            user.PhoneNumber = input.PhoneNumber ?? user.PhoneNumber;
            user.Address = input.Address;
            user.LastModified = DateTime.Now;

            if (input.RoleId.HasValue)
                user.RoleId = input.RoleId.Value;

            if (input.EmployeeId.HasValue)
                user.EmployeeId = input.EmployeeId.Value;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User updated successfully: {UserId}", user.Id);

            return await GetUserByIdAsync(user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", input.Id);
            return null;
        }
    }

    public async Task<bool> DeleteUserAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            // Soft delete by deactivating the user
            user.IsActive = false;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User deactivated (soft deleted): {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {UserId}", userId);
            return false;
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.Fullname ?? "",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                RoleName = user.Role.Name,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.Employee?.FullName,
                PositionId = user.Employee?.PositionId,
                BranchId = user.Employee?.BranchId,
                BranchName = user.Employee?.Branch?.Name,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user: {UserId}", userId);
            return null;
        }
    }

    public async Task<List<UserDto>> GetUsersAsync(GetUsersInput input)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(input.RoleName))
            {
                if (!IsValidWinFormsRole(input.RoleName))
                    return new List<UserDto>();

                userSpec = new Specification<User>(u => u.IsActive && u.Role.Name == input.RoleName);
            }

            if (!string.IsNullOrEmpty(input.SearchText))
            {
                var searchText = input.SearchText.ToLower();
                userSpec = new Specification<User>(u => u.IsActive && 
                    (u.Fullname != null && u.Fullname.ToLower().Contains(searchText) ||
                     u.PhoneNumber.Contains(searchText)));
            }

            if (input.BranchId.HasValue)
            {
                userSpec = new Specification<User>(u => u.IsActive && 
                    u.Employee != null && u.Employee.BranchId == input.BranchId);
            }

            // Include related entities
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(userSpec, true);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.Fullname ?? "",
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                IsActive = u.IsActive,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                EmployeeId = u.EmployeeId,
                EmployeeName = u.Employee?.FullName,
                PositionId = u.Employee?.PositionId,
                BranchId = u.Employee?.BranchId,
                BranchName = u.Employee?.Branch?.Name,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return new List<UserDto>();
        }
    }

    public async Task<bool> ActivateUserAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            user.IsActive = true;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User activated: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeactivateUserAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            user.IsActive = false;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User deactivated: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(long userId, string newPassword)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            var hashedPassword = HashPassword(newPassword);

            user.Password = hashedPassword;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Password reset for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AssignEmployeeToUserAsync(long userId, long employeeId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == userId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            // Check if employee exists
            var employeeSpec = new Specification<Employee>(e => e.Id == employeeId);
            var employee = await _unitOfWork.Repository<Employee>().GetWithSpecAsync(employeeSpec);

            if (employee == null)
            {
                _logger.LogWarning("Employee not found: {EmployeeId}", employeeId);
                return false;
            }

            // Check if employee is already assigned to another user
            var existingUserSpec = new Specification<User>(u => u.EmployeeId == employeeId && u.Id != userId);
            var existingUser = await _unitOfWork.Repository<User>().GetWithSpecAsync(existingUserSpec);

            if (existingUser != null)
            {
                _logger.LogWarning("Employee {EmployeeId} is already assigned to user {UserId}", employeeId, existingUser.Id);
                return false;
            }

            user.EmployeeId = employeeId;
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} assigned to user {UserId}", employeeId, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning employee {EmployeeId} to user {UserId}", employeeId, userId);
            return false;
        }
    }

    public async Task<List<UserDto>> GetUsersWithoutEmployeeAsync()
    {
        try
        {
            var userSpec = new Specification<User>(u => u.IsActive && u.EmployeeId == null);
            userSpec.IncludeStrings.Add("Role");

            var users = await _unitOfWork.Repository<User>().GetAllWithSpecAsync(userSpec, true);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.Fullname ?? "",
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                IsActive = u.IsActive,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users without employee");
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> GetUserByEmployeeIdAsync(long employeeId)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.EmployeeId == employeeId && u.IsActive);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.Fullname ?? "",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                RoleName = user.Role.Name,
                EmployeeId = user.EmployeeId,
                EmployeeName = user.Employee?.FullName,
                PositionId = user.Employee?.PositionId,
                BranchId = user.Employee?.BranchId,
                BranchName = user.Employee?.Branch?.Name,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by employee ID: {EmployeeId}", employeeId);
            return null;
        }
    }

    private static bool IsValidWinFormsRole(string roleName)
    {
        return roleName == Roles.ADMIN || roleName == Roles.MANAGER || roleName == Roles.EMPLOYEE;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "CoffeeShop2024")); // Add salt
        return Convert.ToBase64String(hashedBytes);
    }
}
