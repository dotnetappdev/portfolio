using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/appsettings")]
[Authorize(Roles = "Admin")]
public class AppSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppSettingsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<AppSettingsDto>> Get()
    {
        var settings = await _context.AppSettings.FirstOrDefaultAsync()
            ?? new AppSettings { Id = 1 };
        return ToDto(settings);
    }

    /// <summary>Returns non-sensitive public settings (e.g. Google Analytics ID).</summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<ActionResult<PublicAppSettingsDto>> GetPublic()
    {
        var settings = await _context.AppSettings.FirstOrDefaultAsync();
        return new PublicAppSettingsDto
        {
            GoogleAnalyticsId = settings?.GoogleAnalyticsId,
            PrimaryColor      = settings?.PrimaryColor,
            SecondaryColor    = settings?.SecondaryColor,
            TertiaryColor     = settings?.TertiaryColor
        };
    }

    [HttpPut]
    public async Task<IActionResult> Save([FromBody] AppSettingsDto dto)
    {
        var settings = await _context.AppSettings.FirstOrDefaultAsync();
        if (settings is null)
        {
            settings = new AppSettings { Id = 1 };
            _context.AppSettings.Add(settings);
        }

        settings.ApiBaseUrl                  = dto.ApiBaseUrl;
        settings.GoogleAnalyticsId           = dto.GoogleAnalyticsId;
        settings.VisitorNotificationsEnabled = dto.VisitorNotificationsEnabled;
        settings.VisitorNotificationEmail    = dto.VisitorNotificationEmail;
        settings.VisitorEmailTemplate        = dto.VisitorEmailTemplate;
        settings.PrimaryColor                = string.IsNullOrWhiteSpace(dto.PrimaryColor) ? null : dto.PrimaryColor;
        settings.SecondaryColor              = string.IsNullOrWhiteSpace(dto.SecondaryColor) ? null : dto.SecondaryColor;
        settings.TertiaryColor               = string.IsNullOrWhiteSpace(dto.TertiaryColor) ? null : dto.TertiaryColor;
        settings.BlogPostImageSlots          = dto.BlogPostImageSlots > 0 ? dto.BlogPostImageSlots : 10;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static AppSettingsDto ToDto(AppSettings s) => new()
    {
        Id                           = s.Id,
        ApiBaseUrl                   = s.ApiBaseUrl,
        GoogleAnalyticsId            = s.GoogleAnalyticsId,
        VisitorNotificationsEnabled  = s.VisitorNotificationsEnabled,
        VisitorNotificationEmail     = s.VisitorNotificationEmail,
        VisitorEmailTemplate         = s.VisitorEmailTemplate,
        PrimaryColor                 = s.PrimaryColor,
        SecondaryColor               = s.SecondaryColor,
        TertiaryColor                = s.TertiaryColor,
        BlogPostImageSlots           = s.BlogPostImageSlots > 0 ? s.BlogPostImageSlots : 10,
    };
}
