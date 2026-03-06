namespace Portfolio.Sms.Abstractions;

/// <summary>Represents an outgoing SMS message.</summary>
public sealed record SmsMessage(
    /// <summary>Recipient phone number in E.164 format, e.g. +447911123456</summary>
    string To,
    /// <summary>Message body (plain text, max 160 chars for single SMS).</summary>
    string Body,
    /// <summary>Optional sender override; falls back to the provider's configured From number.</summary>
    string? From = null);
