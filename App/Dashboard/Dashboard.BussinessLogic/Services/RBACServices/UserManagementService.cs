using AutoMapper;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using System.Linq.Expressions;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Helpers;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services.RBACServices;

public interface IUserManagementService
{
    Task<bool> UpdateUserRole(UpdateUserInput input);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<UserDto?> CreateUserAsync(CreateUserInput input);
    Task<UserDto?> UpdateUserAsync(UpdateUserInput input);
    Task<UserDto?> ChangePasswordAsync(ChangePasswordInput input);
    Task<bool> DeleteUserAsync(long userId);
    Task<UserDto?> GetUserByIdAsync(long userId);
    Task<List<UserDto>> GetUsersAsync(GetUsersInput input);
    Task<bool> ActivateUserAsync(long userId);
    Task<bool> DeactivateUserAsync(long userId);
    Task<bool> ResetPasswordAsync(long userId, string newPassword);
    Task<bool> AssignEmployeeToUserAsync(long userId, long employeeId);
    Task<List<UserDto>> GetUsersWithoutEmployeeAsync();
    Task<UserDto?> GetUserByEmployeeIdAsync(long employeeId);
    Task<List<UserDto>> GetUsersByRoleAsync(string roleName);

}

public class UserManagementService : BaseTransactionalService, IUserManagementService
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserManagementService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly DataEncryptionHelper _dataEncryptionHelper;
    public UserManagementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserManagementService> logger,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        DataEncryptionHelper dataEncryptionHelper,
        IEmployeeRepository employeeRepository) : base(unitOfWork)
    {
        _mapper = mapper;
        _dataEncryptionHelper = dataEncryptionHelper;
        _logger = logger;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var userSpec = new Specification<EmployeeUserAccount>(u => u.Username == username && u.IsActive);
        var user = await _userRepository.GetWithSpecAsync(userSpec);
        if (user == null)
            return false;
        return _dataEncryptionHelper.VerifyPassword(password, user.Password);
    }

    public async Task<bool> UpdateUserRole(UpdateUserInput input)
    {
        var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == input.Id);
        var user = await _userRepository.GetWithSpecAsync(userSpec);
        if (user == null)
            return false;

        user.RoleId = input.RoleId ?? user.RoleId;

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
    {
        try
        {
            if (!IsValidWinFormsRole(roleName))
                return new List<UserDto>();
            var userSpec = new Specification<EmployeeUserAccount>(u => u.IsActive && u.Role.Name == roleName);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");
            var users = await _userRepository.GetAllWithSpecAsync(userSpec, true);
            return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users by role: {RoleName}", roleName);
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> ChangePasswordAsync(ChangePasswordInput input)
    {
        try
        {
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == input.UserId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", input.UserId);
                return null;
            }
            if (!_dataEncryptionHelper.VerifyPassword(input.CurrentPassword, user.Password))
            {
                _logger.LogWarning("Old password does not match for user: {UserId}", input.UserId);
                return null;
            }
            var hashedPassword = _dataEncryptionHelper.HashPassword(input.NewPassword);
            user.Password = hashedPassword;
            user.LastModified = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Password changed successfully for user: {UserId}", input.UserId);
            return await GetUserByIdAsync(user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", input.UserId);
            return null;
        }
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserInput input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            try
            {
                var existingUserSpec = new Specification<EmployeeUserAccount>(u => u.Username == input.Username);
                var existingUser = await _userRepository.GetWithSpecAsync(existingUserSpec);

                if (existingUser != null)
                {
                    throw new ArgumentException("User with username {Username} already exists", input.Username);
                }

                var existinEmployee = await _unitOfWork.Repository<Employee>().GetAsync(input.EmployeeId)
                    ?? throw new ArgumentException($"Employee with ID {input.EmployeeId} does not exist");
                var roleSpec = new Specification<Role>(r => r.Id == input.RoleId);
                var role = await _roleRepository.GetWithSpecAsync(roleSpec);

                if (role == null || !IsValidWinFormsRole(role.Name))
                {
                    _logger.LogWarning("Invalid role for user creation: {RoleId}", input.RoleId);
                    return (UserDto?)null;
                }

                var hashedPassword = _dataEncryptionHelper.HashPassword(input.Password);

                var user = _mapper.Map<EmployeeUserAccount>(input);
                user.Password = hashedPassword;

                var createdUser = await _userRepository.AddAsync(user);

                await _unitOfWork.SaveChangesAsync();
                if (createdUser == null)
                {
                    throw new Exception("Failed to create user");
                }
                return await GetUserByIdAsync(createdUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return null;
            }
        });
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserInput input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            try
            {
                var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == input.Id);
                var user = await _userRepository.GetWithSpecAsync(userSpec);

                if (user == null)
                {
                    _logger.LogWarning("User not found: {UserId}", input.Id);
                    return null;
                }

                if (input.Username != user.Username)
                {
                    var existingUserSpec = new Specification<EmployeeUserAccount>(u => u.Username == input.Username && u.Id != input.Id);
                    var existingUser = await _userRepository.GetWithSpecAsync(existingUserSpec);

                    if (existingUser != null)
                    {
                        _logger.LogWarning("Username {Username} is already in use", input.Username);
                        return null;
                    }
                }

                if (input.RoleId.HasValue && input.RoleId != user.RoleId)
                {
                    var roleSpec = new Specification<Role>(r => r.Id == input.RoleId);
                    var role = await _roleRepository.GetWithSpecAsync(roleSpec);

                    if (role == null || !IsValidWinFormsRole(role.Name))
                    {
                        _logger.LogWarning("Invalid role for user update: {RoleId}", input.RoleId);
                        return null;
                    }
                    user.RoleId = input.RoleId.Value;

                }

                if (input.EmployeeId != 0 && input.EmployeeId != user.EmployeeId)
                {
                    var employee = await _employeeRepository.GetAsync(input.EmployeeId);
                    if (employee == null)
                    {
                        _logger.LogWarning("Employee not found for user update: {EmployeeId}", input.EmployeeId);
                        return null;
                    }
                    user.EmployeeId = input.EmployeeId;
                }

                if (!string.IsNullOrEmpty(input.Password))
                {
                    var hashedPassword = _dataEncryptionHelper.HashPassword(input.Password);
                    user.Password = hashedPassword;
                }

                if (input.IsActive.HasValue)
                    user.IsActive = input.IsActive.Value;  

                user.Username = input.Username ?? user.Username;
                user.LastModified = DateTime.Now;

                if (input.RoleId.HasValue)
                    user.RoleId = input.RoleId.Value;

                _userRepository.Update(user);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User updated successfully: {UserId}", user.Id);

                return await GetUserByIdAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", input.Id);
                return null;
            }
        });
        
    }

    public async Task<bool> DeleteUserAsync(long userId)
    {
        try
        {
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var user = await _userRepository.GetWithSpecAsync(userSpec);

            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
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
            // Start with base predicate (active users)
            Expression<Func<EmployeeUserAccount, bool>> predicate = u => u.IsActive;

            // Apply Role filter
            if (!string.IsNullOrEmpty(input.RoleName))
            {
                if (!IsValidWinFormsRole(input.RoleName))
                    return new List<UserDto>();

                Expression<Func<EmployeeUserAccount, bool>> rolePredicate = u => u.IsActive && u.Role.Name == input.RoleName;
                predicate = Dashboard.BussinessLogic.Specifications.SpecificationHelper.CombinePredicates(predicate, rolePredicate);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(input.SearchText))
            {
                var searchText = input.SearchText.ToLower();
                Expression<Func<EmployeeUserAccount, bool>> searchPredicate = u => u.IsActive && (u.Username != null && u.Username.ToLower().Contains(searchText));
                predicate = Dashboard.BussinessLogic.Specifications.SpecificationHelper.CombinePredicates(predicate, searchPredicate);
            }

            var userSpec = new Specification<EmployeeUserAccount>(predicate);

            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var users = await _userRepository.GetAllWithSpecAsync(userSpec, true);

            return _mapper.Map<List<UserDto>>(users);
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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);

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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);

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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            var hashedPassword = _dataEncryptionHelper.HashPassword(newPassword);

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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId);
            var user = await _userRepository.GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return false;
            }

            var employeeSpec = new Specification<Employee>(e => e.Id == employeeId);
            var employee = await _employeeRepository.GetWithSpecAsync(employeeSpec);

            if (employee == null)
            {
                _logger.LogWarning("Employee not found: {EmployeeId}", employeeId);
                return false;
            }

            user.EmployeeId = employeeId;
            user.LastModified = DateTime.Now;
            _employeeRepository.Update(employee);

            await _employeeRepository.SaveChangesAsync();

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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.IsActive && u.EmployeeId == 0);
            userSpec.IncludeStrings.Add("Role");

            var users = await _userRepository.GetAllWithSpecAsync(userSpec, true);

            return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
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
            var userSpec = new Specification<EmployeeUserAccount>(u => u.EmployeeId == employeeId && u.IsActive);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");

            var user = await _userRepository.GetWithSpecAsync(userSpec);

            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by employee ID: {EmployeeId}", employeeId);
            return null;
        }
    }

    private static bool IsValidWinFormsRole(string roleName)
    {
        if (roleName == null)
            return false;

        var validRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
            "guest",
            "customer"
        };
        return !validRoles.Contains(roleName);

    }

    
}
