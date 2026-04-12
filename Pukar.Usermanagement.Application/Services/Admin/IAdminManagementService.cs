using Pukar.Usermanagement.Application.DTOs.Admin;

namespace Pukar.Usermanagement.Application.Services.Admin;

public interface IAdminManagementService
{
    Task<string> CreateRoleAsync(CreateRoleRequestModel request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RoleResponseModel>> ListRolesAsync(CancellationToken cancellationToken = default);

    Task DeleteRoleAsync(int roleId, CancellationToken cancellationToken = default);

    Task<AdminUserResponseModel> CreateUserAsync(CreateUserRequestModel request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminUserResponseModel>> ListUsersAsync(CancellationToken cancellationToken = default);

    Task<AdminUserResponseModel?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<AdminUserResponseModel> UpdateUserAsync(int userId, UpdateUserRequestModel request, CancellationToken cancellationToken = default);

    Task AssignRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);

    Task RemoveRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);
}
