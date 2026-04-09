using Pukar.Usermanagement.Domain.DbModels;

namespace Pukar.Usermanagement.Domain.Repositories.Interface;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
}
