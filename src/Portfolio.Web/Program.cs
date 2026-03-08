using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Portfolio.Web.Components;
using Portfolio.Web.Data;
using Portfolio.Web.Services;
using Portfolio.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    DatabaseProviderFactory.ConfigureDbContext(options, builder.Configuration));

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

// Named HTTP client for the Portfolio API — base URL is stored in AppSettings (DB)
builder.Services.AddHttpClient("PortfolioApi");

// Named HTTP clients for SMS providers
builder.Services.AddHttpClient("Twilio");
builder.Services.AddHttpClient("ClickSend");

// SmsSender reads provider settings from the DB on each call — no restart needed
builder.Services.AddScoped<SmsSender>();
builder.Services.AddScoped<Portfolio.Web.Services.BlogService>();
builder.Services.AddScoped<Portfolio.Web.Services.CmsPageService>();
builder.Services.AddScoped<Portfolio.Web.Services.MenuService>();
builder.Services.AddScoped<Portfolio.Web.Services.AppSettingsService>();
builder.Services.AddScoped<Portfolio.Web.Services.PortfolioApiService>();
builder.Services.AddScoped<Portfolio.Web.Services.PortfolioApiAuthService>();
builder.Services.AddScoped<Portfolio.Web.Services.StaticSiteGeneratorService>();
builder.Services.AddScoped<Portfolio.Web.Services.ProjectService>();

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

// Apply pending EF Core migrations and initialise the CMS database on first run.
// Users and roles live in Portfolio.Api — only CMS / portfolio data is seeded here.
if (builder.Configuration.GetValue<bool>("SeedData"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger  = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();
    logger.LogInformation("Web CMS database migrated and initialised.");
}

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
