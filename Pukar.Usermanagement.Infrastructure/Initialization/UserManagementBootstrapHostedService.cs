using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pukar.Usermanagement.Application.Options;
using Pukar.Usermanagement.Application.Services.Password;
using Pukar.Usermanagement.Domain.Database;
using Pukar.Usermanagement.Domain.DbModels;
using Pukar.Shared;

namespace Pukar.Usermanagement.Infrastructure.Initialization;

public sealed class UserManagementBootstrapHostedService : IHostedService
{
    private const string AdminRoleName = "Admin";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<BootstrapAdminOptions> _bootstrapOptions;
    private readonly ILogger<UserManagementBootstrapHostedService> _logger;

    public UserManagementBootstrapHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<BootstrapAdminOptions> bootstrapOptions,
        ILogger<UserManagementBootstrapHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _bootstrapOptions = bootstrapOptions;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await db.Database.MigrateAsync(cancellationToken);

        var utcNow = DateTime.UtcNow;
        var adminRole = await EnsureAdminRoleAsync(db, utcNow, cancellationToken);

        var options = _bootstrapOptions.Value;
        if (!options.EnableSeeding)
            return;

        if (string.IsNullOrWhiteSpace(options.Email) || string.IsNullOrWhiteSpace(options.Password))
        {
            _logger.LogWarning("Admin seeding is enabled but Email/Password is missing in {SectionName}.", BootstrapAdminOptions.SectionName);
            return;
        }

        var normalizedEmail = EmailNormalizer.Normalize(options.Email);
        var user = await db.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        if (user is null)
        {
            user = new User
            {
                Email = options.Email.Trim(),
                NormalizedEmail = normalizedEmail,
                PasswordHash = hasher.HashPassword(options.Password),
                UserName = string.IsNullOrWhiteSpace(options.UserName) ? null : options.UserName.Trim(),
                IsActive = true,
                CreatedAtUtc = utcNow,
            };
            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);
        }

        var hasAdminRole = await db.UserRoles.AnyAsync(
            ur => ur.UserId == user.Id && ur.RoleId == adminRole.Id,
            cancellationToken);

        if (hasAdminRole)
            return;

        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = adminRole.Id,
        });
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task<Role> EnsureAdminRoleAsync(UserManagementDbContext db, DateTime utcNow, CancellationToken cancellationToken)
    {
        var normalizedRoleName = AdminRoleName.ToUpperInvariant();
        var role = await db.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        if (role is not null)
            return role;

        role = new Role
        {
            Name = AdminRoleName,
            NormalizedName = normalizedRoleName,
            CreatedAtUtc = utcNow,
        };
        db.Roles.Add(role);
        await db.SaveChangesAsync(cancellationToken);
        return role;
    }
}
