using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

public class BlogService(ApplicationDbContext dbContext)
{
    /// <summary>Returns all published posts ordered newest first.</summary>
    public async Task<IReadOnlyList<BlogPost>> GetAllPostsAsync() =>
        await dbContext.BlogPosts
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();

    /// <summary>Returns a published post by its URL slug, or null if not found.</summary>
    public async Task<BlogPost?> GetBySlugAsync(string slug) =>
        await dbContext.BlogPosts
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

    /// <summary>Returns all posts (including drafts) for the admin CMS.</summary>
    public async Task<List<BlogPost>> GetAllForAdminAsync() =>
        await dbContext.BlogPosts
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();

    /// <summary>Creates a new blog post and returns it with the assigned Id.</summary>
    public async Task<BlogPost> CreateAsync(BlogPost post)
    {
        dbContext.BlogPosts.Add(post);
        await dbContext.SaveChangesAsync();
        return post;
    }

    /// <summary>Persists changes to an existing blog post.</summary>
    public async Task UpdateAsync(BlogPost post)
    {
        post.UpdatedAt = DateTime.UtcNow;
        dbContext.BlogPosts.Update(post);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>Deletes a blog post by Id. No-op if not found.</summary>
    public async Task DeleteAsync(int id)
    {
        var post = await dbContext.BlogPosts.FindAsync(id);
        if (post is not null)
        {
            dbContext.BlogPosts.Remove(post);
            await dbContext.SaveChangesAsync();
        }
    }
}
