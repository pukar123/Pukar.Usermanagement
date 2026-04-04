namespace EMS.Application.DTOs.JobPosition;

public class UpdateJobPositionRequestModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}
