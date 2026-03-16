using System.Text;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Portfolio.Api.Data;
using Portfolio.Api.Infrastructure;
using Portfolio.Api.Models;
using Portfolio.Api.Services;
using Microsoft.OpenApi;
using Sentry;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// If a platform-provided port or URL is given (e.g. `PORT` or `ASPNETCORE_URLS`),
// apply it so we don't accidentally bind to the default development port (5000)
// in production environments where the platform controls the listening address.
// Check common environment variables provided by hosting platforms (App Service, containers, etc.).
var platformPortOrUrls = Environment.GetEnvironmentVariable("PORT")
                      ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
                      ?? Environment.GetEnvironmentVariable("ASPNETCORE_PORT")
                      ?? Environment.GetEnvironmentVariable("WEBSITE_PORT");
if (!string.IsNullOrWhiteSpace(platformPortOrUrls))
{
    // If PORT contains a bare number, map it to a wildcard URL; otherwise pass
    // the value through so callers can set full URLs (e.g. "http://*:80").
    if (int.TryParse(platformPortOrUrls, out var numericPort))
        builder.WebHost.UseUrls($"http://0.0.0.0:{numericPort}");
    else
        builder.WebHost.UseUrls(platformPortOrUrls);
}
builder.WebHost.UseSentry(o =>
{
    o.Dsn = builder.Configuration["Sentry:Dsn"] ?? string.Empty;
    o.TracesSampleRate = builder.Configuration.GetValue<double?>("Sentry:TracesSampleRate") ?? 0.2;
    o.MinimumBreadcrumbLevel = LogLevel.Information;
    o.MinimumEventLevel      = LogLevel.Warning;
    o.SendDefaultPii         = false;
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IEmailService, EmailService>();

// Swashbuckle Swagger — active in all environments so the UI is available after deployment.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Portfolio API",
        Version     = "v1",
        Description = "REST API for the Portfolio application."
    });

    // Add JWT bearer security definition so the Swagger UI can send authenticated requests.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token. Example: Bearer {token}"
    });

    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", null, null),
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    DatabaseProviderFactory.ConfigureDbContext(options, builder.Configuration));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException(
        "JWT Key (Jwt:Key) is not configured. " +
        "Set it via environment variable, user secrets, or appsettings.json.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorOrigin", policy =>
    {
        // Support a comma- or semicolon-separated list in configuration and
        // allow matching by domain suffix (e.g. ".dotnetappdevni.com" or "*.dotnetappdevni.com").
        var allowed = builder.Configuration["AllowedOrigins"] ?? "https://localhost:7001";
        var origins = allowed.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(s => s.Trim())
                             .ToArray();

        policy.SetIsOriginAllowed(originString =>
        {
            if (string.IsNullOrWhiteSpace(originString))
                return false;

            try
            {
                var originUri = new Uri(originString);
                var originHost = originUri.Host;

                foreach (var a in origins)
                {
                    if (string.IsNullOrWhiteSpace(a))
                        continue;

                    // Domain suffix or wildcard entry: ".example.com" or "*.example.com"
                    if (a.StartsWith(".") || a.StartsWith("*."))
                    {
                        var domain = a.TrimStart('*').TrimStart('.');
                        if (originHost.Equals(domain, StringComparison.OrdinalIgnoreCase) ||
                            originHost.EndsWith("." + domain, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                    else
                    {
                        // Exact origin match (including scheme/port)
                        if (string.Equals(originString, a, StringComparison.OrdinalIgnoreCase))
                            return true;

                        // If configured value includes a scheme, compare hosts too
                        try
                        {
                            var aUri = new Uri(a);
                            if (string.Equals(aUri.Host, originHost, StringComparison.OrdinalIgnoreCase))
                                return true;
                        }
                        catch
                        {
                            // If 'a' is a bare host like "dotnetappdevni.com", compare host strings
                            var trimmed = a.Replace("https://", "").Replace("http://", "").Split('/')[0];
                            if (trimmed.Equals(originHost, StringComparison.OrdinalIgnoreCase))
                                return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Temporary debug middleware: when `Debug:ShowExceptionSecret` is set, sending
// that secret either as the `X-Debug-Show-Exception` header or as the
// `showException` query value will return the full exception text in the
// response body. In Development the full details are shown automatically.
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception for request {Method} {Path}", context.Request.Method, context.Request.Path);

        var secret = builder.Configuration["Debug:ShowExceptionSecret"];
        var show = false;
        if (!string.IsNullOrWhiteSpace(secret))
        {
            if (context.Request.Headers.TryGetValue("X-Debug-Show-Exception", out var hv) && hv == secret)
                show = true;
            if (context.Request.Query.TryGetValue("showException", out var qv) && qv == secret)
                show = true;
        }
        else if (builder.Environment.IsDevelopment())
        {
            show = true;
        }

        if (show)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(ex.ToString());
            return;
        }

        // Not allowed to show details — rethrow so the standard handler runs.
        throw;
    }
});

// Swagger UI — available in all environments (including production / Azure).
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("BlazorOrigin");

// Serve uploaded media files from wwwroot/uploads/
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Apply pending EF Core migrations on every startup — keeps dev and production
// databases in sync automatically without manual dotnet ef database update.
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger  = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    // SQLite migrations are generated from SQL Server DDL; use EnsureCreated for
    // SQLite so the schema is built directly from the current model instead.
    var providerName = context.Database.ProviderName ?? "";
    if (providerName.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
        await context.Database.EnsureCreatedAsync();
    else
        await context.Database.MigrateAsync();
    logger.LogInformation("Database migrations applied successfully.");

    if (builder.Configuration.GetValue<bool>("SeedData"))
    {
        // Upsert blog posts from BlogPostSeedData — insert new, update content of existing.
        // Look up by Slug (not Id) so we never attempt to INSERT an explicit value into
        // the IDENTITY column, which SQL Server rejects when IDENTITY_INSERT is OFF.
        var seedPosts = BlogPostSeedData.GetAll();
        foreach (var seed in seedPosts)
        {
            var existing = await context.BlogPosts.FirstOrDefaultAsync(b => b.Slug == seed.Slug);
            if (existing is null)
            {
                seed.Id = 0; // let SQL Server assign the identity value
                context.BlogPosts.Add(seed);
            }
            else
            {
                existing.Title           = seed.Title;
                existing.Slug            = seed.Slug;
                existing.Summary         = seed.Summary;
                existing.Category        = seed.Category;
                existing.Tags            = seed.Tags;
                existing.Body            = seed.Body;
                existing.FeaturedImage   = seed.FeaturedImage;
                existing.MetaTitle       = seed.MetaTitle;
                existing.MetaDescription = seed.MetaDescription;
                existing.ReadMinutes     = seed.ReadMinutes;
                existing.PublishedDate   = seed.PublishedDate;
                // IsPublished is intentionally NOT overwritten so CMS publish/unpublish
                // actions made via the admin portal are preserved across restarts.
            }
        }
        await context.SaveChangesAsync();
        logger.LogInformation("Blog post seed complete. {Count} posts processed.", seedPosts.Count);

        // Seed initial MailSettings from appsettings if not yet configured in DB.
        if (!await context.MailSettings.AnyAsync())
        {
            var mailSection = builder.Configuration.GetSection("Mail");
            if (mailSection.Exists())
            {
                context.MailSettings.Add(new MailSettings
                {
                    IsEnabled        = mailSection.GetValue<bool>("IsEnabled"),
                    Provider         = mailSection["Provider"]         ?? "Smtp",
                    FromAddress      = mailSection["FromAddress"],
                    FromName         = mailSection["FromName"],
                    SmtpHost         = mailSection["SmtpHost"],
                    SmtpPort         = mailSection.GetValue<int?>("SmtpPort") ?? 587,
                    SmtpUsername     = mailSection["SmtpUsername"],
                    SmtpPassword     = mailSection["SmtpPassword"],
                    UseSsl           = mailSection.GetValue<bool?>("UseSsl") ?? true,
                    MailerSendApiKey = mailSection["MailerSendApiKey"],
                });
                await context.SaveChangesAsync();
                logger.LogInformation("MailSettings seeded from appsettings.");
            }
        }
    }

    app.Run();
}
catch (Exception ex)
{
    try
    {
        var home = Environment.GetEnvironmentVariable("HOME") ?? AppContext.BaseDirectory;
        var path = Path.Combine(home, "site", "wwwroot", "startup-error.txt");
        var dir = Path.GetDirectoryName(path) ?? home;
        Directory.CreateDirectory(dir);
        File.WriteAllText(path, ex.ToString());
    }



    catch (Exception writeEx)
    {
        // Try to send the exception to Sentry if a DSN is configured. This helps
        // capture fatal startup errors (like port binding failures) in the remote
        // Sentry project so you can inspect them even if stdout logs are missing.
        try
        {
            var dsn = builder.Configuration["Sentry:Dsn"];
            if (!string.IsNullOrWhiteSpace(dsn))
            {
                // Initialize a short-lived Sentry SDK instance if it's not already
                // initialized by UseSentry earlier in the host building process.
                using var _ = SentrySdk.Init(o => { o.Dsn = dsn; o.SendDefaultPii = false; });
                SentrySdk.CaptureException(ex);
                // Give Sentry a moment to send the event before the process exits.
                SentrySdk.FlushAsync(TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
            }
        }
        catch
        {
            // Ignore Sentry failures — we still want to write the local startup file.
        }

    }
    }
