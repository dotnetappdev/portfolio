using Portfolio.Api.Models;

namespace Portfolio.Api.Services;

public interface IEmailService
{
    Task<bool> SendAsync(MailSettings settings, string to, string subject, string body, bool isHtml, CancellationToken ct = default);
}
