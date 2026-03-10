using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/smssettings")]
[Authorize(Roles = "Admin")]
public class SmsSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SmsSettingsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<SmsSettingsDto>> Get()
    {
        var settings = await _context.SmsSettings.FirstOrDefaultAsync()
            ?? new SmsSettings { Id = 1 };
        return ToDto(settings);
    }

    [HttpPut]
    public async Task<IActionResult> Save([FromBody] SmsSettingsDto dto)
    {
        var settings = await _context.SmsSettings.FirstOrDefaultAsync();
        if (settings is null)
        {
            settings = new SmsSettings { Id = 1 };
            _context.SmsSettings.Add(settings);
        }

        settings.Provider              = dto.Provider;
        settings.IsEnabled             = dto.IsEnabled;
        settings.AdminReceiverNumber   = dto.AdminReceiverNumber;
        settings.TwilioAccountSid      = dto.TwilioAccountSid;
        settings.TwilioAuthToken       = dto.TwilioAuthToken;
        settings.TwilioFromNumber      = dto.TwilioFromNumber;
        settings.ClickSendUsername     = dto.ClickSendUsername;
        settings.ClickSendApiKey       = dto.ClickSendApiKey;
        settings.ClickSendFromName     = dto.ClickSendFromName;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static SmsSettingsDto ToDto(SmsSettings s) => new()
    {
        Id                   = s.Id,
        Provider             = s.Provider,
        IsEnabled            = s.IsEnabled,
        AdminReceiverNumber  = s.AdminReceiverNumber,
        TwilioAccountSid     = s.TwilioAccountSid,
        TwilioAuthToken      = s.TwilioAuthToken,
        TwilioFromNumber     = s.TwilioFromNumber,
        ClickSendUsername    = s.ClickSendUsername,
        ClickSendApiKey      = s.ClickSendApiKey,
        ClickSendFromName    = s.ClickSendFromName
    };
}
