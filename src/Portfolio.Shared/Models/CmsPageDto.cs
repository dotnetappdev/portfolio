namespace Portfolio.Shared.Models;

public class CmsPageDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime PublishedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? FeaturedImage { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? OgImage { get; set; }
    public string? CanonicalUrl { get; set; }
}
