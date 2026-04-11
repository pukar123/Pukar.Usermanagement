namespace Pukar.Usermanagement.Application.Options;

/// <summary>Email templates for verification (host appsettings section <c>Email</c>).</summary>
public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string ConfirmationEmailSubject { get; set; } = "Confirm your email";

    /// <summary>Message body; <c>{0}</c> = confirmation URL.</summary>
    public string ConfirmationEmailBodyTemplate { get; set; } = "Please confirm your account by opening this link: {0}";

    /// <summary>Optional base URL for the confirmation link (token is appended). Example: <c>https://localhost:3000/auth/confirm?token=</c></summary>
    public string? ConfirmationLinkBaseUrl { get; set; }
}
