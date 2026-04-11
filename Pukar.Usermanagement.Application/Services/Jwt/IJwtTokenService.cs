namespace Pukar.Usermanagement.Application.Services.Jwt;

public interface IJwtTokenService
{
    string CreateAccessToken(
        int userId,
        string email,
        string? userName,
        bool emailConfirmed,
        IReadOnlyCollection<string> roles,
        DateTime utcNow,
        out DateTime accessTokenExpiresAtUtc);
}
