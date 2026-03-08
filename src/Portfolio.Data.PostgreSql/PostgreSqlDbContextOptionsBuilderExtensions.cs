using Microsoft.EntityFrameworkCore;

namespace Portfolio.Data.PostgreSql;

/// <summary>
/// Extension methods for configuring an EF Core DbContext with the PostgreSQL provider
/// via <see href="https://www.npgsql.org/efcore/">Npgsql</see>.
/// </summary>
public static class PostgreSqlDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use PostgreSQL.
    /// </summary>
    /// <param name="options">The builder being configured.</param>
    /// <param name="connectionString">A PostgreSQL connection string.</param>
    /// <returns>The same builder so calls can be chained.</returns>
    public static DbContextOptionsBuilder UsePortfolioPostgreSql(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return options.UseNpgsql(connectionString);
    }
}
