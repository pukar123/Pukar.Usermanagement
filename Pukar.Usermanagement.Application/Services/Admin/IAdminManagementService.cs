using Pukar.Usermanagement.Application.DTOs.Admin;

namespace Pukar.Usermanagement.Application.Services.Admin;

public interface IAdminManagementService
{
    Task<string> CreateRoleAsync(CreateRoleRequestModel request, CancellationToken cancellationToken = default);
    Task<AdminUserResponseModel> CreateUserAsync(CreateUserRequestModel request, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);
}
