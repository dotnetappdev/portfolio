namespace Portfolio.Shared.Models;
public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Proficiency { get; set; }
}
