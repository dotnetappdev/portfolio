using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>Admin CRUD service for portfolio projects stored in the local Web database.</summary>
public class ProjectService(ApplicationDbContext dbContext)
{
    public async Task<List<PortfolioProject>> GetAllAsync() =>
        await dbContext.Projects.OrderBy(p => p.SortOrder).ToListAsync();

    public async Task<PortfolioProject?> GetByIdAsync(int id) =>
        await dbContext.Projects.FindAsync(id);

    public async Task CreateAsync(PortfolioProject project)
    {
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PortfolioProject project)
    {
        dbContext.Projects.Update(project);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var project = await dbContext.Projects.FindAsync(id);
        if (project != null)
        {
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
        }
    }
}
