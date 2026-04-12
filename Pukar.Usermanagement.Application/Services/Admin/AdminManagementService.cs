using Pukar.Usermanagement.Application.DTOs.Admin;
using Pukar.Usermanagement.Application.Services.Password;
using Pukar.Usermanagement.Domain.DbModels;
using Pukar.Usermanagement.Domain.Repositories.Interface;
using Pukar.Shared;

namespace Pukar.Usermanagement.Application.Services.Admin;

public sealed class AdminManagementService : IAdminManagementService
{
    private const string ProtectedAdminRoleNormalizedName = "ADMIN";

    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IUserRoleRepository _userRoles;
    private readonly IPasswordHasher _passwordHasher;

    public AdminManagementService(
        IUserRepository users,
        IRoleRepository roles,
        IUserRoleRepository userRoles,
        IPasswordHasher passwordHasher)
    {
        _users = users;
        _roles = roles;
        _userRoles = userRoles;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> CreateRoleAsync(CreateRoleRequestModel request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BusinessRuleException("Role name is required.");

        var roleName = request.Name.Trim();
        var normalized = roleName.ToUpperInvariant();

        if (await _roles.GetByNormalizedNameAsync(normalized, cancellationToken) is not null)
            throw new BusinessRuleException("Role already exists.");

        await _roles.AddAsync(new Role
        {
            Name = roleName,
            NormalizedName = normalized,
            CreatedAtUtc = DateTime.UtcNow,
        }, cancellationToken);
        await _roles.SaveChangesAsync(cancellationToken);

        return roleName;
    }

    public async Task<IReadOnlyList<RoleResponseModel>> ListRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _roles.GetAllOrderedByNameAsync(cancellationToken);
        return roles.Select(r => new RoleResponseModel
        {
            Id = r.Id,
            Name = r.Name,
            CreatedAtUtc = r.CreatedAtUtc,
        }).ToList();
    }

    public async Task DeleteRoleAsync(int roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            throw new BusinessRuleException("Role not found.");

        if (string.Equals(role.NormalizedName, ProtectedAdminRoleNormalizedName, StringComparison.Ordinal))
            throw new BusinessRuleException("The Admin role cannot be deleted.");

        _roles.Remove(role);
        await _roles.SaveChangesAsync(cancellationToken);
    }

    public async Task<AdminUserResponseModel> CreateUserAsync(CreateUserRequestModel request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new BusinessRuleException("Email and password are required.");

        var normalized = EmailNormalizer.Normalize(request.Email);
        if (await _users.GetByNormalizedEmailAsync(normalized, cancellationToken) is not null)
            throw new DuplicateEmailException();

        var user = new User
        {
            Email = request.Email.Trim(),
            NormalizedEmail = normalized,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            UserName = string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName.Trim(),
            IsActive = request.IsActive,
            CreatedAtUtc = DateTime.UtcNow,
            EmailConfirmed = true,
        };

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        return MapUser(user, Array.Empty<string>());
    }

    public async Task<IReadOnlyList<AdminUserResponseModel>> ListUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _users.GetAllOrderedByEmailAsync(cancellationToken);
        var ids = users.Select(u => u.Id).ToList();
        var rolesByUser = await _userRoles.GetRoleNamesByUserIdsAsync(ids, cancellationToken);

        return users.Select(u =>
        {
            rolesByUser.TryGetValue(u.Id, out var roles);
            return MapUser(u, roles ?? Array.Empty<string>());
        }).ToList();
    }

    public async Task<AdminUserResponseModel?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return null;

        var roles = await _userRoles.GetRoleNamesByUserIdAsync(userId, cancellationToken);
        return MapUser(user, roles.ToList());
    }

    public async Task<AdminUserResponseModel> UpdateUserAsync(int userId, UpdateUserRequestModel request, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new BusinessRuleException("User not found.");

        if (request.UserName is not null)
            user.UserName = string.IsNullOrWhiteSpace(request.UserName) ? null : request.UserName.Trim();

        if (request.IsActive.HasValue)
            user.IsActive = request.IsActive.Value;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = _passwordHasher.HashPassword(request.Password);

        _users.Update(user);
        await _users.SaveChangesAsync(cancellationToken);

        var roles = await _userRoles.GetRoleNamesByUserIdAsync(userId, cancellationToken);
        return MapUser(user, roles.ToList());
    }

    public async Task AssignRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new BusinessRuleException("User not found.");

        var role = await GetRequiredRoleAsync(roleName, cancellationToken);
        var existing = await _userRoles.GetByUserAndRoleAsync(userId, role.Id, cancellationToken);
        if (existing is not null)
            return;

        await _userRoles.AddAsync(new UserRole
        {
            UserId = userId,
            RoleId = role.Id,
        }, cancellationToken);
        await _userRoles.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default)
    {
        var role = await GetRequiredRoleAsync(roleName, cancellationToken);
        var existing = await _userRoles.GetByUserAndRoleAsync(userId, role.Id, cancellationToken);
        if (existing is null)
            return;

        _userRoles.Remove(existing);
        await _userRoles.SaveChangesAsync(cancellationToken);
    }

    private async Task<Role> GetRequiredRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new BusinessRuleException("Role name is required.");

        var normalized = roleName.Trim().ToUpperInvariant();
        var role = await _roles.GetByNormalizedNameAsync(normalized, cancellationToken);
        if (role is null)
            throw new BusinessRuleException("Role not found.");

        return role;
    }

    private static AdminUserResponseModel MapUser(User user, IReadOnlyList<string> roles)
    {
        return new AdminUserResponseModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            CreatedAtUtc = user.CreatedAtUtc,
            LastLoginAtUtc = user.LastLoginAtUtc,
            Roles = roles,
        };
    }
}
