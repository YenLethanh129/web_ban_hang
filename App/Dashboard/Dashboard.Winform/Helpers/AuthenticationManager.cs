using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Services.RBACServices;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Winform.Helpers
{
    public static class AuthenticationManager
    {
        private static IAuthenticationService? _authenticationService;
        private static BussinessLogic.Services.RBACServices.IAuthorizationService? _authorizationService;
        private static SessionDto? _currentSession;
        private static string? _currentToken;

        public static event EventHandler<bool>? AuthenticationChanged;
        public static event EventHandler? SessionExpired;

        #region Initialization
        public static void Initialize(IAuthenticationService authService, BussinessLogic.Services.RBACServices.IAuthorizationService authzService)
        {
            _authenticationService = authService ?? throw new ArgumentNullException(nameof(authService));
            _authorizationService = authzService ?? throw new ArgumentNullException(nameof(authzService));
        }
        #endregion

        #region Properties
        public static bool IsAuthenticated => _currentSession != null && !string.IsNullOrEmpty(_currentToken);
        public static SessionDto? CurrentSession => _currentSession;
        public static string? CurrentToken => _currentToken;
        public static long? CurrentUserId => _currentSession?.UserId;
        public static string? CurrentUsername => _currentSession?.Username;
        public static string? CurrentRole => _currentSession?.Role;
        public static List<string>? CurrentPermissions => _currentSession?.Permissions;
        #endregion

        #region Authentication Methods

        public static async Task<LoginResult?> LoginAsync(string username, string password)
        {
            try
            {
                if (_authenticationService == null)
                    throw new InvalidOperationException("Authentication service chưa được khởi tạo");

                var loginInput = new LoginInput { Username = username, Password = password };
                var result = await _authenticationService.LoginAsync(loginInput);

                if (result != null)
                {
                    SetLocalSessionFromLoginResult(result);

                    if (_authorizationService != null && !string.IsNullOrEmpty(_currentToken))
                    {
                        try
                        {
                            var sessionFromToken = await _authorizationService.GetUserFromTokenAsync(_currentToken);
                            if (sessionFromToken != null && _currentSession != null)
                            {
                                _currentSession.UserId = sessionFromToken.UserId;
                                _currentSession.Username = sessionFromToken.Username ?? _currentSession.Username;
                                _currentSession.Role = sessionFromToken.Role ?? _currentSession.Role;
                                _currentSession.Permissions = sessionFromToken.Permissions ?? _currentSession.Permissions;
                                _currentSession.Expiration = sessionFromToken.Expiration;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning - Failed to enrich session from authorization service: {ex.Message}");
                        }
                    }

                    AuthenticationChanged?.Invoke(null, true);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        public static void SetLocalSessionFromLoginResult(LoginResult loginResult)
        {
            ArgumentNullException.ThrowIfNull(loginResult);

            _currentToken = loginResult.Token;
            _currentSession = new SessionDto
            {
                Token = loginResult.Token,
                Username = loginResult.Username,
                Role = loginResult.Role,
                Permissions = loginResult.Permissions != null
                    ? loginResult.Permissions.Select(p => p.ToUpperInvariant()).ToList()
                    : new List<string>(),
                Expiration = loginResult.ExpirationDate
            };

            AuthenticationChanged?.Invoke(null, true);
        }

        public static async Task<bool> LogoutAsync()
        {
            try
            {
                if (_authenticationService != null && !string.IsNullOrEmpty(_currentToken))
                {
                    await _authenticationService.LogoutAsync(_currentToken);
                }

                ClearSession();
                AuthenticationChanged?.Invoke(null, false);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                ClearSession();
                return true;
            }
        }

        public static async Task<bool> ValidateTokenAsync()
        {
            try
            {
                if (_authenticationService == null || string.IsNullOrEmpty(_currentToken))
                    return false;

                var isValid = await _authenticationService.ValidateTokenAsync(_currentToken);

                if (!isValid)
                {
                    ClearSession();
                    SessionExpired?.Invoke(null, EventArgs.Empty);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation error: {ex.Message}");
                ClearSession();
                return false;
            }
        }

        public static void ClearSession()
        {
            _currentSession = null;
            _currentToken = null;
        }
        #endregion

        #region Permission Methods
        public static async Task<bool> HasPermissionAsync(string permission)
        {
            if (!IsAuthenticated)
            {
                return false;
            }

            // Admin bypass
            if (IsAdmin)
            {
                return true;
            }

            if (_authorizationService != null && !string.IsNullOrEmpty(_currentToken))
            {
                try
                {
                    var result = await _authorizationService.HasPermissionAsync(_currentToken, permission);
                    // if service returns, use it; if it throws we fallback below
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"HasPermissionAsync service error: {ex.Message}");
                    // fallback to local permissions below
                }
            }

            var hasLocalPermission = CurrentPermissions?.Contains(permission, StringComparer.OrdinalIgnoreCase) ?? false;
            return hasLocalPermission;
        }

        public static async Task<bool> IsInRoleAsync(string role)
        {
            if (!IsAuthenticated)
            {
                Console.WriteLine($"IsInRoleAsync: Not authenticated");
                return false;
            }

            var currentRole = CurrentRole?.Trim();
            var targetRole = role?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(currentRole) || string.IsNullOrEmpty(targetRole))
            {
                Console.WriteLine($"IsInRoleAsync: Empty role - Current: '{currentRole}', Target: '{targetRole}'");
                return false;
            }

            var result = string.Equals(currentRole, targetRole, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"IsInRoleAsync: Current role '{currentRole}' vs Target role '{targetRole}' = {result}");

            if (_authorizationService != null && !string.IsNullOrEmpty(_currentToken))
            {
                try
                {
                    var serviceResult = await _authorizationService.IsInRoleAsync(_currentToken, role!);
                    return serviceResult;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"IsInRoleAsync service error: {ex.Message}");
                    // fallback to local compare
                }
            }

            return result;
        }

        public static async Task<bool> CanAccessResourceAsync(string resource, string action)
        {
            if (!IsAuthenticated || _authorizationService == null || string.IsNullOrEmpty(_currentToken))
                return false;

            try
            {
                return await _authorizationService.CanAccessResourceAsync(_currentToken, resource, action);
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<string>> GetUserPermissionsAsync()
        {
            if (!IsAuthenticated || _authorizationService == null || string.IsNullOrEmpty(_currentToken))
                return new List<string>();

            try
            {
                return await _authorizationService.GetUserPermissionsAsync(_currentToken);
            }
            catch
            {
                return new List<string>();
            }
        }
        #endregion

        #region Role Helper Methods
        public static bool IsAdmin => CurrentRole?.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) == true;
        public static bool IsManager => CurrentRole?.Equals("MANAGER", StringComparison.OrdinalIgnoreCase) == true;
        public static bool IsEmployee => CurrentRole?.Equals("EMPLOYEE", StringComparison.OrdinalIgnoreCase) == true;
        #endregion
    }
}