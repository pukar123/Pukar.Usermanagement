using EMS.Application.DTOs.Role;

namespace EMS.Application.Services.Roles;

public interface IRoleService
{
    Task<RoleResponseModel> CreateAsync(CreateRoleRequestModel request, CancellationToken cancellationToken = default);
    Task<RoleResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoleResponseModel>> GetByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<RoleResponseModel?> UpdateAsync(int id, UpdateRoleRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
