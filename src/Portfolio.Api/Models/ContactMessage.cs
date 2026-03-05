using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

public class ContactMessage
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(300)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Subject { get; set; } = string.Empty;
    [Required]
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
