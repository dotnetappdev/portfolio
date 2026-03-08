using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Api.Data;
using Portfolio.Api.Infrastructure;
using Portfolio.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("BlazorOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Apply pending EF Core migrations and seed the database on startup.
// Controlled by "SeedData": true in appsettings.json.
if (builder.Configuration.GetValue<bool>("SeedData"))
{
    using var scope = app.Services.CreateScope();
    var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userMgr  = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr  = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger   = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();

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

    logger.LogInformation("API seed complete. Admin account: {Email}", adminEmail);
}

app.Run();
