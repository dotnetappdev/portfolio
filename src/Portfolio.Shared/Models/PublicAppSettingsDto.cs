namespace Portfolio.Shared.Models;

/// <summary>Non-sensitive, publicly readable application settings.</summary>
public class PublicAppSettingsDto
{
    /// <summary>Google Analytics 4 Measurement ID (e.g. G-XXXXXXXXXX). Null when not configured.</summary>
    public string? GoogleAnalyticsId { get; set; }
}
