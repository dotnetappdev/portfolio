using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

public class HeroStat
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Value { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Label { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Color { get; set; } = "Primary";
    public int SortOrder { get; set; }
}
