namespace Portfolio.Shared.Models;

public class AppSettingsDto
{
    public int Id { get; set; }
    public string? ApiBaseUrl { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public bool VisitorNotificationsEnabled { get; set; }
    public string? VisitorNotificationEmail { get; set; }
    public string? VisitorEmailTemplate { get; set; }

    // Theme
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? TertiaryColor { get; set; }

    // Media / Blog Post Slots
    public int BlogPostImageSlots { get; set; } = 10;
}
