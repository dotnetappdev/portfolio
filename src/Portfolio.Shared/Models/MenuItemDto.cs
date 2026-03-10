namespace Portfolio.Shared.Models;

public class MenuItemDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; }
    public bool OpenInNewTab { get; set; }
}
