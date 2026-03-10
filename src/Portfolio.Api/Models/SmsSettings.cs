using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>Admin-configurable SMS provider settings (single row, Id = 1).</summary>
public class SmsSettings
{
    public int Id { get; set; }

    /// <summary>Active provider: "None" | "Twilio" | "ClickSend"</summary>
    [MaxLength(50)]
    public string Provider { get; set; } = "None";

    /// <summary>Whether SMS sending is globally enabled.</summary>
    public bool IsEnabled { get; set; }

    /// <summary>Phone number (E.164) that admin alert messages are sent to.</summary>
    [MaxLength(30)]
    public string? AdminReceiverNumber { get; set; }

    // ── Twilio ──────────────────────────────────────────────────────────────
    [MaxLength(100)]
    public string? TwilioAccountSid { get; set; }
    [MaxLength(200)]
    public string? TwilioAuthToken { get; set; }
    /// <summary>Verified Twilio number or Messaging Service SID used as the sender.</summary>
    [MaxLength(50)]
    public string? TwilioFromNumber { get; set; }

    // ── ClickSend ────────────────────────────────────────────────────────────
    [MaxLength(200)]
    public string? ClickSendUsername { get; set; }
    [MaxLength(200)]
    public string? ClickSendApiKey { get; set; }
    /// <summary>Up to 11-char sender ID or verified number shown to recipients.</summary>
    [MaxLength(50)]
    public string? ClickSendFromName { get; set; }
}
