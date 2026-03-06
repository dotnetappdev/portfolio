namespace Portfolio.Sms.Abstractions;

/// <summary>Result returned by an <see cref="ISmsService"/> send operation.</summary>
public sealed record SmsResult(
    bool Succeeded,
    string? MessageId = null,
    string? Error = null)
{
    /// <summary>Creates a successful result, optionally carrying the provider's message ID.</summary>
    public static SmsResult Ok(string? messageId = null) => new(true, messageId);

    /// <summary>Creates a failed result with the supplied error description.</summary>
    public static SmsResult Fail(string error) => new(false, Error: error);
}
