using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SkillsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<SkillDto>> GetAll()
    {
        return await _context.Skills
            .OrderByDescending(s => s.Proficiency)
            .Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category,
                Proficiency = s.Proficiency
            }).ToListAsync();
    }
}
