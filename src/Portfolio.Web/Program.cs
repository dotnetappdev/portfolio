using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using Portfolio.Web.Components;
using Portfolio.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(o =>
{
    o.Dsn = builder.Configuration["Sentry:Dsn"] ?? string.Empty;
    o.TracesSampleRate = builder.Configuration.GetValue<double?>("Sentry:TracesSampleRate") ?? 0.2;
    o.MinimumBreadcrumbLevel = LogLevel.Information;
    o.MinimumEventLevel      = LogLevel.Warning;
    o.SendDefaultPii         = false;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Cookie-based authentication — credentials are validated against Portfolio.Api (JWT).
// This keeps a single Identity store (in the API) and avoids duplicating user tables.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// Named HTTP client for the Portfolio API.
// BaseApiUrl from appsettings sets the deployment-time base address.
builder.Services.AddHttpClient("PortfolioApi", client =>
{
    var baseApiUrl = builder.Configuration["BaseApiUrl"];
    if (!string.IsNullOrWhiteSpace(baseApiUrl))
        client.BaseAddress = new Uri(baseApiUrl.TrimEnd('/') + "/");
});

builder.Services.AddScoped<SmsSender>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<Portfolio.Web.Services.BlogService>();
builder.Services.AddScoped<Portfolio.Web.Services.CmsPageService>();
builder.Services.AddScoped<Portfolio.Web.Services.MenuService>();
builder.Services.AddScoped<Portfolio.Web.Services.AppSettingsService>();
builder.Services.AddScoped<Portfolio.Web.Services.PortfolioApiService>();
builder.Services.AddScoped<Portfolio.Web.Services.PortfolioApiAuthService>();
builder.Services.AddScoped<Portfolio.Web.Services.StaticSiteGeneratorService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Portfolio.Web.Infrastructure.VisitorNotificationMiddleware>();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// ── Authentication endpoints ─────────────────────────────────────────────────
// Login: validates credentials via Portfolio.Api and sets a cookie-based session.
// The cookie stores the user's claims (including roles and the API JWT) so that
// admin Blazor components can make authenticated API calls without re-logging in.
app.MapPost("/account/login", async (
    HttpContext httpContext,
    Portfolio.Web.Services.PortfolioApiAuthService authService,
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string password,
    [Microsoft.AspNetCore.Mvc.FromForm] string? returnUrl) =>
{
    var (principal, _) = await authService.LoginAsync(email, password);

    if (principal == null)
        return Results.Redirect(
            $"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "")}");

    var authProps = new AuthenticationProperties
    {
        IsPersistent = false,
        ExpiresUtc   = DateTimeOffset.UtcNow.AddHours(8)
    };

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

    return Results.Redirect(returnUrl ?? "/admin");
});

app.MapPost("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
}).RequireAuthorization();

// Static site generator — auth-protected download endpoint.
app.MapGet("/admin/generate-static-site", async (
    StaticSiteGeneratorService generator,
    HttpContext ctx) =>
{
    var bytes = await generator.GenerateAsync();
    var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmm");
    ctx.Response.ContentType = "application/zip";
    ctx.Response.Headers.ContentDisposition = $"attachment; filename=\"portfolio-static-{timestamp}.zip\"";
    await ctx.Response.Body.WriteAsync(bytes);
}).RequireAuthorization();

app.Run();
