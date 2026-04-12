namespace Pukar.Usermanagement.Application.DTOs.Admin;

public class RoleResponseModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
