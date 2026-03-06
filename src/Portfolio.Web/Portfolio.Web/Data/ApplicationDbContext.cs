using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Web.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

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

public class HeroStat
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Value { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Label { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Color { get; set; } = "Primary";
    public int SortOrder { get; set; }
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<HeroStat> HeroStats => Set<HeroStat>();
    public DbSet<SmsSettings> SmsSettings => Set<SmsSettings>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<HeroStat>().HasData(
            new HeroStat { Id = 1, Value = "30+",    Label = "Years in .NET",          Color = "Primary",   SortOrder = 1 },
            new HeroStat { Id = 2, Value = "AI",     Label = "First Approach",         Color = "Secondary", SortOrder = 2 },
            new HeroStat { Id = 3, Value = "SecOps", Label = "Security Built In",      Color = "Error",     SortOrder = 3 },
            new HeroStat { Id = 4, Value = "TDD/BDD",Label = "Test-Focused Developer", Color = "Success",   SortOrder = 4 }
        );

        // Seed a default (disabled) SMS settings row so the admin page always has a record
        builder.Entity<SmsSettings>().HasData(
            new SmsSettings { Id = 1, Provider = "None", IsEnabled = false }
        );
    }
}
