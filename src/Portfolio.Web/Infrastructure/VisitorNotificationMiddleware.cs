using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Services;

namespace Portfolio.Web.Infrastructure;

/// <summary>
/// Detects first-time visitors (identified by the absence of a session cookie) and
/// fires an admin notification email in the background. The cookie is written with a
/// 24-hour lifetime so repeated visits within a day only generate one notification.
/// </summary>
public sealed class VisitorNotificationMiddleware
{
    private const string CookieName = "pf_visited";

    private readonly RequestDelegate _next;
    private readonly ILogger<VisitorNotificationMiddleware> _logger;

    public VisitorNotificationMiddleware(RequestDelegate next,
        ILogger<VisitorNotificationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IDbContextFactory<ApplicationDbContext> dbFactory,
        EmailSender emailSender)
    {
        // Only process page requests — ignore static files, _blazor, _framework, API routes, etc.
        var path = context.Request.Path.Value ?? string.Empty;
        var isPageRequest = !path.StartsWith("/_", StringComparison.Ordinal)
                            && !path.StartsWith("/api/", StringComparison.Ordinal)
                            && !HasStaticFileExtension(path);

        if (isPageRequest && !context.Request.Cookies.ContainsKey(CookieName))
        {
            // Set cookie immediately (before response flush)
            context.Response.Cookies.Append(CookieName, "1", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                IsEssential = false,
                Secure = context.Request.IsHttps,
            });

            // Resolve the real client IP — account for reverse proxies
            var ip = GetClientIp(context);
            var page = path;
            var time = DateTime.UtcNow.ToString("u");

            // Fire-and-forget: capture services needed in the background task
            var dbFactoryCapture = dbFactory;
            var emailSenderCapture = emailSender;
            var loggerCapture = _logger;

            _ = Task.Run(async () =>
            {
                try
                {
                    await using var db = await dbFactoryCapture.CreateDbContextAsync();
                    var appSettings = await db.AppSettings.FirstOrDefaultAsync();

                    if (appSettings == null || !appSettings.VisitorNotificationsEnabled)
                        return;

                    var mailSettings = await db.MailSettings.FirstOrDefaultAsync();
                    if (mailSettings == null || !mailSettings.IsEnabled)
                        return;

                    var recipientEmail = string.IsNullOrWhiteSpace(appSettings.VisitorNotificationEmail)
                        ? mailSettings.FromAddress
                        : appSettings.VisitorNotificationEmail;

                    if (string.IsNullOrWhiteSpace(recipientEmail))
                        return;

                    var template = string.IsNullOrWhiteSpace(appSettings.VisitorEmailTemplate)
                        ? DefaultTemplate
                        : appSettings.VisitorEmailTemplate;

                    var body = template
                        .Replace("{{ip}}", ip)
                        .Replace("{{page}}", page)
                        .Replace("{{time}}", time);

                    await emailSenderCapture.SendAsync(recipientEmail,
                        "Portfolio — New visitor arrived", body, isHtml: true);
                }
                catch (Exception ex)
                {
                    loggerCapture.LogError(ex, "Error sending visitor notification");
                }
            });
        }

        await _next(context);
    }

    private static bool HasStaticFileExtension(string path)
    {
        var dot = path.LastIndexOf('.');
        if (dot < 0) return false;
        var ext = path[(dot + 1)..].ToLowerInvariant();
        return ext is "js" or "css" or "png" or "jpg" or "jpeg" or "gif" or "svg" or "ico"
                   or "woff" or "woff2" or "ttf" or "eot" or "map" or "json" or "xml"
                   or "txt" or "pdf" or "zip" or "webp" or "avif";
    }

    private static string GetClientIp(HttpContext context)
    {
        // Respect X-Forwarded-For when behind a reverse proxy
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            // X-Forwarded-For can be a comma-separated list; first entry is the originating client
            var firstIp = forwarded.Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(firstIp))
                return firstIp;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private const string DefaultTemplate =
        "<p><strong>A new visitor has arrived on your portfolio site.</strong></p>" +
        "<ul><li><strong>IP:</strong> {{ip}}</li>" +
        "<li><strong>Page:</strong> {{page}}</li>" +
        "<li><strong>Time (UTC):</strong> {{time}}</li></ul>";
}
