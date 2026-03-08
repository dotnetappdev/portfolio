using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>Reads and persists the single-row <see cref="AppSettings"/> record.</summary>
public class AppSettingsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    /// <summary>Returns the current app settings row, or a default instance if not yet seeded.</summary>
    public async Task<AppSettings> GetAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.AppSettings.FirstOrDefaultAsync()
               ?? new AppSettings { Id = 1 };
    }

    /// <summary>Persists the app settings row (insert or update).</summary>
    public async Task SaveAsync(AppSettings settings)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existing = await dbContext.AppSettings.FindAsync(settings.Id);
        if (existing == null)
            dbContext.AppSettings.Add(settings);
        else
            dbContext.AppSettings.Update(settings);

        await dbContext.SaveChangesAsync();
    }
}
