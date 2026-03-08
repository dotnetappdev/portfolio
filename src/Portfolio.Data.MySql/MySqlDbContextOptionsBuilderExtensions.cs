using Microsoft.EntityFrameworkCore;

namespace Portfolio.Data.MySql;

/// <summary>
/// Extension methods for configuring an EF Core DbContext with the MySQL provider
/// via <see href="https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html">MySql.EntityFrameworkCore</see>.
/// </summary>
public static class MySqlDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use MySQL.
    /// </summary>
    /// <param name="options">The builder being configured.</param>
    /// <param name="connectionString">
    /// A MySQL connection string.
    /// </param>
    /// <returns>The same builder so calls can be chained.</returns>
    public static DbContextOptionsBuilder UsePortfolioMySql(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return options.UseMySQL(connectionString);
    }
}
