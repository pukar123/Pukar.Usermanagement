namespace EMS.API.Models.Department;

public class DepartmentResponse
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? ParentDepartmentId { get; set; }
    public bool IsActive { get; set; }
}
