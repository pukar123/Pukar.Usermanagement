using EMS.Application.DTOs.Organization;

namespace EMS.Application.Services.Organizations;

public interface IOrganizationService
{
    Task<OrganizationDTO> CreateAsync(OrganizationDTO dto, CancellationToken cancellationToken = default);
    Task<OrganizationResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrganizationResponseModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OrganizationResponseModel?> UpdateAsync(int id, UpdateOrganizationRequestModel request, CancellationToken cancellationToken = default);
    Task<OrganizationResponseModel?> UpdateLogoRelativePathAsync(int id, string logoRelativePath, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
