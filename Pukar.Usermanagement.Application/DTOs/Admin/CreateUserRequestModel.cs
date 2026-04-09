namespace Pukar.Usermanagement.Application.DTOs.Admin;

public class CreateUserRequestModel
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? UserName { get; set; }

    public bool IsActive { get; set; } = true;
}
