namespace Portfolio.Shared.Models;

public class SmsSettingsDto
{
    public int Id { get; set; }
    public string Provider { get; set; } = "None";
    public bool IsEnabled { get; set; }
    public string? AdminReceiverNumber { get; set; }
    public string? TwilioAccountSid { get; set; }
    public string? TwilioAuthToken { get; set; }
    public string? TwilioFromNumber { get; set; }
    public string? ClickSendUsername { get; set; }
    public string? ClickSendApiKey { get; set; }
    public string? ClickSendFromName { get; set; }
}
