namespace EMS.Application.DTOs.Role;

public class UpdateRoleRequestModel
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}
