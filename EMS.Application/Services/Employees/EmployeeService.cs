using EMS.Application.DTOs.Employee;
using EMS.Domain.Helpers;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using JobPositionEntity = EMS.Domain.DbModels.JobPosition;

namespace EMS.Application.Services.Employees;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IBaseRepository<Employee> _repository;
    private readonly IBaseRepository<JobPositionEntity> _jobPositionRepository;

    public EmployeeService(IBaseRepository<Employee> repository, IBaseRepository<JobPositionEntity> jobPositionRepository)
    {
        _repository = repository;
        _jobPositionRepository = jobPositionRepository;
    }

    public async Task<EmployeeResponseModel> CreateAsync(CreateEmployeeRequestModel request, CancellationToken cancellationToken = default)
    {
        await EnsureJobPositionMatchesOrganizationAsync(request.OrganizationId, request.JobPositionId, cancellationToken);

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

        await EnsureJobPositionMatchesOrganizationAsync(request.OrganizationId, request.JobPositionId, cancellationToken);

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

    private async Task EnsureJobPositionMatchesOrganizationAsync(int organizationId, int? jobPositionId, CancellationToken cancellationToken)
    {
        if (jobPositionId is null)
            return;

        var jobPosition = await _jobPositionRepository.GetQueryable()
            .FirstOrDefaultAsync(j => j.Id == jobPositionId.Value, cancellationToken);

        if (jobPosition is null)
            throw new BusinessRuleException("Job position was not found.");

        if (jobPosition.OrganizationId != organizationId)
            throw new BusinessRuleException("Job position must belong to the same organization as the employee.");
    }
}
