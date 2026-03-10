namespace Portfolio.Shared.Models;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ReadMinutes { get; set; }
    public string Tags { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string? FeaturedImage { get; set; }
    public string? GitHubUrl { get; set; }
    public string? GalleryImages { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? OgImage { get; set; }
    public string? CanonicalUrl { get; set; }

    public string[] TagList =>
        Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public string[] GalleryImageList =>
        GalleryImages?.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
}
