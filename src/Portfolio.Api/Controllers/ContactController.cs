using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Api.Services;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(ApplicationDbContext context, IEmailService emailService, ILogger<ContactController> logger)
    {
        _context      = context;
        _emailService = emailService;
        _logger       = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ContactMessageDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var message = new ContactMessage
        {
            Name      = dto.Name,
            Email     = dto.Email,
            Subject   = dto.Subject,
            Message   = dto.Message,
            CreatedAt = DateTime.UtcNow
        };
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        // Send email notification to the site owner (best-effort; never block the response).
        try
        {
            var mailSettings = await _context.MailSettings.FirstOrDefaultAsync();
            if (mailSettings is { IsEnabled: true })
            {
                var appSettings = await _context.AppSettings.FirstOrDefaultAsync();
                var to = string.IsNullOrWhiteSpace(appSettings?.VisitorNotificationEmail)
                    ? mailSettings.FromAddress
                    : appSettings.VisitorNotificationEmail;

                if (!string.IsNullOrWhiteSpace(to))
                {
                    var subject = $"Portfolio contact form: {dto.Subject ?? "(no subject)"}";
                    var body    = BuildContactEmailBody(dto);
                    await _emailService.SendAsync(mailSettings, to, subject, body, isHtml: true);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Contact email notification failed (message was still saved).");
        }

        return Ok(new { message = "Thank you for your message! I'll get back to you soon." });
    }

    private static string BuildContactEmailBody(ContactMessageDto dto) =>
        $"""
        <h2>New contact form submission</h2>
        <table cellpadding="6" style="border-collapse:collapse;">
          <tr><td><strong>Name</strong></td><td>{System.Net.WebUtility.HtmlEncode(dto.Name)}</td></tr>
          <tr><td><strong>Email</strong></td><td><a href="mailto:{System.Net.WebUtility.HtmlEncode(dto.Email)}">{System.Net.WebUtility.HtmlEncode(dto.Email)}</a></td></tr>
          <tr><td><strong>Subject</strong></td><td>{System.Net.WebUtility.HtmlEncode(dto.Subject ?? "")}</td></tr>
        </table>
        <h3>Message</h3>
        <p style="white-space:pre-wrap">{System.Net.WebUtility.HtmlEncode(dto.Message)}</p>
        """;
}
