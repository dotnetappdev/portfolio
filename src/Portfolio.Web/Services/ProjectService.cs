using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>Admin CRUD service for portfolio projects stored in the local Web database.</summary>
public class ProjectService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<List<PortfolioProject>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Projects.OrderBy(p => p.SortOrder).ToListAsync();
    }

    public async Task<PortfolioProject?> GetByIdAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Projects.FindAsync(id);
    }

    public async Task<PortfolioProject?> GetBySlugAsync(string slug)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task CreateAsync(PortfolioProject project)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PortfolioProject project)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Projects.Update(project);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var project = await dbContext.Projects.FindAsync(id);
        if (project != null)
        {
            dbContext.Projects.Remove(project);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Project was already deleted by another request — nothing to do
            }
        }
    }
}
