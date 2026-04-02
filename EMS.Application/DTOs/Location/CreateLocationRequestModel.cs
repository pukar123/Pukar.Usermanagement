namespace EMS.Application.DTOs.Location;

public class CreateLocationRequestModel
{
    public int OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public bool IsActive { get; set; } = true;
}
