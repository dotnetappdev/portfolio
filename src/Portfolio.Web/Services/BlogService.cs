namespace Portfolio.Web.Services;

public class BlogService(PortfolioApiService apiService)
{
    public async Task<IReadOnlyList<Portfolio.Shared.Models.BlogPostDto>> GetAllPostsAsync()
        => await apiService.GetBlogPostsAsync();

    public async Task<(IReadOnlyList<Portfolio.Shared.Models.BlogPostDto> Posts, int TotalCount)> GetPagedPostsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 1;
        var all = await apiService.GetBlogPostsAsync();
        var total = all.Count;
        var posts = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (posts, total);
    }

    public async Task<Portfolio.Shared.Models.BlogPostDto?> GetBySlugAsync(string slug)
        => await apiService.GetBlogPostBySlugAsync(slug);

    public async Task<List<Portfolio.Shared.Models.BlogPostDto>> GetAllForAdminAsync(string adminToken)
        => await apiService.GetBlogPostsForAdminAsync(adminToken);

    public async Task<Portfolio.Shared.Models.BlogPostDto> CreateAsync(Portfolio.Shared.Models.BlogPostDto post, string adminToken)
    {
        var (result, _) = await apiService.CreateBlogPostAsync(post, adminToken);
        return result ?? post;
    }

    public async Task UpdateAsync(Portfolio.Shared.Models.BlogPostDto post, string adminToken)
        => await apiService.UpdateBlogPostAsync(post.Id, post, adminToken);

    public async Task DeleteAsync(int id, string adminToken)
        => await apiService.DeleteBlogPostAsync(id, adminToken);
}
