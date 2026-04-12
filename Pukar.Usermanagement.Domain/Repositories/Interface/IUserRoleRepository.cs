using Pukar.Usermanagement.Domain.DbModels;

namespace Pukar.Usermanagement.Domain.Repositories.Interface;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<IReadOnlyCollection<string>> GetRoleNamesByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, IReadOnlyList<string>>> GetRoleNamesByUserIdsAsync(
        IEnumerable<int> userIds,
        CancellationToken cancellationToken = default);

    Task<UserRole?> GetByUserAndRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default);
}
