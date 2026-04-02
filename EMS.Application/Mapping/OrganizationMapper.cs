using EMS.Application.DTOs.Organization;
using EMS.Domain.DbModels;

namespace EMS.Application.Mapping;

internal static class OrganizationMapper
{
    public static void ApplyUpdate(Organization entity, UpdateOrganizationRequestModel request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.IsActive = request.IsActive;
    }

    public static OrganizationResponseModel ToResponse(Organization entity)
    {
        return new OrganizationResponseModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            IsActive = entity.IsActive
        };
    }
}
