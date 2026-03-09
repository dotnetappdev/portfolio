using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Portfolio.Data.CosmosDb;
using Portfolio.Data.MySql;
using Portfolio.Data.PostgreSql;

namespace Portfolio.Web.Infrastructure;

/// <summary>
/// Configures the EF Core database provider based on the "DatabaseProvider" appsetting.
/// Supported values: "SqlServer" (default), "Sqlite", "PostgreSql", "MySql", "CosmosDb".
/// </summary>
public static class DatabaseProviderFactory
{
    public static void ConfigureDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "SqlServer";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        // The migration snapshots are generated against SQLite types. When running against
        // other providers the model fingerprint differs, causing PendingModelChangesWarning.
        // Suppress it — migrations are intentionally provider-aware for this project.
        options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

        switch (provider.Trim().ToLowerInvariant())
        {
            case "sqlite":
                options.UseSqlite(connectionString);
                break;
            case "postgresql":
            case "postgres":
                options.UsePortfolioPostgreSql(connectionString);
                break;
            case "mysql":
                options.UsePortfolioMySql(connectionString);
                break;
            case "cosmosdb":
            case "cosmos":
                options.UsePortfolioCosmosDb(connectionString);
                break;
            case "sqlserver":
            default:
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
                break;
        }
    }
}
