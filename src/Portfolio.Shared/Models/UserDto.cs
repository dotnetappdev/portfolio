namespace Portfolio.Shared.Models;

/// <summary>Represents a user account returned by the Portfolio API.</summary>
public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
