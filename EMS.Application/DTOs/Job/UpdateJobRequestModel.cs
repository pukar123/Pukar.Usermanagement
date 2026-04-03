namespace EMS.Application.DTOs.Job;

public class UpdateJobRequestModel
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}
