using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/mailsettings")]
[Authorize(Roles = "Admin")]
public class MailSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MailSettingsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<MailSettingsDto>> Get()
    {
        var settings = await _context.MailSettings.FirstOrDefaultAsync()
            ?? new MailSettings { Id = 1 };
        return ToDto(settings);
    }

    [HttpPut]
    public async Task<IActionResult> Save([FromBody] MailSettingsDto dto)
    {
        var settings = await _context.MailSettings.FirstOrDefaultAsync();
        if (settings is null)
        {
            settings = new MailSettings { Id = 1 };
            _context.MailSettings.Add(settings);
        }

        settings.IsEnabled        = dto.IsEnabled;
        settings.Provider         = dto.Provider;
        settings.FromAddress      = dto.FromAddress;
        settings.FromName         = dto.FromName;
        settings.MailerSendApiKey = dto.MailerSendApiKey;
        settings.SmtpHost         = dto.SmtpHost;
        settings.SmtpPort         = dto.SmtpPort;
        settings.SmtpUsername     = dto.SmtpUsername;
        settings.SmtpPassword     = dto.SmtpPassword;
        settings.UseSsl           = dto.UseSsl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static MailSettingsDto ToDto(MailSettings s) => new()
    {
        Id               = s.Id,
        IsEnabled        = s.IsEnabled,
        Provider         = s.Provider,
        FromAddress      = s.FromAddress,
        FromName         = s.FromName,
        MailerSendApiKey = s.MailerSendApiKey,
        SmtpHost         = s.SmtpHost,
        SmtpPort         = s.SmtpPort,
        SmtpUsername     = s.SmtpUsername,
        SmtpPassword     = s.SmtpPassword,
        UseSsl           = s.UseSsl
    };
}
