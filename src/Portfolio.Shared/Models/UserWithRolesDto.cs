namespace Portfolio.Shared.Models;

/// <summary>User account details including assigned roles and claims.</summary>
public class UserWithRolesDto : UserDto
{
    public List<string> Roles { get; set; } = [];
    public List<ClaimDto> Claims { get; set; } = [];
    public bool TwoFactorEnabled { get; set; }
}

/// <summary>Represents a single identity claim on a user.</summary>
public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
