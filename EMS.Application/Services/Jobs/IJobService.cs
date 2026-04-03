using EMS.Application.DTOs.Job;

namespace EMS.Application.Services.Jobs;

public interface IJobService
{
    Task<JobResponseModel> CreateAsync(CreateJobRequestModel request, CancellationToken cancellationToken = default);
    Task<JobResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobResponseModel>> GetByRoleAsync(int roleId, CancellationToken cancellationToken = default);
    Task<JobResponseModel?> UpdateAsync(int id, UpdateJobRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
