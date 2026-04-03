using EMS.Application.DTOs.Role;
using EMS.Application.Exceptions;
using EMS.Application.Mapping;
using EMS.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using JobEntity = EMS.Domain.DbModels.Job;
using RoleEntity = EMS.Domain.DbModels.Role;

namespace EMS.Application.Services.Roles;

public sealed class RoleService : IRoleService
{
    private readonly IBaseRepository<RoleEntity> _repository;
    private readonly IBaseRepository<JobEntity> _jobRepository;

    public RoleService(IBaseRepository<RoleEntity> repository, IBaseRepository<JobEntity> jobRepository)
    {
        _repository = repository;
        _jobRepository = jobRepository;
    }

    public async Task<RoleResponseModel> CreateAsync(CreateRoleRequestModel request, CancellationToken cancellationToken = default)
    {
        if (await NameExistsInOrgAsync(request.OrganizationId, request.Name, cancellationToken))
            throw new BusinessRuleException("A role with this name already exists in the organization.");

        if (!string.IsNullOrWhiteSpace(request.Code) &&
            await CodeExistsInOrgAsync(request.OrganizationId, request.Code, cancellationToken))
            throw new BusinessRuleException("A role with this code already exists in the organization.");

        var entity = RoleMapper.ToEntity(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return RoleMapper.ToResponse(entity);
    }

    public async Task<RoleResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : RoleMapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<RoleResponseModel>> GetByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetQueryable()
            .Where(r => r.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
        return list.Select(RoleMapper.ToResponse).ToList();
    }

    public async Task<RoleResponseModel?> UpdateAsync(int id, UpdateRoleRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return null;

        if (await NameExistsInOrgAsync(entity.OrganizationId, request.Name, cancellationToken, id))
            throw new BusinessRuleException("A role with this name already exists in the organization.");

        if (!string.IsNullOrWhiteSpace(request.Code) &&
            await CodeExistsInOrgAsync(entity.OrganizationId, request.Code, cancellationToken, id))
            throw new BusinessRuleException("A role with this code already exists in the organization.");

        RoleMapper.ApplyUpdate(entity, request);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return RoleMapper.ToResponse(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return false;

        if (await _jobRepository.GetQueryable().AnyAsync(j => j.RoleId == id, cancellationToken))
            throw new BusinessRuleException("Cannot delete a role that has jobs. Remove or reassign jobs first.");

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
        return true;
    }

    private async Task<bool> NameExistsInOrgAsync(int organizationId, string name, CancellationToken cancellationToken, int? exceptId = null)
    {
        var q = _repository.GetQueryable().Where(r => r.OrganizationId == organizationId && r.Name == name);
        if (exceptId is int id)
            q = q.Where(r => r.Id != id);
        return await q.AnyAsync(cancellationToken);
    }

    private async Task<bool> CodeExistsInOrgAsync(int organizationId, string code, CancellationToken cancellationToken, int? exceptId = null)
    {
        var q = _repository.GetQueryable().Where(r => r.OrganizationId == organizationId && r.Code == code);
        if (exceptId is int id)
            q = q.Where(r => r.Id != id);
        return await q.AnyAsync(cancellationToken);
    }
}
