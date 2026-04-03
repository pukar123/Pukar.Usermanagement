namespace EMS.Application.DTOs.Role;

public class CreateRoleRequestModel
{
    public int OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}
