using EMS.Application.DTOs.Department;

namespace EMS.Application.Services.Departments;

public interface IDepartmentService
{
    Task<DepartmentResponseModel> CreateAsync(CreateDepartmentRequestModel request, CancellationToken cancellationToken = default);
    Task<DepartmentResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DepartmentResponseModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DepartmentResponseModel?> UpdateAsync(int id, UpdateDepartmentRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
