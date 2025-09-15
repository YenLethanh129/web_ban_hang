using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services.RBACServices
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(string token, string permission);
        Task<bool> IsInRoleAsync(string token, string roleName);
        Task<List<string>> GetUserPermissionsAsync(string token);
        Task<SessionDto?> GetUserFromTokenAsync(string token);
        Task<bool> CanAccessResourceAsync(string token, string resource, string action);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config,
            ILogger<AuthorizationService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _config = config;
            _logger = logger;
        }

        private static string NormalizeToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return string.Empty;
            token = token.Trim();
            const string bearer = "Bearer ";
            if (token.StartsWith(bearer, StringComparison.OrdinalIgnoreCase))
                return token.Substring(bearer.Length).Trim();
            return token;
        }

        public async Task<bool> HasPermissionAsync(string token, string permission)
        {
            try
            {
                var session = await GetUserFromTokenAsync(NormalizeToken(token));
                if (session == null)
                {
                    _logger.LogWarning("HasPermissionAsync: session null for token");
                    return false;
                }

                if (!string.IsNullOrEmpty(session.Role) && string.Equals(session.Role, "ADMIN", StringComparison.OrdinalIgnoreCase))
                    return true;

                return session.Permissions?.Any(p => string.Equals(p, permission, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission {Permission} for token", permission);
                return false;
            }
        }

        public async Task<bool> IsInRoleAsync(string token, string roleName)
        {
            try
            {
                var session = await GetUserFromTokenAsync(NormalizeToken(token));
                if (session == null) return false;
                return !string.IsNullOrEmpty(session.Role) && string.Equals(session.Role, roleName, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking role {Role} for token", roleName);
                return false;
            }
        }

        public async Task<List<string>> GetUserPermissionsAsync(string token)
        {
            try
            {
                var session = await GetUserFromTokenAsync(NormalizeToken(token));
                if (session == null) return new List<string>();
                return session.Permissions ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions for token");
                return new List<string>();
            }
        }

        public async Task<SessionDto?> GetUserFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(NormalizeToken(token));

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
                    return null;

                var userSpec = new Specification<EmployeeUserAccount>(u => u.Id == userId && u.IsActive);
                userSpec.IncludeStrings.Add("Role");
                userSpec.IncludeStrings.Add("Role.RolePermissions");
                userSpec.IncludeStrings.Add("Role.RolePermissions.Permission");

                var user = await _userRepository.GetWithSpecAsync(userSpec);
                if (user == null) return null;

                var permissions = user.Role?.RolePermissions
                    .Select(rp => rp.Permission?.Name ?? string.Empty)
                    .Where(n => !string.IsNullOrEmpty(n))
                    .ToList() ?? new List<string>();

                return new SessionDto
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username,
                    Role = user.Role?.Name ?? string.Empty,
                    Permissions = permissions,
                    Expiration = jwtToken.ValidTo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding token {Token}", token);
                return null;
            }
        }

        public async Task<bool> CanAccessResourceAsync(string token, string resource, string action)
        {
            var session = await GetUserFromTokenAsync(NormalizeToken(token));
            if (session == null) return false;

            var requiredPermission = $"{resource}_{action}";
            if (session.Permissions != null && session.Permissions.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase))
                return true;

            return session.Role?.ToUpperInvariant() switch
            {
                "ADMIN" => true,
                "MANAGER" => !string.Equals(resource, "SYSTEM", StringComparison.OrdinalIgnoreCase),
                "EMPLOYEE" => new[] { "INVENTORY", "ORDERS", "CUSTOMERS" }.Contains(resource?.ToUpperInvariant())
                               && !string.Equals(action, "DELETE", StringComparison.OrdinalIgnoreCase)
                               && !string.Equals(action, "MANAGE", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
    }
}

