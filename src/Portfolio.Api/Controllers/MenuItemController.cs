using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/menuitems")]
public class MenuItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MenuItemController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<MenuItemDto>> GetVisible()
    {
        return await _context.MenuItems
            .Where(m => m.IsVisible)
            .OrderBy(m => m.SortOrder)
            .Select(m => ToDto(m))
            .ToListAsync();
    }

    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<MenuItemDto>> GetAll()
    {
        return await _context.MenuItems
            .OrderBy(m => m.SortOrder)
            .Select(m => ToDto(m))
            .ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuItemDto>> Create([FromBody] MenuItemDto dto)
    {
        var item = FromDto(dto);
        _context.MenuItems.Add(item);
        await _context.SaveChangesAsync();
        dto.Id = item.Id;
        return CreatedAtAction(nameof(GetVisible), dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] MenuItemDto dto)
    {
        var item = await _context.MenuItems.FindAsync(id);
        if (item is null) return NotFound();

        item.Label        = dto.Label;
        item.Url          = dto.Url;
        item.Icon         = dto.Icon;
        item.SortOrder    = dto.SortOrder;
        item.IsVisible    = dto.IsVisible;
        item.OpenInNewTab = dto.OpenInNewTab;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.MenuItems.FindAsync(id);
        if (item is null) return NotFound();
        _context.MenuItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static MenuItemDto ToDto(MenuItem m) => new()
    {
        Id           = m.Id,
        Label        = m.Label,
        Url          = m.Url,
        Icon         = m.Icon,
        SortOrder    = m.SortOrder,
        IsVisible    = m.IsVisible,
        OpenInNewTab = m.OpenInNewTab
    };

    private static MenuItem FromDto(MenuItemDto dto) => new()
    {
        Label        = dto.Label,
        Url          = dto.Url,
        Icon         = dto.Icon,
        SortOrder    = dto.SortOrder,
        IsVisible    = dto.IsVisible,
        OpenInNewTab = dto.OpenInNewTab
    };
}
