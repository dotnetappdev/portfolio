namespace Portfolio.Shared.Models;

/// <summary>DTO for a media file stored in the media library.</summary>
public class MediaFileDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MediaType { get; set; } = "image";
    public string? Alt { get; set; }
    public DateTime UploadedAt { get; set; }

    /// <summary>Public URL to access the file, e.g. "/uploads/media/a1b2c3.jpg".</summary>
    public string Url { get; set; } = string.Empty;
}
