namespace EMS.Domain.DbModels;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>Longer description for setup screens and profiles.</summary>
    public string? Description { get; set; }

    /// <summary>Short motto or tagline.</summary>
    public string? Motto { get; set; }

    /// <summary>Web-relative path to logo file on disk (e.g. /uploads/organizations/1/xxx.png). Not binary in DB.</summary>
    public string? LogoRelativePath { get; set; }
}
