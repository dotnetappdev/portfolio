using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Portfolio.Web.Components;
using Portfolio.Web.Data;
using Portfolio.Web.Services;
using Portfolio.Web.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    DatabaseProviderFactory.ConfigureDbContext(options, builder.Configuration));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
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

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
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
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Portfolio.Web.Client._Imports).Assembly);

// Proper HTTP endpoint for login — Blazor Server circuits cannot set cookies,
// so authentication must go through a standard HTTP POST handler.
app.MapPost("/account/login", async (
    HttpContext httpContext,
    SignInManager<ApplicationUser> signInManager,
    [Microsoft.AspNetCore.Mvc.FromForm] string email,
    [Microsoft.AspNetCore.Mvc.FromForm] string password,
    [Microsoft.AspNetCore.Mvc.FromForm] string? returnUrl) =>
{
    var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);
    if (result.Succeeded)
        return Results.Redirect(returnUrl ?? "/admin");
    if (result.IsLockedOut)
        return Results.Redirect($"/login?error=locked&returnUrl={Uri.EscapeDataString(returnUrl ?? "")}");
    return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "")}");
});

app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
}).RequireAuthorization();

// Seed admin user for web app — controlled by "SeedData": true in appsettings.json
if (builder.Configuration.GetValue<bool>("SeedData"))
{
    using var scope = app.Services.CreateScope();
    var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userMgr  = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr  = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger   = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.EnsureCreatedAsync();

    if (!await roleMgr.RoleExistsAsync("Admin"))
        await roleMgr.CreateAsync(new IdentityRole("Admin"));

    var adminEmail    = builder.Configuration["DefaultAdmin:Email"]    ?? "admin@portfolio.com";
    var adminPassword = builder.Configuration["DefaultAdmin:Password"] ?? "Admin@123456!";

    if (await userMgr.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName       = adminEmail,
            Email          = adminEmail,
            FirstName      = "David",
            LastName       = "Buckley",
            EmailConfirmed = true
        };
        var result = await userMgr.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
            await userMgr.AddToRoleAsync(admin, "Admin");
    }

    logger.LogInformation("Web seed complete. Admin account: {Email}", adminEmail);
}

app.Run();
