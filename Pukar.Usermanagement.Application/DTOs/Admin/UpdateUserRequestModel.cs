namespace Pukar.Usermanagement.Application.DTOs.Admin;

public class UpdateUserRequestModel
{
    public string? UserName { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Optional password reset (BCrypt hash applied when non-empty).</summary>
    public string? Password { get; set; }
}
