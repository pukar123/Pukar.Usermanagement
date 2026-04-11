using System.Net.Mail;
using Microsoft.Extensions.Options;
using Pukar.Usermanagement.Application.DTOs.Auth;
using Pukar.Usermanagement.Application.Helpers;
using Pukar.Usermanagement.Application.Options;
using Pukar.Usermanagement.Application.Services.Email;
using Pukar.Usermanagement.Application.Services.Jwt;
using Pukar.Usermanagement.Application.Services.Password;
using Pukar.Usermanagement.Domain.DbModels;
using Pukar.Shared;
using Pukar.Usermanagement.Domain.Repositories.Interface;

namespace Pukar.Usermanagement.Application.Services.Auth;

public sealed class AuthService : IAuthService
{
    private const string DefaultSelfRegistrationRoleName = "User";

    private readonly IUserRepository _users;
    private readonly IUserRoleRepository _userRoles;
    private readonly IRoleRepository _roles;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwt;
    private readonly JwtTokenOptions _jwtOptions;
    private readonly RegistrationOptions _registrationOptions;
    private readonly EmailOptions _emailOptions;
    private readonly IEmailSender _emailSender;

    public AuthService(
        IUserRepository users,
        IUserRoleRepository userRoles,
        IRoleRepository roles,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwt,
        IOptions<JwtTokenOptions> jwtOptions,
        IOptions<RegistrationOptions> registrationOptions,
        IOptions<EmailOptions> emailOptions,
        IEmailSender emailSender)
    {
        _users = users;
        _userRoles = userRoles;
        _roles = roles;
        _refreshTokens = refreshTokens;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
        _jwtOptions = jwtOptions.Value;
        _registrationOptions = registrationOptions.Value;
        _emailOptions = emailOptions.Value;
        _emailSender = emailSender;
    }

    public async Task<RegisterResponseModel> RegisterAsync(
        RegisterRequestModel request,
        string? clientInfo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new BusinessRuleException("Email and password are required.");

        if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
            throw new BusinessRuleException("Confirm password is required.");

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
            throw new BusinessRuleException("Password and confirm password do not match.");

        try
        {
            _ = new MailAddress(request.Email.Trim());
        }
        catch (FormatException)
        {
            throw new BusinessRuleException("Invalid email address.");
        }

        PasswordPolicy.EnsureMeetsComplexity(request.Password);

        var normalized = EmailNormalizer.Normalize(request.Email);
        if (await _users.GetByNormalizedEmailAsync(normalized, cancellationToken) is not null)
            throw new DuplicateEmailException();

        var utcNow = DateTime.UtcNow;
        var requireConfirmation = _registrationOptions.RequireEmailConfirmation;

        var user = new User
        {
            Email = request.Email.Trim(),
            NormalizedEmail = normalized,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            UserName = string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName.Trim(),
            IsActive = true,
            CreatedAtUtc = utcNow,
            EmailConfirmed = !requireConfirmation,
        };

        string? plainConfirmationToken = null;
        if (requireConfirmation)
        {
            plainConfirmationToken = RefreshTokenCrypto.GenerateOpaqueToken();
            user.EmailConfirmationTokenHash = RefreshTokenCrypto.HashToken(plainConfirmationToken);
            user.EmailConfirmationExpiresAtUtc = utcNow.AddHours(
                Math.Clamp(_registrationOptions.EmailConfirmationTokenExpirationHours, 1, 168));
        }

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        var registrationRole = await _roles.GetByNormalizedNameAsync(
            DefaultSelfRegistrationRoleName.ToUpperInvariant(),
            cancellationToken);
        if (registrationRole is not null)
        {
            await _userRoles.AddAsync(
                new UserRole { UserId = user.Id, RoleId = registrationRole.Id },
                cancellationToken);
            await _userRoles.SaveChangesAsync(cancellationToken);
        }

        if (requireConfirmation && plainConfirmationToken is not null)
        {
            var link = BuildConfirmationLink(_emailOptions.ConfirmationLinkBaseUrl, plainConfirmationToken);
            var body = string.Format(_emailOptions.ConfirmationEmailBodyTemplate, link);
            await _emailSender.SendAsync(user.Email, _emailOptions.ConfirmationEmailSubject, body, cancellationToken);

            return new RegisterResponseModel
            {
                RequiresEmailConfirmation = true,
                Email = user.Email,
                Message = "Registration successful. Please check your email to confirm your account before signing in.",
            };
        }

        var auth = await IssueTokensAsync(user, clientInfo, utcNow, cancellationToken);
        return new RegisterResponseModel
        {
            RequiresEmailConfirmation = false,
            Email = user.Email,
            Message = "Registration successful.",
            Auth = auth,
        };
    }

    public async Task<AuthResponseModel> ConfirmEmailAsync(
        ConfirmEmailRequestModel request,
        string? clientInfo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            throw new BusinessRuleException("Token is required.");

        var hash = RefreshTokenCrypto.HashToken(request.Token);
        var user = await _users.GetByEmailConfirmationTokenHashAsync(hash, cancellationToken);
        if (user is null)
            throw new BusinessRuleException("Invalid or expired confirmation link.");

        var utcNow = DateTime.UtcNow;
        if (user.EmailConfirmationExpiresAtUtc is null || user.EmailConfirmationExpiresAtUtc < utcNow)
            throw new BusinessRuleException("Invalid or expired confirmation link.");

        if (user.EmailConfirmed)
        {
            return await IssueTokensAsync(user, clientInfo, utcNow, cancellationToken);
        }

        user.EmailConfirmed = true;
        user.EmailConfirmationTokenHash = null;
        user.EmailConfirmationExpiresAtUtc = null;
        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        return await IssueTokensAsync(user, clientInfo, utcNow, cancellationToken);
    }

    public async Task ResendConfirmationEmailAsync(
        ResendConfirmationRequestModel request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return;

        try
        {
            _ = new MailAddress(request.Email.Trim());
        }
        catch (FormatException)
        {
            return;
        }

        var normalized = EmailNormalizer.Normalize(request.Email);
        var user = await _users.GetByNormalizedEmailAsync(normalized, cancellationToken);
        if (user is null || user.EmailConfirmed || !user.IsActive)
            return;

        if (!_registrationOptions.RequireEmailConfirmation)
            return;

        var utcNow = DateTime.UtcNow;
        var plainConfirmationToken = RefreshTokenCrypto.GenerateOpaqueToken();
        user.EmailConfirmationTokenHash = RefreshTokenCrypto.HashToken(plainConfirmationToken);
        user.EmailConfirmationExpiresAtUtc = utcNow.AddHours(
            Math.Clamp(_registrationOptions.EmailConfirmationTokenExpirationHours, 1, 168));
        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        var link = BuildConfirmationLink(_emailOptions.ConfirmationLinkBaseUrl, plainConfirmationToken);
        var body = string.Format(_emailOptions.ConfirmationEmailBodyTemplate, link);
        await _emailSender.SendAsync(user.Email, _emailOptions.ConfirmationEmailSubject, body, cancellationToken);
    }

    public async Task<AuthResponseModel> LoginAsync(
        LoginRequestModel request,
        string? clientInfo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new BusinessRuleException("Email and password are required.");

        var normalized = EmailNormalizer.Normalize(request.Email);
        var user = await _users.GetByNormalizedEmailAsync(normalized, cancellationToken);
        if (user is null || !user.IsActive)
            throw new BusinessRuleException("Invalid email or password.");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new BusinessRuleException("Invalid email or password.");

        if (_registrationOptions.RequireEmailConfirmation && !user.EmailConfirmed)
            throw new BusinessRuleException("Please confirm your email before signing in.");

        var utcNow = DateTime.UtcNow;
        user.LastLoginAtUtc = utcNow;
        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        return await IssueTokensAsync(user, clientInfo, utcNow, cancellationToken);
    }

    public async Task<AuthResponseModel> RefreshAsync(
        RefreshTokenRequestModel request,
        string? clientInfo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new BusinessRuleException("Refresh token is required.");

        var hash = RefreshTokenCrypto.HashToken(request.RefreshToken);
        var stored = await _refreshTokens.GetActiveByTokenHashAsync(hash, cancellationToken);
        if (stored is null || stored.RevokedAtUtc is not null || stored.ExpiresAtUtc < DateTime.UtcNow)
            throw new BusinessRuleException("Invalid or expired refresh token.");

        var user = await _users.GetByIdAsync(stored.UserId, cancellationToken);
        if (user is null || !user.IsActive)
            throw new BusinessRuleException("Invalid or expired refresh token.");

        if (_registrationOptions.RequireEmailConfirmation && !user.EmailConfirmed)
            throw new BusinessRuleException("Please confirm your email before signing in.");

        var utcNow = DateTime.UtcNow;
        stored.RevokedAtUtc = utcNow;
        _refreshTokens.Update(stored);

        await _refreshTokens.SaveChangesAsync(cancellationToken);

        return await IssueTokensAsync(user, clientInfo, utcNow, cancellationToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        var hash = RefreshTokenCrypto.HashToken(refreshToken);
        var stored = await _refreshTokens.GetActiveByTokenHashAsync(hash, cancellationToken);
        if (stored is null)
            return;

        stored.RevokedAtUtc = DateTime.UtcNow;
        _refreshTokens.Update(stored);
        await _refreshTokens.SaveChangesAsync(cancellationToken);
    }

    private async Task<AuthResponseModel> IssueTokensAsync(
        User user,
        string? clientInfo,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        var roles = await _userRoles.GetRoleNamesByUserIdAsync(user.Id, cancellationToken);
        var accessToken = _jwt.CreateAccessToken(
            user.Id,
            user.Email,
            user.UserName,
            user.EmailConfirmed,
            roles,
            utcNow,
            out var accessExpires);

        var plainRefresh = RefreshTokenCrypto.GenerateOpaqueToken();
        var refreshHash = RefreshTokenCrypto.HashToken(plainRefresh);

        var refreshEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAtUtc = utcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            CreatedAtUtc = utcNow,
            ClientInfo = TruncateClientInfo(clientInfo),
        };

        await _refreshTokens.AddAsync(refreshEntity, cancellationToken);
        await _refreshTokens.SaveChangesAsync(cancellationToken);

        return new AuthResponseModel
        {
            AccessToken = accessToken,
            RefreshToken = plainRefresh,
            AccessTokenExpiresAtUtc = accessExpires,
            TokenType = "Bearer",
            User = new UserResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
            },
        };
    }

    private static string? TruncateClientInfo(string? clientInfo)
    {
        if (string.IsNullOrEmpty(clientInfo))
            return null;
        return clientInfo.Length <= 512 ? clientInfo : clientInfo[..512];
    }

    private static string BuildConfirmationLink(string? baseUrl, string plainToken)
    {
        var encoded = Uri.EscapeDataString(plainToken);
        if (string.IsNullOrWhiteSpace(baseUrl))
            return $"(configure {EmailOptions.SectionName}:{nameof(EmailOptions.ConfirmationLinkBaseUrl)}) {encoded}";

        return $"{baseUrl.TrimEnd()}{encoded}";
    }
}
