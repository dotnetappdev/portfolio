using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ContactMessageDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var message = new ContactMessage
        {
            Name = dto.Name,
            Email = dto.Email,
            Subject = dto.Subject,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Thank you for your message! I'll get back to you soon." });
    }
}
