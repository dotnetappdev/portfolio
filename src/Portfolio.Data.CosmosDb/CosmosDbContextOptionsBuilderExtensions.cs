using Microsoft.EntityFrameworkCore;

namespace Portfolio.Data.CosmosDb;

/// <summary>
/// Extension methods for configuring an EF Core DbContext with the Azure Cosmos DB provider.
/// </summary>
public static class CosmosDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use Azure Cosmos DB.
    /// </summary>
    /// <param name="options">The builder being configured.</param>
    /// <param name="connectionString">
    /// A Cosmos DB connection string containing the following semicolon-separated keys:
    /// <list type="bullet">
    ///   <item><term>AccountEndpoint</term><description>Required. The HTTPS endpoint URI.</description></item>
    ///   <item><term>AccountKey</term><description>Required. The primary or secondary access key.</description></item>
    ///   <item><term>Database</term> (or <term>DatabaseName</term>)<description>Optional. Defaults to "Portfolio".</description></item>
    /// </list>
    /// Example: <c>AccountEndpoint=https://myaccount.documents.azure.com:443/;AccountKey=ABC==;Database=portfolio</c>
    /// </param>
    /// <returns>The same builder so calls can be chained.</returns>
    public static DbContextOptionsBuilder UsePortfolioCosmosDb(
        this DbContextOptionsBuilder options,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        // Parse the required parts out of the Cosmos DB connection string.
        // Expected keys: AccountEndpoint, AccountKey, Database (or DatabaseName)
        var parts = connectionString
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(p => p.Split('=', 2))
            .Where(p => p.Length == 2)
            .ToDictionary(
                p => p[0].Trim(),
                p => p[1].Trim(),
                StringComparer.OrdinalIgnoreCase);

        if (!parts.TryGetValue("AccountEndpoint", out var endpoint))
            throw new InvalidOperationException(
                "Cosmos DB connection string must contain 'AccountEndpoint'.");
        if (!parts.TryGetValue("AccountKey", out var key))
            throw new InvalidOperationException(
                "Cosmos DB connection string must contain 'AccountKey'.");

        parts.TryGetValue("Database", out var database);
        database ??= parts.GetValueOrDefault("DatabaseName") ?? "Portfolio";

        return options.UseCosmos(endpoint, key, database);
    }
}
