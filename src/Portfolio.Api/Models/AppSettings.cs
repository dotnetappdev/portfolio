using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>Admin-configurable general application settings (single row, Id = 1).</summary>
public class AppSettings
{
    public int Id { get; set; }

    /// <summary>Base URL of the Portfolio API used by PortfolioApiService (e.g. https://api.example.com/).</summary>
    [MaxLength(500)]
    public string? ApiBaseUrl { get; set; }

    /// <summary>Google Analytics 4 Measurement ID (e.g. G-XXXXXXXXXX). Leave empty to disable GA.</summary>
    [MaxLength(50)]
    public string? GoogleAnalyticsId { get; set; }

    // ── Visitor Notifications ─────────────────────────────────────────────────
    /// <summary>Send an email to the admin when a new visitor lands on the site.</summary>
    public bool VisitorNotificationsEnabled { get; set; }

    /// <summary>Override email address for visitor notifications; falls back to MailSettings.FromAddress when empty.</summary>
    [MaxLength(200)]
    public string? VisitorNotificationEmail { get; set; }

    /// <summary>Template for the visitor notification email body. Supports {{ip}}, {{page}}, {{time}} placeholders.</summary>
    public string? VisitorEmailTemplate { get; set; }

    // ── Media / Blog Post Slots ───────────────────────────────────────────────
    /// <summary>Number of image slots available in the blog post editor (1 hero + additional).
    /// Tenants can adjust this in admin settings. Default is 10.</summary>
    public int BlogPostImageSlots { get; set; } = 10;

    // ── Site Theme ────────────────────────────────────────────────────────────
    /// <summary>Primary brand colour (CSS hex, e.g. #5B5BD6). Defaults to indigo when null.</summary>
    [MaxLength(20)]
    public string? PrimaryColor { get; set; }

    /// <summary>Secondary brand colour (CSS hex). Defaults to sky-blue when null.</summary>
    [MaxLength(20)]
    public string? SecondaryColor { get; set; }

    /// <summary>Tertiary/accent colour (CSS hex). Defaults to emerald when null.</summary>
    [MaxLength(20)]
    public string? TertiaryColor { get; set; }
}
