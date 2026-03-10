namespace Portfolio.Shared.Models;

public class MailSettingsDto
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public string Provider { get; set; } = "Smtp";
    public string? FromAddress { get; set; }
    public string? FromName { get; set; }
    public string? MailerSendApiKey { get; set; }
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public bool UseSsl { get; set; } = true;
}
