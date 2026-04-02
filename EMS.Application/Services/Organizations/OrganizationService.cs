using EMS.Application.DTOs.Organization;
using EMS.Application.Mapping;
using EMS.Domain.DbModels;
using EMS.Domain.Repositories.Interface;

namespace EMS.Application.Services.Organizations;

public sealed class OrganizationService : IOrganizationService
{
    private readonly IBaseRepository<Organization> _repository;

    public OrganizationService(IBaseRepository<Organization> repository)
    {
        _repository = repository;
    }

    public async Task<OrganizationResponseModel> CreateAsync(CreateOrganizationRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = OrganizationMapper.ToEntity(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return OrganizationMapper.ToResponse(entity);
    }

    public async Task<OrganizationResponseModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : OrganizationMapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<OrganizationResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync();
        return list.Select(OrganizationMapper.ToResponse).ToList();
    }

    public async Task<OrganizationResponseModel?> UpdateAsync(int id, UpdateOrganizationRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            return null;

        OrganizationMapper.ApplyUpdate(entity, request);
        _repository.Update(entity);
        await _repository.SaveChangesAsync();
        return OrganizationMapper.ToResponse(entity);
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
