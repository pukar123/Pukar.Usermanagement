namespace Pukar.Usermanagement.Application.DTOs.Auth;

public class RegisterResponseModel
{
    public bool RequiresEmailConfirmation { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Email { get; set; }

    /// <summary>Present when registration completes without a pending confirmation step.</summary>
    public AuthResponseModel? Auth { get; set; }
}
