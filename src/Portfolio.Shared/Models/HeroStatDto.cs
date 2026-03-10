namespace Portfolio.Shared.Models;

public class HeroStatDto
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Color { get; set; } = "Primary";
    public int SortOrder { get; set; }
}
