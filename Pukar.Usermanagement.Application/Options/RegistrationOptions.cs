namespace Pukar.Usermanagement.Application.Options;

/// <summary>Self-service registration behavior (host appsettings section <c>Registration</c>).</summary>
public sealed class RegistrationOptions
{
    public const string SectionName = "Registration";

    /// <summary>When true, new users must confirm email before receiving tokens on register; login is blocked until confirmed.</summary>
    public bool RequireEmailConfirmation { get; set; } = true;

    public int EmailConfirmationTokenExpirationHours { get; set; } = 24;
}
