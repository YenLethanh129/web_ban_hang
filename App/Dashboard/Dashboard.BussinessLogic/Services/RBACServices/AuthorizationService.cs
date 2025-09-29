using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.DataAccess.Data;

namespace Dashboard.BussinessLogic.Services.RBACServices
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(string token, string permission);
        Task<bool> IsInRoleAsync(string token, string roleName);
        Task<List<string>> GetUserPermissionsAsync(string token);
        Task<SessionDto?> GetUserFromTokenAsync(string token);
        Task<bool> CanAccessResourceAsync(string token, string resource, string action);
        Task<SessionDto> GetUserFromTokenAsync();
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            IUnitOfWork unitOfWork,
            IConfiguration config,
            ILogger<AuthorizationService> logger)
        {
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
            try
            {
                token = NormalizeToken(token);
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("GetUserFromTokenAsync: token is empty");
                    return null;
                }

                var secret = _config["JwtSettings:SecretKey"] ?? _config["JwtSettings:Secret"];
                var issuer = _config["JwtSettings:Issuer"];
                var audience = _config["JwtSettings:Audience"];

                ClaimsPrincipal? principal = null;
                var handler = new JwtSecurityTokenHandler();

                if (!string.IsNullOrWhiteSpace(secret) && !string.IsNullOrWhiteSpace(issuer) && !string.IsNullOrWhiteSpace(audience))
                {
                    var key = Encoding.UTF8.GetBytes(secret);
                    var parameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    try
                    {
                        principal = handler.ValidateToken(token, parameters, out _);
                    }
                    catch (SecurityTokenException ste)
                    {
                        _logger.LogWarning(ste, "GetUserFromTokenAsync: token validation failed");
                        return null;
                    }
                }
                else
                {
                    _logger.LogWarning("GetUserFromTokenAsync: JWT configuration missing, reading token without validation");
                    var jwt = handler.ReadJwtToken(token);
                    var identity = new ClaimsIdentity(jwt.Claims);
                    principal = new ClaimsPrincipal(identity);
                }

                if (principal == null)
                {
                    _logger.LogWarning("GetUserFromTokenAsync: unable to obtain principal from token");
                    return null;
                }

                var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                long userId = 0;
                if (!string.IsNullOrWhiteSpace(sub)) long.TryParse(sub, out userId);

                var username = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
                               ?? principal.FindFirst(ClaimTypes.Name)?.Value
                               ?? principal.Identity?.Name;

                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value
                                ?? principal.FindFirst("role")?.Value
                                ?? principal.FindFirst("roles")?.Value
                                ?? principal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                var permissions = new List<string>();
                permissions.AddRange(principal.FindAll("permission").Select(c => c.Value));
                permissions.AddRange(principal.FindAll("permissions").Select(c => c.Value));
                permissions.AddRange(principal.FindAll("Permission").Select(c => c.Value));
                permissions = permissions.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.ToUpperInvariant()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                var tokenRepo = _unitOfWork.Repository<Dashboard.DataAccess.Models.Entities.RBAC.Token>();

                var tokens = await tokenRepo.GetQueryable()
                    .Where(t => t.TokenValue == token || t.TokenValue == ("Bearer " + token))
                    .ToListAsync();

                var dbToken = tokens.FirstOrDefault(t => t.TokenValue == token
                    || t.TokenValue == ("Bearer " + token)
                    || NormalizeToken(t.TokenValue!) == token);


                if (dbToken == null)
                {
                    _logger.LogWarning("GetUserFromTokenAsync: token not found in database");
                    return null;
                }

                if (dbToken.Revoked || dbToken.Expired || (dbToken.ExpirationDate.HasValue && dbToken.ExpirationDate <= DateTime.UtcNow))
                {
                    _logger.LogWarning("GetUserFromTokenAsync: token is revoked or expired in database");
                    return null;
                }

                var session = new SessionDto
                {
                    Token = token,
                    UserId = userId,
                    Username = username ?? string.Empty,
                    Role = roleClaim ?? string.Empty,
                    Permissions = permissions ?? new List<string>(),
                    Expiration = dbToken.ExpirationDate ?? DateTime.UtcNow
                };

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserFromTokenAsync: unexpected error");
                return null;
            }
        }

        public Task<SessionDto> GetUserFromTokenAsync()
        {
            throw new NotImplementedException("GetUserFromTokenAsync without token is not supported in AuthorizationService");
        }

        public async Task<bool> CanAccessResourceAsync(string token, string resource, string action)
        {
            var session = await GetUserFromTokenAsync(NormalizeToken(token));
            if (session == null) return false;

            var requiredPermission = $"{resource}_{action}".ToUpperInvariant();

            return session.Role?.ToUpperInvariant() == "ADMIN"
                || (session.Permissions?.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase) ?? false);
        }
    }
}