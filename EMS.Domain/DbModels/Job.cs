namespace EMS.Domain.DbModels;

public class Job
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    public Role Role { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
