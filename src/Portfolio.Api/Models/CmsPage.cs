using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>An arbitrary CMS page with a custom slug and rich HTML content.</summary>
public class CmsPage
{
    public int Id { get; set; }

    [Required, MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>HTML body produced by the WYSIWYG editor.</summary>
    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Last time this page was saved/updated.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>URL of the featured/hero image for this page.</summary>
    [MaxLength(1000)]
    public string? FeaturedImage { get; set; }

    // ── SEO ──────────────────────────────────────────────────────────────────
    [MaxLength(300)]
    public string? MetaTitle { get; set; }
    [MaxLength(500)]
    public string? MetaDescription { get; set; }
    [MaxLength(500)]
    public string? OgImage { get; set; }
    [MaxLength(500)]
    public string? CanonicalUrl { get; set; }
}
