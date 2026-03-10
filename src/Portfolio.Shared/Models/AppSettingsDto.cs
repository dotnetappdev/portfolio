namespace Portfolio.Shared.Models;

public class AppSettingsDto
{
    public int Id { get; set; }
    public string? ApiBaseUrl { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public bool VisitorNotificationsEnabled { get; set; }
    public string? VisitorNotificationEmail { get; set; }
    public string? VisitorEmailTemplate { get; set; }
}
