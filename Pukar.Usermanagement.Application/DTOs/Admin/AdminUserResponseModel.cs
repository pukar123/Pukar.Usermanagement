namespace Pukar.Usermanagement.Application.DTOs.Admin;

public class AdminUserResponseModel
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? UserName { get; set; }

    public bool IsActive { get; set; }
}
