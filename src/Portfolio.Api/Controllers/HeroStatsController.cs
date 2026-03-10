using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeroStatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HeroStatsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<HeroStatDto>> GetAll()
    {
        return await _context.HeroStats
            .OrderBy(s => s.SortOrder)
            .Select(s => new HeroStatDto
            {
                Id       = s.Id,
                Value    = s.Value,
                Label    = s.Label,
                Color    = s.Color,
                SortOrder = s.SortOrder
            }).ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HeroStatDto>> Create([FromBody] HeroStatDto dto)
    {
        var stat = new HeroStat
        {
            Value     = dto.Value,
            Label     = dto.Label,
            Color     = dto.Color,
            SortOrder = dto.SortOrder
        };
        _context.HeroStats.Add(stat);
        await _context.SaveChangesAsync();
        dto.Id = stat.Id;
        return CreatedAtAction(nameof(GetAll), dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] HeroStatDto dto)
    {
        var stat = await _context.HeroStats.FindAsync(id);
        if (stat is null) return NotFound();

        stat.Value     = dto.Value;
        stat.Label     = dto.Label;
        stat.Color     = dto.Color;
        stat.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var stat = await _context.HeroStats.FindAsync(id);
        if (stat is null) return NotFound();
        _context.HeroStats.Remove(stat);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
