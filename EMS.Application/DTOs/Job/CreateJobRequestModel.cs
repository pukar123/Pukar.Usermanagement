namespace EMS.Application.DTOs.Job;

public class CreateJobRequestModel
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}
