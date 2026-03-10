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

var builder = WebApplication.CreateBuilder(args);

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
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger UI — available in all environments (including production / Azure).
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("BlazorOrigin");
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
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleMgr.RoleExistsAsync("Admin"))
            await roleMgr.CreateAsync(new IdentityRole("Admin"));

        var adminEmail    = builder.Configuration["DefaultAdmin:Email"];
        var adminPassword = builder.Configuration["DefaultAdmin:Password"];
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning(
                "DefaultAdmin:Email / DefaultAdmin:Password not configured. " +
                "Skipping admin seed. Set DefaultAdmin__Email and DefaultAdmin__Password " +
                "environment variables to seed an admin account.");
        }
        else if (await userMgr.FindByEmailAsync(adminEmail) == null)
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

            logger.LogInformation("API seed complete. Admin account: {Email}", adminEmail);
        }

        // Seed initial MailSettings from appsettings if not yet configured in DB.
        if (!await context.MailSettings.AnyAsync())
        {
            var mailSection = builder.Configuration.GetSection("Mail");
            if (mailSection.Exists())
            {
                context.MailSettings.Add(new MailSettings
                {
                    Id               = 1,
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
        var dir  = Path.GetDirectoryName(path) ?? home;
        Directory.CreateDirectory(dir);
        File.WriteAllText(path, ex.ToString());
    }
    catch
    {
        // Swallow secondary exceptions when writing the error file.
    }

    throw;
}
