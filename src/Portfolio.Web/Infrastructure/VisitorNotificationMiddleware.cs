using Portfolio.Web.Services;

namespace Portfolio.Web.Infrastructure;

/// <summary>
/// Detects first-time visitors (identified by the absence of a session cookie) and
/// fires an admin notification via Portfolio.Api in the background. The cookie is
/// written with a 24-hour lifetime so repeated visits within a day only generate one notification.
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

    public async Task InvokeAsync(HttpContext context, PortfolioApiService apiService, IConfiguration configuration)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var isPageRequest = !path.StartsWith("/_", StringComparison.Ordinal)
                            && !path.StartsWith("/api/", StringComparison.Ordinal)
                            && !HasStaticFileExtension(path);

        if (isPageRequest && !context.Request.Cookies.ContainsKey(CookieName))
        {
            context.Response.Cookies.Append(CookieName, "1", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                IsEssential = false,
                Secure = context.Request.IsHttps,
            });

            var ip = GetClientIp(context);
            var page = path;
            var time = DateTime.UtcNow.ToString("u");
            var serviceApiKey = configuration["ServiceApiKey"] ?? string.Empty;
            var apiServiceCapture = apiService;
            var loggerCapture = _logger;

            _ = Task.Run(async () =>
            {
                try
                {
                    await apiServiceCapture.NotifyVisitorArrivedAsync(ip, page, time, serviceApiKey);
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
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            var firstIp = forwarded.Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(firstIp))
                return firstIp;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
