using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services;

public class EmailService(IHttpClientFactory httpFactory, ILogger<EmailService> logger) : IEmailService
{
    public async Task<bool> SendAsync(MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(settings.FromAddress))
        {
            logger.LogWarning("Email not sent: From Address is not configured.");
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
            logger.LogError(ex, "Failed to send email to {To} (subject: {Subject})", to, subject);
            return false;
        }
    }

    private async Task<bool> SendViaMailerSendAsync(MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(settings.MailerSendApiKey))
        {
            logger.LogWarning("Email not sent: MailerSend API key is not configured.");
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

        var client   = httpFactory.CreateClient("MailerSend");
        var response = await client.SendAsync(request, ct);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Email sent via MailerSend to {To} (subject: {Subject})", to, subject);
            return true;
        }

        var error = await response.Content.ReadAsStringAsync(ct);
        logger.LogError("MailerSend returned {Status}: {Error}", (int)response.StatusCode, error);
        return false;
    }

    private async Task<bool> SendViaSmtpAsync(MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(settings.SmtpHost))
        {
            logger.LogWarning("Email not sent: SMTP host is not configured.");
            return false;
        }

        using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
        {
            EnableSsl      = settings.UseSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
        };

        if (!string.IsNullOrWhiteSpace(settings.SmtpUsername))
            client.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword ?? string.Empty);

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
        logger.LogInformation("Email sent via SMTP to {To} (subject: {Subject})", to, subject);
        return true;
    }
}
