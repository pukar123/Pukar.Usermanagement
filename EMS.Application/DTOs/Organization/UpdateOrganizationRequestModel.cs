namespace EMS.Application.DTOs.Organization;

public class UpdateOrganizationRequestModel
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}
