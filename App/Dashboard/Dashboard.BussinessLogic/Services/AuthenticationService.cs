using AutoMapper;
using Dashboard.BussinessLogic.Dtos.AuthDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.BussinessLogic.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(LoginInput input);
    Task<bool> LogoutAsync(long userId);
    Task<AuthenticationResult> ValidateSessionAsync(long userId);
    Task<bool> ChangePasswordAsync(ChangePasswordInput input);
    Task<SessionDto?> GetCurrentSessionAsync(long userId);
    Task<bool> HasPermissionAsync(long userId, string permission);
    Task<List<string>> GetUserPermissionsAsync(long userId);
}

public class AuthenticationService : BaseTransactionalService, IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuthenticationService> _logger;
    private static readonly Dictionary<long, SessionDto> _activeSessions = new();

    public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthenticationService> logger) 
        : base(unitOfWork)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthenticationResult> LoginAsync(LoginInput input)
    {
        try
        {
            _logger.LogInformation("Login attempt for phone: {PhoneNumber}", input.PhoneNumber);

            // Find user by phone number
            var userSpec = new Specification<User>(u => u.PhoneNumber == input.PhoneNumber && u.IsActive);
            userSpec.IncludeStrings.Add("Role");
            userSpec.IncludeStrings.Add("Employee");
            userSpec.IncludeStrings.Add("Employee.Branch");
            userSpec.IncludeStrings.Add("Role.RolePermissions");
            userSpec.IncludeStrings.Add("Role.RolePermissions.Permission");

            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found with phone: {PhoneNumber}", input.PhoneNumber);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Số điện thoại hoặc mật khẩu không đúng."
                };
            }

            // Verify password
            if (!VerifyPassword(input.Password, user.Password))
            {
                _logger.LogWarning("Invalid password for user: {UserId}", user.Id);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Số điện thoại hoặc mật khẩu không đúng."
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Inactive user login attempt: {UserId}", user.Id);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Tài khoản đã bị vô hiệu hóa."
                };
            }

            // Check role restrictions for WinForms (only ADMIN, MANAGER, EMPLOYEE)
            if (user.Role?.Name == null || !IsValidWinFormsRole(user.Role.Name))
            {
                _logger.LogWarning("Invalid role for WinForms access: {Role}", user.Role?.Name);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Bạn không có quyền truy cập ứng dụng này."
                };
            }

            // Create session
            var permissions = user.Role.RolePermissions
                .Select(rp => rp.Permission.Name)
                .ToList();

            var sessionId = Guid.NewGuid().ToString();
            var session = new SessionDto
            {
                SessionId = sessionId,
                UserId = user.Id,
                FullName = user.Fullname ?? "",
                RoleName = user.Role.Name,
                EmployeeId = user.EmployeeId,
                Position = user.Employee?.Position,
                BranchId = user.Employee?.BranchId,
                BranchName = user.Employee?.Branch?.Name,
                Permissions = permissions,
                LoginTime = DateTime.Now
            };

            // Store session
            _activeSessions[user.Id] = session;

            _logger.LogInformation("Successful login for user: {UserId}, Role: {Role}", user.Id, user.Role.Name);

            return new AuthenticationResult
            {
                IsSuccess = true,
                Session = session
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for phone: {PhoneNumber}", input.PhoneNumber);
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng nhập."
            };
        }
    }

    public Task<bool> LogoutAsync(long userId)
    {
        try
        {
            _activeSessions.Remove(userId);
            _logger.LogInformation("User logged out: {UserId}", userId);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", userId);
            return Task.FromResult(false);
        }
    }

    public async Task<AuthenticationResult> ValidateSessionAsync(long userId)
    {
        try
        {
            if (!_activeSessions.ContainsKey(userId))
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Phiên đăng nhập đã hết hạn."
                };
            }

            var session = _activeSessions[userId];

            // Verify user still exists and is active
            var userSpec = new Specification<User>(u => u.Id == userId && u.IsActive);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _activeSessions.Remove(userId);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Tài khoản không tồn tại hoặc đã bị vô hiệu hóa."
                };
            }

            return new AuthenticationResult
            {
                IsSuccess = true,
                Session = session
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating session for user: {UserId}", userId);
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = "Đã xảy ra lỗi khi xác thực phiên đăng nhập."
            };
        }
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordInput input)
    {
        try
        {
            var userSpec = new Specification<User>(u => u.Id == input.UserId);
            var user = await _unitOfWork.Repository<User>().GetWithSpecAsync(userSpec);

            if (user == null)
            {
                _logger.LogWarning("User not found for password change: {UserId}", input.UserId);
                return false;
            }

            // Verify current password
            if (!VerifyPassword(input.CurrentPassword, user.Password))
            {
                _logger.LogWarning("Invalid current password for user: {UserId}", input.UserId);
                return false;
            }

            // Hash new password
            user.Password = HashPassword(input.NewPassword);
            user.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Password changed for user: {UserId}", input.UserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", input.UserId);
            return false;
        }
    }

    public Task<SessionDto?> GetCurrentSessionAsync(long userId)
    {
        _activeSessions.TryGetValue(userId, out var session);
        return Task.FromResult(session);
    }

    public async Task<bool> HasPermissionAsync(long userId, string permission)
    {
        try
        {
            var session = await GetCurrentSessionAsync(userId);
            return session?.Permissions.Contains(permission) ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission {Permission} for user: {UserId}", permission, userId);
            return false;
        }
    }

    public async Task<List<string>> GetUserPermissionsAsync(long userId)
    {
        try
        {
            var session = await GetCurrentSessionAsync(userId);
            return session?.Permissions ?? new List<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for user: {UserId}", userId);
            return new List<string>();
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

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashToVerify = HashPassword(password);
        return hashToVerify == hashedPassword;
    }
}
