using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

public interface ITokenCleanupService
{
    Task CleanupExpiredTokensAsync();
}

public class TokenCleanupService : ITokenCleanupService
{
    private readonly IUnitOfWork _uow;

    public TokenCleanupService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var tokenRepo = _uow.Repository<Token>();
        var now = DateTime.UtcNow;

        var expiredTokens = await tokenRepo.GetQueryable()
            .Where(t => t.ExpirationDate < now || t.Revoked || t.Expired)
            .ToListAsync();

        if (expiredTokens.Any())
        {
            tokenRepo.RemoveRange(expiredTokens);
            await _uow.SaveChangesAsync();
        }
    }
}
