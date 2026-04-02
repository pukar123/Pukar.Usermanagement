using EMS.Application.DTOs.Department;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;

namespace EMS.Application.Services.Departments;

public sealed class DepartmentService : IDepartmentService
{
    private readonly IBaseRepository<Department> _repository;

    public DepartmentService(IBaseRepository<Department> repository)
    {
        _repository = repository;
    }

    public async Task<DepartmentResponseModel> CreateAsync(CreateDepartmentRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = DepartmentMapper.ToEntity(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return DepartmentMapper.ToResponse(entity);
    }

    public async Task<DepartmentResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : DepartmentMapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<DepartmentResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync();
        return list.Select(DepartmentMapper.ToResponse).ToList();
    }

    public async Task<DepartmentResponseModel?> UpdateAsync(int id, UpdateDepartmentRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return null;

        DepartmentMapper.ApplyUpdate(entity, request);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return DepartmentMapper.ToResponse(entity);
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
