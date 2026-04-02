namespace EMS.API.Models.Organization;

public class CreateOrganizationRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}
