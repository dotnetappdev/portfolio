using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>A media file (image or video) stored in the media library.</summary>
public class MediaFile
{
    public int Id { get; set; }

    /// <summary>Unique stored filename (e.g. "a1b2c3-original.jpg").</summary>
    [Required, MaxLength(500)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>Original filename as supplied by the uploader.</summary>
    [MaxLength(500)]
    public string OriginalName { get; set; } = string.Empty;

    /// <summary>MIME content type, e.g. "image/jpeg" or "video/mp4".</summary>
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>File size in bytes.</summary>
    public long FileSize { get; set; }

    /// <summary>"image" or "video".</summary>
    [MaxLength(20)]
    public string MediaType { get; set; } = "image";

    /// <summary>Optional alt text / description for accessibility.</summary>
    [MaxLength(500)]
    public string? Alt { get; set; }

    /// <summary>When this file was uploaded (UTC).</summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
