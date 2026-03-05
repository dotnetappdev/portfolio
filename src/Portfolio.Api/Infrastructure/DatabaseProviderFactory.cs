using Microsoft.EntityFrameworkCore;

namespace Portfolio.Api.Infrastructure;

/// <summary>
/// Configures the EF Core database provider based on the "DatabaseProvider" appsetting.
/// Supported values: "SqlServer" (default), "PostgreSql", "Sqlite".
/// </summary>
public static class DatabaseProviderFactory
{
    public static void ConfigureDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "SqlServer";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        switch (provider.Trim().ToLowerInvariant())
        {
            case "sqlite":
                options.UseSqlite(connectionString);
                break;
            case "postgresql":
            case "postgres":
                // Npgsql provider — add reference to Npgsql.EntityFrameworkCore.PostgreSQL
                // and uncomment: options.UseNpgsql(connectionString);
                throw new NotSupportedException(
                    "PostgreSQL provider requires the Npgsql.EntityFrameworkCore.PostgreSQL NuGet package. " +
                    "See README for setup instructions.");
            case "sqlserver":
            default:
                options.UseSqlServer(connectionString);
                break;
        }
    }
}
