namespace Portfolio.Shared.Models;

public class ForgotPasswordDto
{
    public string Email { get; set; } = "";
    /// <summary>Base URL of the Blazor web app (e.g. https://mysite.com/).
    /// The API appends reset-password?email=...&amp;token=... to build the callback link.</summary>
    public string CallbackBaseUrl { get; set; } = "";
}
