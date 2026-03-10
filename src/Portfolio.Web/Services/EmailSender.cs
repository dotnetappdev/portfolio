using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>
/// Sends emails using settings stored in the database.
/// Supports "Smtp" (System.Net.Mail) and "MailerSend" (REST API).
/// Reads settings on every send so admin changes take effect immediately without a restart.
/// </summary>
public sealed class EmailSender
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IDbContextFactory<ApplicationDbContext> dbFactory,
        IHttpClientFactory httpFactory,
        ILogger<EmailSender> logger)
    {
        _dbFactory   = dbFactory;
        _httpFactory = httpFactory;
        _logger      = logger;
    }

    /// <summary>
    /// Sends an email. Returns <c>true</c> on success, <c>false</c> when mail is disabled or
    /// misconfigured. Never throws — errors are logged.
    /// </summary>
    public async Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false,
        CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        var settings = await db.MailSettings.FirstOrDefaultAsync(ct);

        if (settings == null || !settings.IsEnabled)
        {
            _logger.LogDebug("Email not sent — mail is disabled.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.FromAddress))
        {
            _logger.LogWarning("Email not sent — From Address is not configured.");
            return false;
        }

        try
        {
            return settings.Provider == "MailerSend"
                ? await SendViaMailerSendAsync(settings, to, subject, body, isHtml, ct)
                : await SendViaSmtpAsync(settings, to, subject, body, isHtml, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To} (subject: {Subject})", to, subject);
            return false;
        }
    }

    // ───────────────────────────────────────────────────────────────────────────────────

    private async Task<bool> SendViaMailerSendAsync(
        MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(settings.MailerSendApiKey))
        {
            _logger.LogWarning("Email not sent — MailerSend API key is not configured.");
            return false;
        }

        var payload = new
        {
            from    = new { email = settings.FromAddress, name = settings.FromName ?? "" },
            to      = new[] { new { email = to } },
            subject,
            html    = isHtml ? body : (string?)null,
            text    = isHtml ? (string?)null : body,
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mailersend.com/v1/email")
        {
            Content = JsonContent.Create(payload),
        };
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.MailerSendApiKey);

        var client   = _httpFactory.CreateClient("MailerSend");
        var response = await client.SendAsync(request, ct);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent via MailerSend to {To} (subject: {Subject})", to, subject);
            return true;
        }

        var error = await response.Content.ReadAsStringAsync(ct);
        _logger.LogError("MailerSend returned {Status}: {Error}", (int)response.StatusCode, error);
        return false;
    }

    private async Task<bool> SendViaSmtpAsync(
        MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(settings.SmtpHost))
        {
            _logger.LogWarning("Email not sent — SMTP host is not configured.");
            return false;
        }

        using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
        {
            EnableSsl        = settings.UseSsl,
            DeliveryMethod   = SmtpDeliveryMethod.Network,
        };

        if (!string.IsNullOrWhiteSpace(settings.SmtpUsername))
            client.Credentials = new NetworkCredential(
                settings.SmtpUsername, settings.SmtpPassword ?? string.Empty);

        var from = string.IsNullOrWhiteSpace(settings.FromName)
            ? new MailAddress(settings.FromAddress!)
            : new MailAddress(settings.FromAddress!, settings.FromName);

        using var message = new MailMessage(from, new MailAddress(to))
        {
            Subject    = subject,
            Body       = body,
            IsBodyHtml = isHtml,
        };

        await client.SendMailAsync(message, ct);
        _logger.LogInformation("Email sent via SMTP to {To} (subject: {Subject})", to, subject);
        return true;
    }

    /// <summary>Sends a test email to verify the current email configuration.</summary>
    public Task<bool> SendTestAsync(string to, CancellationToken ct = default)
        => SendAsync(to, "Portfolio — Email test",
            "<p>Your email settings are working correctly.</p>", isHtml: true, ct);
}
