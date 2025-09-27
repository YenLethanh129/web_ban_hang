using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.BussinessLogic.Dtos.RBACDtos;
using Dashboard.BussinessLogic.Security;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Helpers;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BussinessLogic.Services.RBACServices
{
    public interface IAuthenticationService
    {
        Task<LoginResult?> LoginAsync(LoginInput dto);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtTokenService _jwtService;
        private readonly DataEncryptionHelper _dataEncryptionHelper;
        public AuthenticationService(IUnitOfWork uow, JwtTokenService jwtService, DataEncryptionHelper dataEncryptionHelper)
        {
            _uow = uow;
            _jwtService = jwtService;
            _dataEncryptionHelper = dataEncryptionHelper;
        }

        public async Task<LoginResult?> LoginAsync(LoginInput dto)
        {
            var userRepo = _uow.Repository<Dashboard.DataAccess.Models.Entities.RBAC.EmployeeUserAccount>();
            var tokenRepo = _uow.Repository<Dashboard.DataAccess.Models.Entities.RBAC.Token>();

            var user = await userRepo.GetQueryable()
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null) return null;
            if (!_dataEncryptionHelper.VerifyPassword(dto.Password, user.Password))
                return null;

            var permissions = user.Role?.RolePermissions?
                .Where(rp => rp.Permission != null)
                .Select(rp => rp.Permission.Name)
                .Where(name => !string.IsNullOrEmpty(name))
                .Select(name => name.ToUpperInvariant())
                .ToList() ?? new List<string>();

            var roleName = user.Role?.Name ?? "EMPLOYEE";

            var tokenString = _jwtService.GenerateToken(user.Id, user.Username, roleName, permissions);
            var expiration = DateTime.UtcNow.AddMinutes(60);

            var token = new Token
            {
                UserId = user.Id,
                TokenValue = tokenString,
                ExpirationDate = expiration,
                Expired = false,
                Revoked = false,
                TokenType = "JWT"
            };

            await tokenRepo.AddAsync(token);
            await _uow.SaveChangesAsync();

            return new LoginResult
            {
                Token = tokenString,
                ExpirationDate = expiration,
                Username = user.Username,
                Role = roleName,
                Permissions = permissions
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var principal = _jwtService.ValidateToken(token);
                if (principal == null) return false;

                var tokenRepo = _uow.Repository<Dashboard.DataAccess.Models.Entities.RBAC.Token>();
                var dbToken = await tokenRepo.GetQueryable()
                    .FirstOrDefaultAsync(t => t.TokenValue == token && !t.Expired && !t.Revoked);

                return dbToken != null && dbToken.ExpirationDate > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync(string token)
        {
            var tokenRepo = _uow.Repository<Dashboard.DataAccess.Models.Entities.RBAC.Token>();
            var dbToken = await tokenRepo.GetQueryable()
                .FirstOrDefaultAsync(t => t.TokenValue == token);
            if (dbToken != null)
            {
                dbToken.Revoked = true;
                tokenRepo.Update(dbToken);
                await _uow.SaveChangesAsync();
            }
        }
    }
}