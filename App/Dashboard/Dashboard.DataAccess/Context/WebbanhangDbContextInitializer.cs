using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess.Context;

public class WebbanhangDbContextInitializer(WebbanhangDbContext context)
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();
    }

    public async Task SeedAsync()
    {
        await TrySeedAsync();
    }

    public async Task TrySeedAsync()
    {
        await context.Database.EnsureCreatedAsync();
    }
}