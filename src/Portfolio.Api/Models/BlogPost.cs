using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

public class BlogPost
{
    public int Id { get; set; }

    [Required, MaxLength(300)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Summary { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Last time this post was saved/updated.</summary>
    public DateTime? UpdatedAt { get; set; }

    public int ReadMinutes { get; set; } = 5;

    /// <summary>Comma-separated list of tags, e.g. "AI, .NET, C#".</summary>
    [MaxLength(500)]
    public string Tags { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;

    /// <summary>URL of the featured/hero image shown at the top of the post and in listing cards.</summary>
    [MaxLength(1000)]
    public string? FeaturedImage { get; set; }

    // ── SEO ──────────────────────────────────────────────────────────────────
    /// <summary>Overrides the browser tab title and og:title for this post.</summary>
    [MaxLength(300)]
    public string? MetaTitle { get; set; }

    /// <summary>meta description and og:description for search engines.</summary>
    [MaxLength(500)]
    public string? MetaDescription { get; set; }

    /// <summary>Open Graph image URL shown when shared on social media.</summary>
    [MaxLength(500)]
    public string? OgImage { get; set; }

    /// <summary>Explicit canonical URL; if null the current request URL is used.</summary>
    [MaxLength(500)]
    public string? CanonicalUrl { get; set; }
}
