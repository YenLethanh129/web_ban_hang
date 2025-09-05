using Dashboard.BussinessLogic.Dtos.AuthDtos;
using Dashboard.BussinessLogic.Services;
using Dashboard.BussinessLogic.Shared;

namespace Dashboard.Winform.Helpers;

/// <summary>
/// Helper class for WinForms authentication management
/// Provides session management and permission checking for WinForms application
/// </summary>
public static class AuthenticationHelper
{
    private static SessionDto? _currentSession;
    private static IAuthenticationService? _authenticationService;
    private static IAuthorizationService? _authorizationService;

    /// <summary>
    /// Initialize authentication services
    /// </summary>
    public static void Initialize(IAuthenticationService authService, IAuthorizationService authzService)
    {
        _authenticationService = authService;
        _authorizationService = authzService;
    }

    /// <summary>
    /// Current authenticated user session
    /// </summary>
    public static SessionDto? CurrentSession => _currentSession;

    /// <summary>
    /// Current authenticated user ID
    /// </summary>
    public static long? CurrentUserId => _currentSession?.UserId;

    /// <summary>
    /// Current user's role
    /// </summary>
    public static string? CurrentUserRole => _currentSession?.RoleName;

    /// <summary>
    /// Current user's full name
    /// </summary>
    public static string? CurrentUserName => _currentSession?.FullName;

    /// <summary>
    /// Check if user is authenticated
    /// </summary>
    public static bool IsAuthenticated => _currentSession != null && !_currentSession.IsExpired;

    /// <summary>
    /// Check if current user is Admin
    /// </summary>
    public static bool IsAdmin => CurrentUserRole == Roles.ADMIN;

    /// <summary>
    /// Check if current user is Manager
    /// </summary>
    public static bool IsManager => CurrentUserRole == Roles.MANAGER;

    /// <summary>
    /// Check if current user is Employee
    /// </summary>
    public static bool IsEmployee => CurrentUserRole == Roles.EMPLOYEE;

    /// <summary>
    /// Check if current user can manage other users
    /// </summary>
    public static bool CanManageUsers => IsAdmin;

    /// <summary>
    /// Check if current user can manage employees
    /// </summary>
    public static bool CanManageEmployees => IsAdmin || IsManager;

    /// <summary>
    /// Check if current user can view financial data
    /// </summary>
    public static bool CanViewFinancials => IsAdmin || IsManager;

    /// <summary>
    /// Authenticate user and create session
    /// </summary>
    public static async Task<AuthenticationResult> LoginAsync(string phoneNumber, string password)
    {
        if (_authenticationService == null)
            throw new InvalidOperationException("Authentication service not initialized");

        var loginInput = new LoginInput
        {
            PhoneNumber = phoneNumber,
            Password = password
        };

        var result = await _authenticationService.LoginAsync(loginInput);
        
        if (result.IsSuccess && result.Session != null)
        {
            _currentSession = result.Session;
        }

        return result;
    }

    /// <summary>
    /// Logout current user
    /// </summary>
    public static async Task<bool> LogoutAsync()
    {
        if (_authenticationService == null || _currentSession == null)
            return true;

        var result = await _authenticationService.LogoutAsync(_currentSession.UserId);
        
        if (result)
        {
            _currentSession = null;
        }

        return result;
    }

    /// <summary>
    /// Validate current session
    /// </summary>
    public static async Task<bool> ValidateSessionAsync()
    {
        if (_authenticationService == null || _currentSession == null)
            return false;

        var result = await _authenticationService.ValidateSessionAsync(_currentSession.UserId);
        
        if (result.IsSuccess && result.Session != null && !result.Session.IsExpired)
        {
            _currentSession = result.Session;
            return true;
        }

        _currentSession = null;
        return false;
    }

    /// <summary>
    /// Check if current user has specific permission
    /// </summary>
    public static async Task<bool> HasPermissionAsync(string permission)
    {
        if (_authorizationService == null || _currentSession == null)
            return false;

        return await _authorizationService.HasPermissionAsync(_currentSession.UserId, permission);
    }

    /// <summary>
    /// Check if current user can access a resource with specific action
    /// </summary>
    public static async Task<bool> CanAccessResourceAsync(string resource, string action)
    {
        if (_authorizationService == null || _currentSession == null)
            return false;

        return await _authorizationService.CanAccessResourceAsync(_currentSession.UserId, resource, action);
    }

    /// <summary>
    /// Get current user's permissions
    /// </summary>
    public static async Task<List<string>> GetUserPermissionsAsync()
    {
        if (_authorizationService == null || _currentSession == null)
            return new List<string>();

        return await _authorizationService.GetUserPermissionsAsync(_currentSession.UserId);
    }

    /// <summary>
    /// Refresh current session from server
    /// </summary>
    public static async Task<bool> RefreshSessionAsync()
    {
        if (_authenticationService == null || _currentSession == null)
            return false;

        var session = await _authenticationService.GetCurrentSessionAsync(_currentSession.UserId);
        
        if (session != null && !session.IsExpired)
        {
            _currentSession = session;
            return true;
        }

        _currentSession = null;
        return false;
    }

    /// <summary>
    /// Clear current session (for logout or session expiry)
    /// </summary>
    public static void ClearSession()
    {
        _currentSession = null;
    }

    /// <summary>
    /// Get display text for current user
    /// </summary>
    public static string GetUserDisplayText()
    {
        if (_currentSession == null)
            return "Not authenticated";

        var roleName = _currentSession.RoleName switch
        {
            Roles.ADMIN => "Quản trị viên",
            Roles.MANAGER => "Quản lý",
            Roles.EMPLOYEE => "Nhân viên",
            _ => _currentSession.RoleName
        };

        return $"{_currentSession.FullName} ({roleName})";
    }

    /// <summary>
    /// Check permissions for UI elements
    /// </summary>
    public static class UIPermissions
    {
        public static async Task<bool> CanViewUserManagement() 
            => await HasPermissionAsync(Permissions.MANAGE_USERS);

        public static async Task<bool> CanCreateUser() 
            => await HasPermissionAsync(Permissions.MANAGE_USERS);

        public static async Task<bool> CanEditUser() 
            => await HasPermissionAsync(Permissions.MANAGE_USERS);

        public static async Task<bool> CanDeleteUser() 
            => await HasPermissionAsync(Permissions.MANAGE_USERS);

        public static async Task<bool> CanViewReports() 
            => await HasPermissionAsync(Permissions.VIEW_BASIC_REPORTS);

        public static async Task<bool> CanManageInventory() 
            => await HasPermissionAsync(Permissions.MANAGE_INVENTORY);

        public static async Task<bool> CanViewOrders() 
            => await HasPermissionAsync(Permissions.MANAGE_ORDERS);

        public static async Task<bool> CanCreateOrder() 
            => await HasPermissionAsync(Permissions.MANAGE_ORDERS);

        public static async Task<bool> CanViewExpenses() 
            => await HasPermissionAsync(Permissions.MANAGE_EXPENSES);

        public static async Task<bool> CanManageSuppliers() 
            => await HasPermissionAsync(Permissions.MANAGE_SUPPLIERS);

        public static async Task<bool> CanViewSystemSettings() 
            => await HasPermissionAsync(Permissions.MANAGE_SYSTEM_SETTINGS);
    }
}
