using EMS.Domain.Enums;

namespace EMS.Domain.DbModels;

public class Employee
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? LocationId { get; set; }
    public int? ManagerId { get; set; }
    public int? JobId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateJoined { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;
    public bool IsActive { get; set; } = true;
    public string? ExternalIdentityKey { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Organization Organization { get; set; } = null!;
    public Department? Department { get; set; }
    public Location? Location { get; set; }
    public Job? Job { get; set; }
    public Employee? Manager { get; set; }
    public ICollection<Employee> DirectReports { get; set; } = new List<Employee>();
}
