namespace Portfolio.Sms.Twilio;

/// <summary>Configuration options for the Twilio SMS provider.</summary>
public sealed class TwilioOptions
{
    /// <summary>Twilio Account SID — found on the Twilio Console dashboard.</summary>
    public string AccountSid { get; set; } = string.Empty;

    /// <summary>Twilio Auth Token — found on the Twilio Console dashboard.</summary>
    public string AuthToken { get; set; } = string.Empty;

    /// <summary>Verified Twilio phone number or Messaging Service SID to send from (E.164 format).</summary>
    public string From { get; set; } = string.Empty;
}
