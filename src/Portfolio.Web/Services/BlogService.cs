using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

public class BlogService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    /// <summary>Returns all published posts ordered newest first.</summary>
    public async Task<IReadOnlyList<BlogPost>> GetAllPostsAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.BlogPosts
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();
    }

    /// <summary>Returns a page of published posts and the total published post count.</summary>
    public async Task<(IReadOnlyList<BlogPost> Posts, int TotalCount)> GetPagedPostsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var query = dbContext.BlogPosts
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedDate);

        var total = await query.CountAsync();
        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, total);
    }

    /// <summary>Returns a published post by its URL slug, or null if not found.</summary>
    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.BlogPosts
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
    }

    /// <summary>Returns all posts (including drafts) for the admin CMS.</summary>
    public async Task<List<BlogPost>> GetAllForAdminAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.BlogPosts
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();
    }

    /// <summary>Creates a new blog post and returns it with the assigned Id.</summary>
    public async Task<BlogPost> CreateAsync(BlogPost post)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.BlogPosts.Add(post);
        await dbContext.SaveChangesAsync();
        return post;
    }

    /// <summary>Persists changes to an existing blog post.</summary>
    public async Task UpdateAsync(BlogPost post)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        post.UpdatedAt = DateTime.UtcNow;
        dbContext.BlogPosts.Update(post);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>Deletes a blog post by Id. No-op if not found.</summary>
    public async Task DeleteAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var post = await dbContext.BlogPosts.FindAsync(id);
        if (post is not null)
        {
            dbContext.BlogPosts.Remove(post);
            await dbContext.SaveChangesAsync();
        }
    }
}
