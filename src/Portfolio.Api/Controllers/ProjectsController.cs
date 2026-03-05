using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null) return NotFound();
        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
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
}
