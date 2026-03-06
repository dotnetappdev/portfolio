namespace Portfolio.Sms.ClickSend;

/// <summary>Configuration options for the ClickSend SMS provider.</summary>
public sealed class ClickSendOptions
{
    /// <summary>ClickSend account username (email address).</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>ClickSend API key from the dashboard.</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Optional sender ID shown to recipients (up to 11 alphanumeric chars),
    /// or a verified phone number. Defaults to "Portfolio" when null/empty.
    /// </summary>
    public string? From { get; set; }
}
