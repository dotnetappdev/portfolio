using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<ProjectDto>> GetAll()
    {
        return await _context.Projects
            .OrderBy(p => p.SortOrder)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Description = p.Description,
                ShortDescription = p.ShortDescription,
                TechStack = p.TechStack,
                GitHubUrl = p.GitHubUrl,
                LiveUrl = p.LiveUrl,
                ImageUrl = p.ImageUrl,
                Category = p.Category,
                IsFeatured = p.IsFeatured,
                SortOrder = p.SortOrder
            }).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return NotFound();
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Slug = project.Slug,
            Description = project.Description,
            ShortDescription = project.ShortDescription,
            TechStack = project.TechStack,
            GitHubUrl = project.GitHubUrl,
            LiveUrl = project.LiveUrl,
            ImageUrl = project.ImageUrl,
            Category = project.Category,
            IsFeatured = project.IsFeatured,
            SortOrder = project.SortOrder
        };
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<ProjectDto>> GetBySlug(string slug)
    {
        var normalised = slug.ToLowerInvariant();
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Slug == normalised);
        if (project is null) return NotFound();
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Slug = project.Slug,
            Description = project.Description,
            ShortDescription = project.ShortDescription,
            TechStack = project.TechStack,
            GitHubUrl = project.GitHubUrl,
            LiveUrl = project.LiveUrl,
            ImageUrl = project.ImageUrl,
            Category = project.Category,
            IsFeatured = project.IsFeatured,
            SortOrder = project.SortOrder
        };
    }

    [HttpGet("featured")]
    public async Task<IEnumerable<ProjectDto>> GetFeatured()
    {
        return await _context.Projects
            .Where(p => p.IsFeatured)
            .OrderBy(p => p.SortOrder)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Description = p.Description,
                ShortDescription = p.ShortDescription,
                TechStack = p.TechStack,
                GitHubUrl = p.GitHubUrl,
                LiveUrl = p.LiveUrl,
                ImageUrl = p.ImageUrl,
                Category = p.Category,
                IsFeatured = p.IsFeatured,
                SortOrder = p.SortOrder
            }).ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectDto dto)
    {
        dto.Slug = dto.Slug.ToLowerInvariant();
        var slugExists = await _context.Projects.AnyAsync(p => p.Slug == dto.Slug);
        if (slugExists)
            return BadRequest($"A project with slug '{dto.Slug}' already exists.");

        var project = new Project
        {
            Title            = dto.Title,
            Slug             = dto.Slug,
            ShortDescription = dto.ShortDescription,
            Description      = dto.Description,
            TechStack        = dto.TechStack,
            GitHubUrl        = dto.GitHubUrl,
            LiveUrl          = dto.LiveUrl,
            ImageUrl         = dto.ImageUrl,
            Category         = dto.Category,
            IsFeatured       = dto.IsFeatured,
            SortOrder        = dto.SortOrder
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        dto.Id = project.Id;
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return NotFound();

        var normalisedSlug = dto.Slug.ToLowerInvariant();
        if (normalisedSlug != project.Slug)
        {
            var slugExists = await _context.Projects.AnyAsync(p => p.Slug == normalisedSlug && p.Id != id);
            if (slugExists)
                return BadRequest($"A project with slug '{normalisedSlug}' already exists.");
        }

        project.Title            = dto.Title;
        project.Slug             = normalisedSlug;
        project.ShortDescription = dto.ShortDescription;
        project.Description      = dto.Description;
        project.TechStack        = dto.TechStack;
        project.GitHubUrl        = dto.GitHubUrl;
        project.LiveUrl          = dto.LiveUrl;
        project.ImageUrl         = dto.ImageUrl;
        project.Category         = dto.Category;
        project.IsFeatured       = dto.IsFeatured;
        project.SortOrder        = dto.SortOrder;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return NotFound();
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
