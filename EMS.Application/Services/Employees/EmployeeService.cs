using EMS.Application.DTOs.Employee;
using EMS.Application.Exceptions;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using JobEntity = EMS.Domain.DbModels.Job;

namespace EMS.Application.Services.Employees;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IBaseRepository<Employee> _repository;
    private readonly IBaseRepository<JobEntity> _jobRepository;

    public EmployeeService(IBaseRepository<Employee> repository, IBaseRepository<JobEntity> jobRepository)
    {
        _repository = repository;
        _jobRepository = jobRepository;
    }

    public async Task<EmployeeResponseModel> CreateAsync(CreateEmployeeRequestModel request, CancellationToken cancellationToken = default)
    {
        await EnsureJobMatchesOrganizationAsync(request.OrganizationId, request.JobId, cancellationToken);

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

        await EnsureJobMatchesOrganizationAsync(request.OrganizationId, request.JobId, cancellationToken);

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

    private async Task EnsureJobMatchesOrganizationAsync(int organizationId, int? jobId, CancellationToken cancellationToken)
    {
        if (jobId is null)
            return;

        var job = await _jobRepository.GetQueryable()
            .Include(j => j.Role)
            .FirstOrDefaultAsync(j => j.Id == jobId.Value, cancellationToken);

        if (job is null)
            throw new BusinessRuleException("Job was not found.");

        if (job.Role.OrganizationId != organizationId)
            throw new BusinessRuleException("Job must belong to the same organization as the employee.");
    }
}
