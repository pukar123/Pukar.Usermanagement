using EMS.Application.DTOs.Employee;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;

namespace EMS.Application.Services.Employees;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IBaseRepository<Employee> _repository;

    public EmployeeService(IBaseRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeResponseModel> CreateAsync(CreateEmployeeRequestModel request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var entity = EmployeeMapper.ToEntity(request);
        entity.CreatedAtUtc = now;
        entity.UpdatedAtUtc = now;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return EmployeeMapper.ToResponse(entity);
    }

    public async Task<EmployeeResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : EmployeeMapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<EmployeeResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync();
        return list.Select(EmployeeMapper.ToResponse).ToList();
    }

    public async Task<EmployeeResponseModel?> UpdateAsync(int id, UpdateEmployeeRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return null;

        EmployeeMapper.ApplyUpdate(entity, request);
        entity.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return EmployeeMapper.ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return false;

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
        return true;
    }
}
