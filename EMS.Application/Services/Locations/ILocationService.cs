using EMS.Application.DTOs.Location;

namespace EMS.Application.Services.Locations;

public interface ILocationService
{
    Task<LocationResponseModel> CreateAsync(CreateLocationRequestModel request, CancellationToken cancellationToken = default);
    Task<LocationResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LocationResponseModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LocationResponseModel?> UpdateAsync(int id, UpdateLocationRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
