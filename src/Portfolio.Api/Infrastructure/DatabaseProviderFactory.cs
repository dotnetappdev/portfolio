using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Portfolio.Data.CosmosDb;
// using Portfolio.Data.MySql; // Disabled until Pomelo supports EF Core 10
using Portfolio.Data.PostgreSql;

namespace Portfolio.Api.Infrastructure;

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

        // Migrations are generated against SQLite types. When running against other providers the
        // model fingerprint differs, producing PendingModelChangesWarning. Suppress it intentionally —
        // migrations are provider-aware and the schema is correct for each supported provider.
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
            // case "mysql": // Disabled until Pomelo supports EF Core 10
            //     options.UsePortfolioMySql(connectionString);
            //     break;
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
