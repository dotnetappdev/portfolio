namespace Portfolio.Shared.Models;

/// <summary>Payload for updating an existing user account.</summary>
public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    /// <summary>If non-empty, the user's password will be reset to this value.</summary>
    public string? NewPassword { get; set; }
}
