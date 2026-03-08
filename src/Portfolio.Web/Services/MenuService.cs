using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

public class MenuService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    /// <summary>Returns all visible menu items ordered by SortOrder.</summary>
    public async Task<List<MenuItem>> GetVisibleItemsAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.MenuItems
            .Where(m => m.IsVisible)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    /// <summary>Returns all menu items (including hidden) for the admin CMS.</summary>
    public async Task<List<MenuItem>> GetAllForAdminAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.MenuItems
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    /// <summary>Creates a new menu item.</summary>
    public async Task<MenuItem> CreateAsync(MenuItem item)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.MenuItems.Add(item);
        await dbContext.SaveChangesAsync();
        return item;
    }

    /// <summary>Persists changes to an existing menu item.</summary>
    public async Task UpdateAsync(MenuItem item)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.MenuItems.Update(item);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>Deletes a menu item by Id. No-op if not found.</summary>
    public async Task DeleteAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var item = await dbContext.MenuItems.FindAsync(id);
        if (item is not null)
        {
            dbContext.MenuItems.Remove(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
