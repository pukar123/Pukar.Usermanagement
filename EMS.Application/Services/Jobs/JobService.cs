using EMS.Application.DTOs.Job;
using EMS.Application.Exceptions;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using JobEntity = EMS.Domain.DbModels.Job;
using RoleEntity = EMS.Domain.DbModels.Role;

namespace EMS.Application.Services.Jobs;

public sealed class JobService : IJobService
{
    private readonly IBaseRepository<JobEntity> _repository;
    private readonly IBaseRepository<RoleEntity> _roleRepository;
    private readonly IBaseRepository<Employee> _employeeRepository;

    public JobService(
        IBaseRepository<JobEntity> repository,
        IBaseRepository<RoleEntity> roleRepository,
        IBaseRepository<Employee> employeeRepository)
    {
        _repository = repository;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<JobResponseModel> CreateAsync(CreateJobRequestModel request, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role is null)
            throw new BusinessRuleException("Role was not found.");

        if (await NameExistsInRoleAsync(request.RoleId, request.Name, cancellationToken))
            throw new BusinessRuleException("A job with this name already exists under the role.");

        if (!string.IsNullOrWhiteSpace(request.Code) &&
            await CodeExistsInRoleAsync(request.RoleId, request.Code, cancellationToken))
            throw new BusinessRuleException("A job with this code already exists under the role.");

        var entity = JobMapper.ToEntity(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return JobMapper.ToResponse(entity);
    }

    public async Task<JobResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : JobMapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<JobResponseModel>> GetByRoleAsync(int roleId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetQueryable()
            .Where(j => j.RoleId == roleId)
            .ToListAsync(cancellationToken);
        return list.Select(JobMapper.ToResponse).ToList();
    }

    public async Task<JobResponseModel?> UpdateAsync(int id, UpdateJobRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return null;

        if (await NameExistsInRoleAsync(entity.RoleId, request.Name, cancellationToken, id))
            throw new BusinessRuleException("A job with this name already exists under the role.");

        if (!string.IsNullOrWhiteSpace(request.Code) &&
            await CodeExistsInRoleAsync(entity.RoleId, request.Code, cancellationToken, id))
            throw new BusinessRuleException("A job with this code already exists under the role.");

        JobMapper.ApplyUpdate(entity, request);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return JobMapper.ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return false;

        if (await _employeeRepository.GetQueryable().AnyAsync(e => e.JobId == id, cancellationToken))
            throw new BusinessRuleException("Cannot delete a job that is assigned to employees. Unassign employees first.");

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
        return true;
    }

    private async Task<bool> NameExistsInRoleAsync(int roleId, string name, CancellationToken cancellationToken, int? exceptId = null)
    {
        var q = _repository.GetQueryable().Where(j => j.RoleId == roleId && j.Name == name);
        if (exceptId is int eid)
            q = q.Where(j => j.Id != eid);
        return await q.AnyAsync(cancellationToken);
    }

    private async Task<bool> CodeExistsInRoleAsync(int roleId, string code, CancellationToken cancellationToken, int? exceptId = null)
    {
        var q = _repository.GetQueryable().Where(j => j.RoleId == roleId && j.Code == code);
        if (exceptId is int eid)
            q = q.Where(j => j.Id != eid);
        return await q.AnyAsync(cancellationToken);
    }
}
