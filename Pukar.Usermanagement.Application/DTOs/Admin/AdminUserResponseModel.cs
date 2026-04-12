namespace Pukar.Usermanagement.Application.DTOs.Admin;

public class AdminUserResponseModel
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? UserName { get; set; }

    public bool IsActive { get; set; }

    public bool EmailConfirmed { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
}
