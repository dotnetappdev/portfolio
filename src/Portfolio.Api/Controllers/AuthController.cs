using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Api.Services;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ApplicationDbContext context,
        IEmailService emailService)
    {
        _userManager  = userManager;
        _configuration = configuration;
        _context      = context;
        _emailService = emailService;
    }

    // ── Login ────────────────────────────────────────────────────────────────

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized(new { message = "Invalid credentials" });

        if (await _userManager.IsLockedOutAsync(user))
            return Unauthorized(new { message = "Account locked" });

        // Reset failed access count on successful login.
        await _userManager.ResetAccessFailedCountAsync(user);

        // Check if 2FA is required.
        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            // Return a partial response so the client can prompt for a TOTP code.
            return Ok(new { twoFactorRequired = true, userId = user.Id });
        }

        var token = await GenerateJwtTokenAsync(user);
        return Ok(new { token, email = user.Email, twoFactorRequired = false });
    }

    // ── 2FA ─────────────────────────────────────────────────────────────────

    /// <summary>Verify a TOTP code after the initial 2FA challenge and return a JWT on success.</summary>
    [HttpPost("login/2fa")]
    public async Task<IActionResult> LoginWith2Fa([FromBody] TwoFactorVerifyDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null) return Unauthorized(new { message = "Invalid request" });

        var code = dto.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
        var isValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);

        if (!isValid)
            return Unauthorized(new { message = "Invalid authenticator code" });

        var token = await GenerateJwtTokenAsync(user);
        return Ok(new { token, email = user.Email, twoFactorRequired = false });
    }

    /// <summary>Generate a TOTP setup key and QR URI for the authenticated user.</summary>
    [HttpGet("users/{id}/2fa/setup")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTwoFactorSetup(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var key = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var email = user.Email ?? user.UserName ?? id;
        var issuer = _configuration["Jwt:Issuer"] ?? "Portfolio";
        var uri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}?secret={key}&issuer={Uri.EscapeDataString(issuer)}&digits=6";

        return Ok(new { sharedKey = key, authenticatorUri = uri });
    }

    /// <summary>Verify a TOTP code and enable 2FA for the given user.</summary>
    [HttpPost("users/{id}/2fa/enable")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EnableTwoFactor(string id, [FromBody] TwoFactorVerifyDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var code = dto.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
        var isValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);

        if (!isValid)
            return BadRequest(new { message = "Invalid verification code" });

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        return Ok(new { message = "Two-factor authentication enabled", recoveryCodes });
    }

    /// <summary>Disable 2FA for the given user.</summary>
    [HttpPost("users/{id}/2fa/disable")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DisableTwoFactor(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        return Ok(new { message = "Two-factor authentication disabled" });
    }

    // ── Registration ─────────────────────────────────────────────────────────

    /// <summary>Register a new user account (public endpoint — no auth required).</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            return BadRequest(new { message = "Passwords do not match" });

        var user = new ApplicationUser
        {
            UserName       = dto.Email,
            Email          = dto.Email,
            FirstName      = dto.FirstName,
            LastName       = dto.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "Registration successful", email = user.Email });
    }

    // ── Forgot / Reset Password ──────────────────────────────────────────────

    /// <summary>
    /// Generates an Identity password-reset token and emails a link to the user.
    /// Always returns 200 to avoid leaking whether the address is registered.
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user != null)
        {
            var token    = await _userManager.GeneratePasswordResetTokenAsync(user);
            var base64   = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            var baseUrl  = dto.CallbackBaseUrl.TrimEnd('/');
            var resetUrl = $"{baseUrl}/reset-password?email={Uri.EscapeDataString(dto.Email)}&token={Uri.EscapeDataString(base64)}";

            var mailSettings = await _context.MailSettings.FirstOrDefaultAsync();
            if (mailSettings != null && mailSettings.IsEnabled)
            {
                var subject = "Reset your password";
                var body    = $"""
                    <p>Hi,</p>
                    <p>You requested a password reset for your portfolio admin account.</p>
                    <p><a href="{resetUrl}">Click here to reset your password</a></p>
                    <p>This link expires after 1 hour. If you did not request this, you can safely ignore this email.</p>
                    """;
                await _emailService.SendAsync(mailSettings, dto.Email, subject, body, isHtml: true);
            }
        }

        return Ok(new { message = "If that email is registered you will receive a reset link shortly." });
    }

    /// <summary>Validates the Identity reset token and sets the new password.</summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
            return BadRequest(new { message = "Passwords do not match." });

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return BadRequest(new { message = "Invalid request." });

        string token;
        try
        {
            token = Encoding.UTF8.GetString(Convert.FromBase64String(dto.Token));
        }
        catch
        {
            return BadRequest(new { message = "Invalid or malformed token." });
        }

        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return BadRequest(new { message = errors });
        }

        return Ok(new { message = "Password has been reset successfully." });
    }

    // ── User CRUD ────────────────────────────────────────────────────────────

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var result = new List<UserWithRolesDto>();
        foreach (var u in users)
        {
            var roles  = await _userManager.GetRolesAsync(u);
            var claims = await _userManager.GetClaimsAsync(u);
            result.Add(new UserWithRolesDto
            {
                Id               = u.Id,
                Email            = u.Email,
                FirstName        = u.FirstName,
                LastName         = u.LastName,
                Roles            = [.. roles],
                Claims           = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }).ToList(),
                TwoFactorEnabled = u.TwoFactorEnabled
            });
        }

        return Ok(result);
    }

    [HttpPost("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var user = new ApplicationUser
        {
            UserName       = dto.Email,
            Email          = dto.Email,
            FirstName      = dto.FirstName,
            LastName       = dto.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User created successfully", email = user.Email });
    }

    [HttpPut("users/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            user.Email    = dto.Email;
            user.UserName = dto.Email;
        }

        if (dto.FirstName != null) user.FirstName = dto.FirstName;
        if (dto.LastName  != null) user.LastName  = dto.LastName;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var pwResult = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!pwResult.Succeeded) return BadRequest(pwResult.Errors);
        }

        return Ok(new { message = "User updated successfully" });
    }

    [HttpDelete("users/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = "User deleted successfully" });
    }

    // ── User Roles ───────────────────────────────────────────────────────────

    [HttpGet("users/{id}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [HttpPost("users/{id}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserRole(string id, [FromBody] string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = $"Role '{role}' added to user" });
    }

    [HttpDelete("users/{id}/roles/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveUserRole(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = $"Role '{role}' removed from user" });
    }

    // ── User Claims ──────────────────────────────────────────────────────────

    [HttpGet("users/{id}/claims")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserClaims(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var claims = await _userManager.GetClaimsAsync(user);
        return Ok(claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }));
    }

    [HttpPost("users/{id}/claims")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserClaim(string id, [FromBody] ClaimDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.AddClaimAsync(user, new Claim(dto.Type, dto.Value));
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = "Claim added" });
    }

    [HttpDelete("users/{id}/claims")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveUserClaim(string id, [FromBody] ClaimDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.RemoveClaimAsync(user, new Claim(dto.Type, dto.Value));
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new { message = "Claim removed" });
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            // Email is used as the unique display name (matches UserName in Identity).
            new(ClaimTypes.Name,           user.Email!),
            new(ClaimTypes.Email,          user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Include role claims so the Blazor Web app can enforce [Authorize(Roles = "Admin")].
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
