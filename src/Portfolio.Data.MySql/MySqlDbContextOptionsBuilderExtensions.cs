using Microsoft.EntityFrameworkCore;

namespace Portfolio.Data.MySql;

/// <summary>
/// Extension methods for configuring an EF Core DbContext with the MySQL provider
/// via <see href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo</see>.
/// </summary>
public static class MySqlDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use MySQL with
    /// automatic server-version detection.
    /// </summary>
    /// <param name="options">The builder being configured.</param>
    /// <param name="connectionString">
    /// A MySQL connection string.
    /// </param>
    /// <remarks>
    /// Uses <see cref="ServerVersion.AutoDetect"/> to detect the MySQL/MariaDB server
    /// version at configuration time (once during app startup, not per request).
    /// For high-throughput scenarios or large deployments, consider passing a fixed
    /// <see cref="ServerVersion"/> to avoid a round-trip on startup.
    /// </remarks>
    /// <returns>The same builder so calls can be chained.</returns>
    public static DbContextOptionsBuilder UsePortfolioMySql(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
