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

    public async Task<UserRole?> GetByUserAndRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<UserRole>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
    }
}
