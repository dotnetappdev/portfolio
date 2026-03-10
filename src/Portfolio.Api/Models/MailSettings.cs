using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>Admin-configurable outbound email settings (single row, Id = 1).</summary>
public class MailSettings
{
    public int Id { get; set; }

    /// <summary>Whether outbound email is enabled.</summary>
    public bool IsEnabled { get; set; }

    /// <summary>Email provider: "Smtp" | "MailerSend"</summary>
    [MaxLength(50)]
    public string Provider { get; set; } = "Smtp";

    // ── Shared ────────────────────────────────────────────────────────────

    /// <summary>The "From" email address shown to recipients.</summary>
    [MaxLength(200)]
    public string? FromAddress { get; set; }

    /// <summary>Display name in the "From" field.</summary>
    [MaxLength(100)]
    public string? FromName { get; set; }

    // ── MailerSend ────────────────────────────────────────────────────────

    /// <summary>MailerSend API token (used when Provider = "MailerSend").</summary>
    [MaxLength(500)]
    public string? MailerSendApiKey { get; set; }

    // ── SMTP ──────────────────────────────────────────────────────────────

    [MaxLength(300)]
    public string? SmtpHost { get; set; }

    public int SmtpPort { get; set; } = 587;

    /// <summary>SMTP login username (often an email address).</summary>
    [MaxLength(200)]
    public string? SmtpUsername { get; set; }

    [MaxLength(500)]
    public string? SmtpPassword { get; set; }

    /// <summary>Whether to use SSL/TLS (true) or STARTTLS / no encryption (false).</summary>
    public bool UseSsl { get; set; } = true;
}
