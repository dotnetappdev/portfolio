namespace Portfolio.Shared.Models;

/// <summary>Non-sensitive, publicly readable application settings.</summary>
public class PublicAppSettingsDto
{
    /// <summary>Google Analytics 4 Measurement ID (e.g. G-XXXXXXXXXX). Null when not configured.</summary>
    public string? GoogleAnalyticsId { get; set; }

    /// <summary>Primary brand colour (CSS hex). Null to use the default palette colour.</summary>
    public string? PrimaryColor { get; set; }

    /// <summary>Secondary brand colour (CSS hex). Null to use the default.</summary>
    public string? SecondaryColor { get; set; }

    /// <summary>Tertiary/accent colour (CSS hex). Null to use the default.</summary>
    public string? TertiaryColor { get; set; }
}
