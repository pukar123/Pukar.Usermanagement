using EMS.Application.DTOs.JobPosition;

namespace EMS.Application.Services.JobPositions;

public interface IJobPositionService
{
    Task<JobPositionResponseModel> CreateAsync(CreateJobPositionRequestModel request, CancellationToken cancellationToken = default);
    Task<JobPositionResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobPositionResponseModel>> GetByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<JobPositionResponseModel?> UpdateAsync(int id, UpdateJobPositionRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
