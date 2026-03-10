using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Services;
using Portfolio.Sms.Abstractions;
using Portfolio.Sms.ClickSend;
using Portfolio.Sms.Twilio;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationsController> _logger;
    private readonly IConfiguration _config;

    public NotificationsController(
        ApplicationDbContext context,
        IHttpClientFactory httpFactory,
        IEmailService emailService,
        ILogger<NotificationsController> logger,
        IConfiguration config)
    {
        _context      = context;
        _httpFactory  = httpFactory;
        _emailService = emailService;
        _logger       = logger;
        _config       = config;
    }

    public sealed record VisitorPayload(string Ip, string Page, string Time);
    public sealed record SendToPayload(string? To);

    [HttpPost("visitor-arrived")]
    public async Task<IActionResult> VisitorArrived([FromBody] VisitorPayload payload, CancellationToken ct)
    {
        var apiKey = _config["ServiceApiKey"];
        var header = Request.Headers["X-Service-Api-Key"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey) || header != apiKey)
            return Unauthorized();

        var appSettings = await _context.AppSettings.FirstOrDefaultAsync(ct);
        if (appSettings is null || !appSettings.VisitorNotificationsEnabled)
            return Ok();

        var mailSettings = await _context.MailSettings.FirstOrDefaultAsync(ct);
        if (mailSettings is null || !mailSettings.IsEnabled)
            return Ok();

        var to = string.IsNullOrWhiteSpace(appSettings.VisitorNotificationEmail)
            ? mailSettings.FromAddress
            : appSettings.VisitorNotificationEmail;

        if (string.IsNullOrWhiteSpace(to))
            return Ok();

        var body = BuildVisitorEmailBody(appSettings.VisitorEmailTemplate, payload.Ip, payload.Page, payload.Time);

        await _emailService.SendAsync(mailSettings, to, "New visitor on your portfolio", body, isHtml: true, ct);

        return Ok();
    }

    [HttpPost("test-email")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TestEmail([FromBody] SendToPayload payload, CancellationToken ct)
    {
        var mailSettings = await _context.MailSettings.FirstOrDefaultAsync(ct);
        if (mailSettings is null || !mailSettings.IsEnabled)
            return BadRequest("Email is not enabled or configured.");

        var to = payload.To;
        if (string.IsNullOrWhiteSpace(to))
            return BadRequest("Recipient address is required.");

        var sent = await _emailService.SendAsync(
            mailSettings, to,
            "Portfolio: Email test",
            "<p>Your email settings are working correctly.</p>",
            isHtml: true, ct);

        return sent ? Ok() : StatusCode(500, "Failed to send test email. Check server logs.");
    }

    [HttpPost("test-sms")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> TestSms([FromBody] SendToPayload payload, CancellationToken ct)
    {
        var smsSettings = await _context.SmsSettings.FirstOrDefaultAsync(ct);
        if (smsSettings is null || !smsSettings.IsEnabled)
            return BadRequest("SMS is not enabled or configured.");

        var to = string.IsNullOrWhiteSpace(payload.To)
            ? smsSettings.AdminReceiverNumber
            : payload.To;

        if (string.IsNullOrWhiteSpace(to))
            return BadRequest("No recipient number provided and AdminReceiverNumber is not configured.");

        var result = await SendSmsAsync(smsSettings, to, "Portfolio: SMS test - your SMS settings are working correctly.", ct);

        return result.Succeeded ? Ok() : StatusCode(500, result.Error ?? "Failed to send test SMS. Check server logs.");
    }

    // ── SMS helpers ──────────────────────────────────────────────────────────

    private async Task<SmsResult> SendSmsAsync(
        Models.SmsSettings settings, string to, string body, CancellationToken ct)
    {
        try
        {
            ISmsService svc = settings.Provider switch
            {
                "Twilio" => new TwilioSmsService(
                    _httpFactory.CreateClient("Twilio"),
                    new TwilioOptions
                    {
                        AccountSid = settings.TwilioAccountSid ?? string.Empty,
                        AuthToken  = settings.TwilioAuthToken  ?? string.Empty,
                        From       = settings.TwilioFromNumber ?? string.Empty
                    }),

                "ClickSend" => new ClickSendSmsService(
                    _httpFactory.CreateClient("ClickSend"),
                    new ClickSendOptions
                    {
                        Username = settings.ClickSendUsername ?? string.Empty,
                        ApiKey   = settings.ClickSendApiKey   ?? string.Empty,
                        From     = settings.ClickSendFromName
                    }),

                _ => throw new InvalidOperationException($"Unknown SMS provider: '{settings.Provider}'")
            };

            var result = await svc.SendAsync(new SmsMessage(to, body), ct);

            if (result.Succeeded)
                _logger.LogInformation("SMS sent via {Provider} to {To} (id: {Id})",
                    settings.Provider, to, result.MessageId);
            else
                _logger.LogWarning("SMS send via {Provider} to {To} failed: {Error}",
                    settings.Provider, to, result.Error);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending SMS via {Provider} to {To}",
                settings.Provider, to);
            return SmsResult.Fail(ex.Message);
        }
    }

    private static string BuildVisitorEmailBody(string? template, string ip, string page, string time)
    {
        if (!string.IsNullOrWhiteSpace(template))
        {
            return template
                .Replace("{{ip}}", ip)
                .Replace("{{page}}", page)
                .Replace("{{time}}", time);
        }

        return $"<p>A new visitor arrived on your portfolio.</p>" +
               $"<ul><li><strong>IP:</strong> {ip}</li>" +
               $"<li><strong>Page:</strong> {page}</li>" +
               $"<li><strong>Time:</strong> {time}</li></ul>";
    }
}
