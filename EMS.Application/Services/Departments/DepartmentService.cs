using EMS.Application.DTOs.Department;
using EMS.Application.Exceptions;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EMS.Application.Services.Departments;

public sealed class DepartmentService : IDepartmentService
{
    private readonly IBaseRepository<Department> _repository;

    public DepartmentService(IBaseRepository<Department> repository)
    {
        _repository = repository;
    }

    public async Task<DepartmentDTO> CreateAsync(DepartmentDTO dto, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _repository.BeginTransactionAsync();
        try
        {
            if (await NameAlreadyExistsInOrgAsync(dto.OrganizationId, dto.Name, cancellationToken))
                throw new BusinessRuleException("A department with this name already exists in the organization.");

            if (!string.IsNullOrWhiteSpace(dto.Code) &&
                await CodeAlreadyExistsInOrgAsync(dto.OrganizationId, dto.Code, cancellationToken))
                throw new BusinessRuleException("A department with this code already exists in the organization.");

            if (dto.ParentDepartmentId is int parentId)
            {
                var parent = await _repository.GetByIdAsync(parentId);
                if (parent is null)
                    throw new BusinessRuleException("Parent department was not found.");

                if (parent.OrganizationId != dto.OrganizationId)
                    throw new BusinessRuleException("Parent department must belong to the same organization.");
            }

            var entity = new Department();
            MapDtoToEntity(dto, entity);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            await transaction.CommitAsync();
            dto.Id = entity.Id;
            return dto;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    private async Task<bool> NameAlreadyExistsInOrgAsync(int organizationId, string name, CancellationToken cancellationToken)
    {
        return await _repository.GetQueryable()
            .AnyAsync(d => d.OrganizationId == organizationId && d.Name == name, cancellationToken);
    }

    private async Task<bool> CodeAlreadyExistsInOrgAsync(int organizationId, string code, CancellationToken cancellationToken)
    {
        return await _repository.GetQueryable()
            .AnyAsync(d => d.OrganizationId == organizationId && d.Code == code, cancellationToken);
    }

    private static void MapDtoToEntity(DepartmentDTO dto, Department entity)
    {
        entity.OrganizationId = dto.OrganizationId;
        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.ParentDepartmentId = dto.ParentDepartmentId;
        entity.IsActive = dto.IsActive;
    }
}
