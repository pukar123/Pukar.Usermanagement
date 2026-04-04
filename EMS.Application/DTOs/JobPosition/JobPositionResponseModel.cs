namespace EMS.Application.DTOs.JobPosition;

public class JobPositionResponseModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}
