using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

/// <summary>A navigation menu item stored in the database.</summary>
public class MenuItem
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Label { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Url { get; set; } = string.Empty;

    /// <summary>MUI icon name, e.g. "Home", "Article". Empty = no icon.</summary>
    [MaxLength(100)]
    public string Icon { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsVisible { get; set; } = true;

    /// <summary>When true the link opens in a new browser tab.</summary>
    public bool OpenInNewTab { get; set; }
}
