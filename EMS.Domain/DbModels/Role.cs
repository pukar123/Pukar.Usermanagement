namespace EMS.Domain.DbModels;

public class Role
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    public Organization Organization { get; set; } = null!;
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
