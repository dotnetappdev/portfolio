using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>
/// Sends emails using the SMTP settings stored in the database.
/// Reads <see cref="MailSettings"/> on every send so admin changes take effect immediately.
/// </summary>
public sealed class EmailSender
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<EmailSender> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Sends an email. Returns <c>true</c> on success, <c>false</c> when mail is disabled or misconfigured.
    /// Never throws — errors are logged.
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

        if (string.IsNullOrWhiteSpace(settings.SmtpHost) ||
            string.IsNullOrWhiteSpace(settings.FromAddress))
        {
            _logger.LogWarning("Email not sent — SMTP host or from-address is not configured.");
            return false;
        }

        try
        {
            using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
            {
                EnableSsl = settings.UseSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            if (!string.IsNullOrWhiteSpace(settings.SmtpUsername))
                client.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword ?? string.Empty);

            var from = string.IsNullOrWhiteSpace(settings.FromName)
                ? new MailAddress(settings.FromAddress)
                : new MailAddress(settings.FromAddress, settings.FromName);

            using var message = new MailMessage(from, new MailAddress(to))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
            };

            await client.SendMailAsync(message, ct);
            _logger.LogInformation("Email sent to {To} (subject: {Subject})", to, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To} (subject: {Subject})", to, subject);
            return false;
        }
    }

    /// <summary>Sends a test email to verify SMTP configuration.</summary>
    public Task<bool> SendTestAsync(string to, CancellationToken ct = default)
        => SendAsync(to, "Portfolio — SMTP test",
            "<p>Your SMTP settings are working correctly.</p>", isHtml: true, ct);
}
