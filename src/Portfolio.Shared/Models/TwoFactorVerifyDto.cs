namespace Portfolio.Shared.Models;

/// <summary>Payload for verifying a TOTP code during 2FA setup or login.</summary>
public class TwoFactorVerifyDto
{
    /// <summary>The user ID — only required during the login 2FA challenge flow.</summary>
    public string UserId { get; set; } = string.Empty;
    /// <summary>The six-digit TOTP code from the authenticator app.</summary>
    public string Code { get; set; } = string.Empty;
}
