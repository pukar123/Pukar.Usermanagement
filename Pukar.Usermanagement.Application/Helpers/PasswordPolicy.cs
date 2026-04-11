using Pukar.Shared;

namespace Pukar.Usermanagement.Application.Helpers;

/// <summary>Password rules for self-service registration (confirm password, complexity).</summary>
public static class PasswordPolicy
{
    public const int MinimumLength = 8;

    public static void EnsureMeetsComplexity(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new BusinessRuleException("Password is required.");

        if (password.Length < MinimumLength)
            throw new BusinessRuleException($"Password must be at least {MinimumLength} characters.");

        if (!password.Any(char.IsUpper))
            throw new BusinessRuleException("Password must contain at least one uppercase letter.");

        if (!password.Any(char.IsLower))
            throw new BusinessRuleException("Password must contain at least one lowercase letter.");

        if (!password.Any(char.IsDigit))
            throw new BusinessRuleException("Password must contain at least one digit.");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new BusinessRuleException("Password must contain at least one special character.");
    }
}
