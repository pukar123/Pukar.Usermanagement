namespace Pukar.Usermanagement.Application.Options;

public class BootstrapAdminOptions
{
    public const string SectionName = "UserManagementBootstrapAdmin";

    public bool EnableSeeding { get; set; } = true;

    public string Email { get; set; } = "admin@ems.local";

    public string Password { get; set; } = "Admin@123456";

    public string UserName { get; set; } = "admin";
}
