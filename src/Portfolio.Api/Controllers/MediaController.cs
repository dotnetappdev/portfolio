using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Models;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/media")]
[Authorize(Roles = "Admin")]
public class MediaController : ControllerBase
{
    private const long MaxFileSizeBytes = 100 * 1024 * 1024; // 100 MB
    private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/gif", "image/webp", "image/svg+xml"];
    private static readonly string[] AllowedVideoTypes = ["video/mp4", "video/webm", "video/ogg", "video/quicktime"];

    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MediaController> _logger;

    public MediaController(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        ILogger<MediaController> logger)
    {
        _context = context;
        _env     = env;
        _logger  = logger;
    }

    /// <summary>Returns all media files, newest first.</summary>
    [HttpGet]
    public async Task<ActionResult<List<MediaFileDto>>> GetAll()
    {
        var files = await _context.MediaFiles
            .OrderByDescending(f => f.UploadedAt)
            .ToListAsync();

        var baseUrl = GetMediaBaseUrl();
        return files.Select(f => ToDto(f, baseUrl)).ToList();
    }

    /// <summary>Uploads a file to the media library.</summary>
    [HttpPost("upload")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<ActionResult<MediaFileDto>> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        if (file.Length > MaxFileSizeBytes)
            return BadRequest($"File exceeds the 100 MB size limit.");

        var mediaType = DetermineMediaType(file.ContentType);
        if (mediaType is null)
            return BadRequest($"Unsupported file type: {file.ContentType}. Allowed: images (JPEG, PNG, GIF, WebP, SVG) and videos (MP4, WebM, OGG, MOV).");

        var uploadsDir = GetUploadsDirectory();
        Directory.CreateDirectory(uploadsDir);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var storedName = $"{Guid.NewGuid():N}{extension}";
        var fullPath   = Path.Combine(uploadsDir, storedName);

        try
        {
            await using var stream = System.IO.File.Create(fullPath);
            await file.CopyToAsync(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write uploaded file {FileName}", storedName);
            return StatusCode(500, "Failed to save file.");
        }

        var mediaFile = new MediaFile
        {
            FileName     = storedName,
            OriginalName = file.FileName,
            ContentType  = file.ContentType,
            FileSize     = file.Length,
            MediaType    = mediaType,
            UploadedAt   = DateTime.UtcNow
        };

        _context.MediaFiles.Add(mediaFile);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Media file uploaded: {FileName} ({Size} bytes)", storedName, file.Length);

        return CreatedAtAction(nameof(GetAll), ToDto(mediaFile, GetMediaBaseUrl()));
    }

    /// <summary>Deletes a media file from the library and from disk.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile is null) return NotFound();

        // Remove from disk
        var uploadsDir = GetUploadsDirectory();
        var fullPath   = Path.Combine(uploadsDir, mediaFile.FileName);
        if (System.IO.File.Exists(fullPath))
        {
            try { System.IO.File.Delete(fullPath); }
            catch (Exception ex) { _logger.LogWarning(ex, "Could not delete file {Path}", fullPath); }
        }

        _context.MediaFiles.Remove(mediaFile);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Updates the alt text of a media file.</summary>
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAlt(int id, [FromBody] UpdateMediaAltDto dto)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(id);
        if (mediaFile is null) return NotFound();
        mediaFile.Alt = dto.Alt;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private string GetUploadsDirectory() =>
        Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "media");

    private string GetMediaBaseUrl()
    {
        var request = Request;
        return $"{request.Scheme}://{request.Host}";
    }

    private static string? DetermineMediaType(string contentType)
    {
        if (AllowedImageTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase)) return "image";
        if (AllowedVideoTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase)) return "video";
        return null;
    }

    private static MediaFileDto ToDto(MediaFile f, string baseUrl) => new()
    {
        Id           = f.Id,
        FileName     = f.FileName,
        OriginalName = f.OriginalName,
        ContentType  = f.ContentType,
        FileSize     = f.FileSize,
        MediaType    = f.MediaType,
        Alt          = f.Alt,
        UploadedAt   = f.UploadedAt,
        Url          = $"{baseUrl}/uploads/media/{f.FileName}",
    };
}

/// <summary>Payload for PATCH /api/media/{id}.</summary>
public sealed class UpdateMediaAltDto
{
    public string? Alt { get; set; }
}
