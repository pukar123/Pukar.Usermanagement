namespace EMS.Application.DTOs.Organization;

public class OrganizationResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public string? Motto { get; set; }
    /// <summary>Relative URL path served as static files (e.g. /uploads/organizations/1/logo.png).</summary>
    public string? LogoRelativePath { get; set; }
}
