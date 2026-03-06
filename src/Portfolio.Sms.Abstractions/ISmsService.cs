namespace Portfolio.Sms.Abstractions;

/// <summary>Abstraction for an SMS delivery provider.</summary>
public interface ISmsService
{
    /// <summary>Sends a single SMS message and returns the result.</summary>
    Task<SmsResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);
}
