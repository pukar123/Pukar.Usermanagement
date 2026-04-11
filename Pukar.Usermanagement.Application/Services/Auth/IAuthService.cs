using Pukar.Usermanagement.Application.DTOs.Auth;

namespace Pukar.Usermanagement.Application.Services.Auth;

public interface IAuthService
{
    Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel request, string? clientInfo, CancellationToken cancellationToken = default);

    Task<AuthResponseModel> ConfirmEmailAsync(ConfirmEmailRequestModel request, string? clientInfo, CancellationToken cancellationToken = default);

    Task ResendConfirmationEmailAsync(ResendConfirmationRequestModel request, CancellationToken cancellationToken = default);

    Task<AuthResponseModel> LoginAsync(LoginRequestModel request, string? clientInfo, CancellationToken cancellationToken = default);

    Task<AuthResponseModel> RefreshAsync(RefreshTokenRequestModel request, string? clientInfo, CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
