using Microsoft.EntityFrameworkCore;
using Pukar.Usermanagement.Domain.Database;
using Pukar.Usermanagement.Domain.DbModels;
using Pukar.Usermanagement.Domain.Repositories.Interface;

namespace Pukar.Usermanagement.Infrastructure.Repositories;

public sealed class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(UserManagementDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyCollection<string>> GetRoleNamesByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<UserRole>()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<int, IReadOnlyList<string>>> GetRoleNamesByUserIdsAsync(
        IEnumerable<int> userIds,
        CancellationToken cancellationToken = default)
    {
        var ids = userIds.Distinct().ToList();
        if (ids.Count == 0)
            return new Dictionary<int, IReadOnlyList<string>>();

        var rows = await Context.Set<UserRole>()
            .Where(ur => ids.Contains(ur.UserId))
            .Select(ur => new { ur.UserId, RoleName = ur.Role.Name })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(x => x.UserId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<string>)g.Select(x => x.RoleName).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(n => n).ToList());
    }

    public async Task<UserRole?> GetByUserAndRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<UserRole>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
    }
}
