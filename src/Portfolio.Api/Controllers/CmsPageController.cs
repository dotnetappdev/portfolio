using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/cmspages")]
public class CmsPageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CmsPageController(ApplicationDbContext context) => _context = context;

    [HttpGet("{slug}")]
    public async Task<ActionResult<CmsPageDto>> GetBySlug(string slug)
    {
        var page = await _context.CmsPages
            .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        if (page is null) return NotFound();
        return ToDto(page);
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<CmsPageDto>> GetAll()
    {
        return await _context.CmsPages
            .OrderByDescending(p => p.PublishedDate)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CmsPageDto>> Create([FromBody] CmsPageDto dto)
    {
        var page = FromDto(dto);
        _context.CmsPages.Add(page);
        await _context.SaveChangesAsync();
        dto.Id = page.Id;
        return CreatedAtAction(nameof(GetBySlug), new { slug = page.Slug }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CmsPageDto dto)
    {
        var page = await _context.CmsPages.FindAsync(id);
        if (page is null) return NotFound();

        page.Title           = dto.Title;
        page.Slug            = dto.Slug;
        page.Body            = dto.Body;
        page.IsPublished     = dto.IsPublished;
        page.PublishedDate   = dto.PublishedDate;
        page.UpdatedAt       = dto.UpdatedAt;
        page.FeaturedImage   = dto.FeaturedImage;
        page.MetaTitle       = dto.MetaTitle;
        page.MetaDescription = dto.MetaDescription;
        page.OgImage         = dto.OgImage;
        page.CanonicalUrl    = dto.CanonicalUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var page = await _context.CmsPages.FindAsync(id);
        if (page is null) return NotFound();
        _context.CmsPages.Remove(page);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static CmsPageDto ToDto(CmsPage p) => new()
    {
        Id               = p.Id,
        Title            = p.Title,
        Slug             = p.Slug,
        Body             = p.Body,
        IsPublished      = p.IsPublished,
        PublishedDate    = p.PublishedDate,
        UpdatedAt        = p.UpdatedAt,
        FeaturedImage    = p.FeaturedImage,
        MetaTitle        = p.MetaTitle,
        MetaDescription  = p.MetaDescription,
        OgImage          = p.OgImage,
        CanonicalUrl     = p.CanonicalUrl
    };

    private static CmsPage FromDto(CmsPageDto dto) => new()
    {
        Title            = dto.Title,
        Slug             = dto.Slug,
        Body             = dto.Body,
        IsPublished      = dto.IsPublished,
        PublishedDate    = dto.PublishedDate,
        UpdatedAt        = dto.UpdatedAt,
        FeaturedImage    = dto.FeaturedImage,
        MetaTitle        = dto.MetaTitle,
        MetaDescription  = dto.MetaDescription,
        OgImage          = dto.OgImage,
        CanonicalUrl     = dto.CanonicalUrl
    };
}
