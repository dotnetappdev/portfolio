using Microsoft.EntityFrameworkCore;
using Portfolio.Data.CosmosDb;
using Portfolio.Data.MySql;
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
                options.UseSqlServer(connectionString);
                break;
        }
    }
}
