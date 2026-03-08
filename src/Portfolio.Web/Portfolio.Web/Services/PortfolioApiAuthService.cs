using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Portfolio.Shared.Models;
using Portfolio.Web.Data;

namespace Portfolio.Web.Services;

/// <summary>
/// Handles authentication and user management by calling the Portfolio.Api.
/// Used by the web application to avoid duplicating the Identity store.
/// </summary>
public class PortfolioApiAuthService(
    IHttpClientFactory httpClientFactory,
    ApplicationDbContext dbContext,
    ILogger<PortfolioApiAuthService> logger)
{
    // Cached for the lifetime of this scoped service instance (one per request).
    // Thread-safety is not a concern because scoped services are never shared across requests.
    private string? _cachedBaseUrl;

    private async Task<HttpClient> GetClientAsync()
    {
        if (_cachedBaseUrl == null)
        {
            var settings = await dbContext.AppSettings.AsNoTracking().FirstOrDefaultAsync();
            _cachedBaseUrl = settings?.ApiBaseUrl ?? "https://localhost:7002/";
        }

        var client = httpClientFactory.CreateClient("PortfolioApi");
        client.BaseAddress = new Uri(_cachedBaseUrl.TrimEnd('/') + "/");
        return client;
    }

    private HttpClient GetClientWithToken(string baseUrl, string token)
    {
        var client = httpClientFactory.CreateClient("PortfolioApi");
        client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>
    /// Calls the API login endpoint and, on success, returns a <see cref="ClaimsPrincipal"/>
    /// built from the JWT claims so the web app can create a cookie-based session.
    /// Also returns the raw JWT token to store for subsequent admin API calls.
    /// </summary>
    public async Task<(ClaimsPrincipal? Principal, string? Token)> LoginAsync(string email, string password)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.PostAsJsonAsync("api/auth/login",
                new LoginDto { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return (null, null);

            using var doc = await JsonDocument.ParseAsync(
                await response.Content.ReadAsStreamAsync());

            if (!doc.RootElement.TryGetProperty("token", out var tokenElement))
                return (null, null);

            var token = tokenElement.GetString();
            if (string.IsNullOrEmpty(token))
                return (null, null);

            var principal = BuildPrincipalFromJwt(token);
            return (principal, token);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "API login call failed");
            return (null, null);
        }
    }

    /// <summary>Lists all user accounts via the API (requires admin JWT).</summary>
    public async Task<List<UserDto>> GetUsersAsync(string adminToken)
    {
        try
        {
            if (_cachedBaseUrl == null)
            {
                var settings = await dbContext.AppSettings.AsNoTracking().FirstOrDefaultAsync();
                _cachedBaseUrl = settings?.ApiBaseUrl ?? "https://localhost:7002/";
            }
            var client = GetClientWithToken(_cachedBaseUrl, adminToken);
            return await client.GetFromJsonAsync<List<UserDto>>("api/auth/users")
                   ?? [];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch users from API");
            return [];
        }
    }

    /// <summary>Creates a new user account via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> CreateUserAsync(CreateUserDto dto, string adminToken)
    {
        try
        {
            if (_cachedBaseUrl == null)
            {
                var settings = await dbContext.AppSettings.AsNoTracking().FirstOrDefaultAsync();
                _cachedBaseUrl = settings?.ApiBaseUrl ?? "https://localhost:7002/";
            }
            var client = GetClientWithToken(_cachedBaseUrl, adminToken);
            var response = await client.PostAsJsonAsync("api/auth/users", dto);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>
    /// Decodes the JWT payload (without signature validation — we trust our own API)
    /// and returns a <see cref="ClaimsPrincipal"/> for cookie authentication.
    /// </summary>
    private static ClaimsPrincipal BuildPrincipalFromJwt(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
            throw new FormatException("Malformed JWT from API");

        // Base64url → Base64: replace URL-safe chars and add missing padding.
        // JWT uses base64url (RFC 4648 §5): '-' instead of '+', '_' instead of '/',
        // and no '=' padding. We restore standard base64 before decoding.
        var payload = parts[1];
        var padded  = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')
                             .Replace('-', '+').Replace('_', '/');
        var bytes = Convert.FromBase64String(padded);

        using var doc = JsonDocument.Parse(bytes);

        var claims = new List<Claim>();
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            // Map well-known JWT claim names to the long-form ClaimTypes URIs that
            // ASP.NET Core [Authorize(Roles = "Admin")] understands.
            if (prop.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var elem in prop.Value.EnumerateArray())
                    AddClaim(claims, prop.Name, elem.GetString() ?? string.Empty);
            }
            else if (prop.Value.ValueKind == JsonValueKind.String)
            {
                AddClaim(claims, prop.Name, prop.Value.GetString() ?? string.Empty);
            }
        }

        // Ensure the access token itself is stored as a claim so admin components
        // can retrieve it for subsequent authenticated API calls.
        if (!claims.Any(c => c.Type == "access_token"))
            claims.Add(new Claim("access_token", jwt));

        var identity = new ClaimsIdentity(
            claims,
            "Portfolio.Api",  // authenticationType — must be non-null for IsAuthenticated = true
            ClaimTypes.Name,
            ClaimTypes.Role);

        return new ClaimsPrincipal(identity);
    }

    private static void AddClaim(List<Claim> claims, string jwtName, string value)
    {
        // Map JWT short names to ASP.NET Core ClaimTypes URIs
        var type = jwtName switch
        {
            "sub"   => ClaimTypes.NameIdentifier,
            "email" => ClaimTypes.Email,
            "name"  => ClaimTypes.Name,
            // JwtSecurityTokenHandler writes ClaimTypes.Role as the full URI
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
            "role"  => ClaimTypes.Role,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" => ClaimTypes.Email,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"         => ClaimTypes.Name,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" => ClaimTypes.NameIdentifier,
            _       => jwtName
        };
        claims.Add(new Claim(type, value));
    }
}
