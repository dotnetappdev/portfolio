namespace Portfolio.Shared.Models;

/// <summary>A single media item (image or video) attached to a blog post.</summary>
public class BlogMediaItem
{
    /// <summary>URL of the image or video (YouTube/Vimeo supported for embed).</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>"image" or "video".</summary>
    public string Type { get; set; } = "image";

    /// <summary>"before_article" or "after_article".</summary>
    public string Position { get; set; } = "after_article";

    /// <summary>Zero-based display order within the same position.</summary>
    public int SortOrder { get; set; }

    /// <summary>Optional caption shown below the image or video.</summary>
    public string? Caption { get; set; }
}
