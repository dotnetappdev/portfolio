using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;
using System.Text.Json;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlogController> _logger;

    public BlogController(ApplicationDbContext context, ILogger<BlogController> logger)
    {
        _context = context;
        _logger  = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetPublished()
    {
        try
        {
            var posts = await _context.BlogPosts
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();

            return Ok(posts.Select(p => ToDto(p)).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while fetching published blog posts");
            return StatusCode(500, "An error occurred while fetching blog posts.");
        }
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
        var posts = await _context.BlogPosts
            .OrderByDescending(p => p.PublishedDate)
            .ToListAsync();

        return posts.Select(p => ToDto(p)).ToList();
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

        var mediaJson = SerialiseMediaItems(dto.MediaItems, dto.MediaItemsJson);

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
        post.GitHubUrl        = dto.GitHubUrl;
        post.GalleryImages    = dto.GalleryImages;
        post.MediaItemsJson   = mediaJson;
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

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented        = false
    };

    /// <summary>Serialises the MediaItems list to JSON, or falls back to the raw JSON string.</summary>
    private static string? SerialiseMediaItems(List<BlogMediaItem> items, string? rawJson) =>
        items.Count > 0 ? JsonSerializer.Serialize(items, _jsonOpts) : rawJson;

    private BlogPostDto ToDto(BlogPost p)
    {
        var dto = new BlogPostDto
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
            GitHubUrl        = p.GitHubUrl,
            GalleryImages    = p.GalleryImages,
            MediaItemsJson   = p.MediaItemsJson,
            MetaTitle        = p.MetaTitle,
            MetaDescription  = p.MetaDescription,
            OgImage          = p.OgImage,
            CanonicalUrl     = p.CanonicalUrl
        };

        // Deserialise MediaItemsJson so callers can use the typed list directly.
        if (!string.IsNullOrWhiteSpace(p.MediaItemsJson))
        {
            try
            {
                dto.MediaItems = JsonSerializer.Deserialize<List<BlogMediaItem>>(p.MediaItemsJson, _jsonOpts) ?? [];
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to deserialise MediaItemsJson for BlogPost {Id}", p.Id);
            }
        }

        return dto;
    }

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
        GitHubUrl        = dto.GitHubUrl,
        GalleryImages    = dto.GalleryImages,
        MediaItemsJson   = SerialiseMediaItems(dto.MediaItems, dto.MediaItemsJson),
        MetaTitle        = dto.MetaTitle,
        MetaDescription  = dto.MetaDescription,
        OgImage          = dto.OgImage,
        CanonicalUrl     = dto.CanonicalUrl
    };
}
