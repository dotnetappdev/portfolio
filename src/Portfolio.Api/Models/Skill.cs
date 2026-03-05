using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models;

public class Skill
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    public int Proficiency { get; set; }
}
