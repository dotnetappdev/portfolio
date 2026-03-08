using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

public class CmsPageService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    /// <summary>Returns a published CMS page by its URL slug, or null if not found.</summary>
    public async Task<CmsPage?> GetBySlugAsync(string slug)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.CmsPages
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
    }

    /// <summary>Returns all pages (including drafts) for the admin CMS.</summary>
    public async Task<List<CmsPage>> GetAllForAdminAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.CmsPages
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();
    }

    /// <summary>Creates a new CMS page and returns it with the assigned Id.</summary>
    public async Task<CmsPage> CreateAsync(CmsPage page)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.CmsPages.Add(page);
        await dbContext.SaveChangesAsync();
        return page;
    }

    /// <summary>Persists changes to an existing CMS page.</summary>
    public async Task UpdateAsync(CmsPage page)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        page.UpdatedAt = DateTime.UtcNow;
        dbContext.CmsPages.Update(page);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>Deletes a CMS page by Id. No-op if not found.</summary>
    public async Task DeleteAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var page = await dbContext.CmsPages.FindAsync(id);
        if (page is not null)
        {
            dbContext.CmsPages.Remove(page);
            await dbContext.SaveChangesAsync();
        }
    }
}
