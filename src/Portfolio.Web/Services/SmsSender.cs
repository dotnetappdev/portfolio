using Microsoft.EntityFrameworkCore;
using Portfolio.Sms.Abstractions;
using Portfolio.Sms.ClickSend;
using Portfolio.Sms.Twilio;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>
/// Reads SMS provider settings from the database and dispatches messages via the
/// configured provider (Twilio or ClickSend). Changes in the admin panel take effect
/// immediately on the next send — no app restart required.
/// </summary>
public sealed class SmsSender
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<SmsSender> _logger;

    public SmsSender(
        ApplicationDbContext db,
        IHttpClientFactory httpFactory,
        ILogger<SmsSender> logger)
    {
        _db = db;
        _httpFactory = httpFactory;
        _logger = logger;
    }

    /// <summary>
    /// Sends a message to the admin receiver number configured in the SMS settings.
    /// Returns a failed result (without throwing) when SMS is disabled or misconfigured.
    /// </summary>
    public async Task<SmsResult> SendToAdminAsync(string body, CancellationToken ct = default)
    {
        var settings = await _db.SmsSettings.FirstOrDefaultAsync(ct);
        if (settings == null || !settings.IsEnabled)
            return SmsResult.Fail("SMS is not enabled.");

        if (string.IsNullOrWhiteSpace(settings.AdminReceiverNumber))
            return SmsResult.Fail("Admin receiver number is not configured.");

        return await SendInternalAsync(settings, settings.AdminReceiverNumber, body, ct);
    }

    /// <summary>
    /// Sends a message to an arbitrary recipient using the configured provider.
    /// Returns a failed result (without throwing) when SMS is disabled or misconfigured.
    /// </summary>
    public async Task<SmsResult> SendAsync(string to, string body, CancellationToken ct = default)
    {
        var settings = await _db.SmsSettings.FirstOrDefaultAsync(ct);
        if (settings == null || !settings.IsEnabled)
            return SmsResult.Fail("SMS is not enabled.");

        return await SendInternalAsync(settings, to, body, ct);
    }

    private async Task<SmsResult> SendInternalAsync(
        Portfolio.Web.Data.SmsSettings settings, string to, string body, CancellationToken ct)
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
}
