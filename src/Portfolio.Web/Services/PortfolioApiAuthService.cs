using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Portfolio.Shared.Models;

namespace Portfolio.Web.Services;

/// <summary>
/// Handles authentication and user management by calling the Portfolio.Api.
/// Used by the web application to avoid duplicating the Identity store.
/// </summary>
public class PortfolioApiAuthService(
    IHttpClientFactory httpClientFactory,
    ILogger<PortfolioApiAuthService> logger)
{
    private HttpClient GetClient() => httpClientFactory.CreateClient("PortfolioApi");

    private HttpClient GetClientWithToken(string token)
    {
        var client = GetClient();
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
            var client = GetClient();
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

    /// <summary>Lists all user accounts with roles and claims via the API (requires admin JWT).</summary>
    public async Task<List<UserWithRolesDto>> GetUsersAsync(string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            return await client.GetFromJsonAsync<List<UserWithRolesDto>>("api/auth/users")
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
            var client = GetClientWithToken(adminToken);
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

    /// <summary>Updates an existing user account via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> UpdateUserAsync(string userId, UpdateUserDto dto, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.PutAsJsonAsync($"api/auth/users/{userId}", dto);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Deletes a user account via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> DeleteUserAsync(string userId, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.DeleteAsync($"api/auth/users/{userId}");
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Assigns a role to a user via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> AddUserRoleAsync(string userId, string role, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.PostAsJsonAsync($"api/auth/users/{userId}/roles", role);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to add role to user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Removes a role from a user via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> RemoveUserRoleAsync(string userId, string role, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.DeleteAsync($"api/auth/users/{userId}/roles/{Uri.EscapeDataString(role)}");
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to remove role from user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Lists all available roles via the API (requires admin JWT).</summary>
    public async Task<List<RoleDto>> GetRolesAsync(string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            return await client.GetFromJsonAsync<List<RoleDto>>("api/roles") ?? [];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch roles from API");
            return [];
        }
    }

    /// <summary>Creates a new role via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> CreateRoleAsync(string roleName, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.PostAsJsonAsync("api/roles", roleName);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create role via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Deletes a role via the API (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> DeleteRoleAsync(string roleName, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.DeleteAsync($"api/roles/{Uri.EscapeDataString(roleName)}");
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete role via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Sends a password-reset email via the API (no auth required).</summary>
    public async Task<(bool Success, string? Error)> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        try
        {
            var client   = GetClient();
            var response = await client.PostAsJsonAsync("api/auth/forgot-password", dto);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Forgot-password API call failed");
            return (false, ex.Message);
        }
    }

    /// <summary>Resets a user's password using the Identity token from the reset email.</summary>
    public async Task<(bool Success, string? Error)> ResetPasswordAsync(ResetPasswordDto dto)
    {
        try
        {
            var client   = GetClient();
            var response = await client.PostAsJsonAsync("api/auth/reset-password", dto);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            // Try to parse a friendly message from the JSON body
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                    return (false, msg.GetString());
            }
            catch { }
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Reset-password API call failed");
            return (false, ex.Message);
        }
    }

    /// <summary>Registers a new user account via the public API endpoint.</summary>
    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterDto dto)
    {
        try
        {
            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/auth/register", dto);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to register user via API");
            return (false, ex.Message);
        }
    }

    /// <summary>Gets 2FA setup details (shared key and OTP URI) for a user (requires admin JWT).</summary>
    public async Task<(string? SharedKey, string? AuthenticatorUri, string? Error)> GetTwoFactorSetupAsync(string userId, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            using var response = await client.GetAsync($"api/auth/users/{userId}/2fa/setup");
            if (!response.IsSuccessStatusCode)
                return (null, null, await response.Content.ReadAsStringAsync());

            using var doc = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStreamAsync());
            var sharedKey = doc.RootElement.GetProperty("sharedKey").GetString();
            var uri = doc.RootElement.GetProperty("authenticatorUri").GetString();
            return (sharedKey, uri, null);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get 2FA setup from API");
            return (null, null, ex.Message);
        }
    }

    /// <summary>Verifies a TOTP code and enables 2FA for a user (requires admin JWT).</summary>
    public async Task<(bool Success, IReadOnlyList<string> RecoveryCodes, string? Error)> EnableTwoFactorAsync(string userId, string code, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.PostAsJsonAsync($"api/auth/users/{userId}/2fa/enable",
                new TwoFactorVerifyDto { UserId = userId, Code = code });
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                var codes = json.TryGetProperty("recoveryCodes", out var arr)
                    ? arr.EnumerateArray().Select(e => e.GetString()).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!).ToList()
                    : (IReadOnlyList<string>)[];
                return (true, codes, null);
            }

            var body = await response.Content.ReadAsStringAsync();
            return (false, [], body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to enable 2FA via API");
            return (false, [], ex.Message);
        }
    }

    /// <summary>Disables 2FA for a user (requires admin JWT).</summary>
    public async Task<(bool Success, string? Error)> DisableTwoFactorAsync(string userId, string adminToken)
    {
        try
        {
            var client = GetClientWithToken(adminToken);
            var response = await client.PostAsync($"api/auth/users/{userId}/2fa/disable", null);
            if (response.IsSuccessStatusCode) return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to disable 2FA via API");
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
