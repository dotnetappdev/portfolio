using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

public class Project
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [MaxLength(500)]
    public string TechStack { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? GitHubUrl { get; set; }
    [MaxLength(200)]
    public string? LiveUrl { get; set; }
    [MaxLength(200)]
    public string? ImageUrl { get; set; }
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}
