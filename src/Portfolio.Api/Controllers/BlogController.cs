using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BlogController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<BlogPostDto>> GetPublished()
    {
        return await _context.BlogPosts
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedDate)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<BlogPostDto>> GetBySlug(string slug)
    {
        var post = await _context.BlogPosts
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        if (post is null) return NotFound();
        return ToDto(post);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<BlogPostDto>> GetAll()
    {
        return await _context.BlogPosts
            .OrderByDescending(p => p.PublishedDate)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BlogPostDto>> Create([FromBody] BlogPostDto dto)
    {
        var post = FromDto(dto);
        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();
        dto.Id = post.Id;
        return CreatedAtAction(nameof(GetBySlug), new { slug = post.Slug }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] BlogPostDto dto)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post is null) return NotFound();

        post.Slug             = dto.Slug;
        post.Title            = dto.Title;
        post.Summary          = dto.Summary;
        post.Category         = dto.Category;
        post.PublishedDate    = dto.PublishedDate;
        post.UpdatedAt        = dto.UpdatedAt;
        post.ReadMinutes      = dto.ReadMinutes;
        post.Tags             = dto.Tags;
        post.Body             = dto.Body;
        post.IsPublished      = dto.IsPublished;
        post.FeaturedImage    = dto.FeaturedImage;
        post.MetaTitle        = dto.MetaTitle;
        post.MetaDescription  = dto.MetaDescription;
        post.OgImage          = dto.OgImage;
        post.CanonicalUrl     = dto.CanonicalUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post is null) return NotFound();
        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static BlogPostDto ToDto(BlogPost p) => new()
    {
        Id               = p.Id,
        Slug             = p.Slug,
        Title            = p.Title,
        Summary          = p.Summary,
        Category         = p.Category,
        PublishedDate    = p.PublishedDate,
        UpdatedAt        = p.UpdatedAt,
        ReadMinutes      = p.ReadMinutes,
        Tags             = p.Tags,
        Body             = p.Body,
        IsPublished      = p.IsPublished,
        FeaturedImage    = p.FeaturedImage,
        MetaTitle        = p.MetaTitle,
        MetaDescription  = p.MetaDescription,
        OgImage          = p.OgImage,
        CanonicalUrl     = p.CanonicalUrl
    };

    private static BlogPost FromDto(BlogPostDto dto) => new()
    {
        Slug             = dto.Slug,
        Title            = dto.Title,
        Summary          = dto.Summary,
        Category         = dto.Category,
        PublishedDate    = dto.PublishedDate,
        UpdatedAt        = dto.UpdatedAt,
        ReadMinutes      = dto.ReadMinutes,
        Tags             = dto.Tags,
        Body             = dto.Body,
        IsPublished      = dto.IsPublished,
        FeaturedImage    = dto.FeaturedImage,
        MetaTitle        = dto.MetaTitle,
        MetaDescription  = dto.MetaDescription,
        OgImage          = dto.OgImage,
        CanonicalUrl     = dto.CanonicalUrl
    };
}
