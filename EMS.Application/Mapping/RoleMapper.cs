using RoleDtos = EMS.Application.DTOs.Role;
using RoleEntity = EMS.Domain.DbModels.Role;

namespace EMS.Application.Mapping;

internal static class RoleMapper
{
    public static RoleEntity ToEntity(RoleDtos.CreateRoleRequestModel request)
    {
        return new RoleEntity
        {
            OrganizationId = request.OrganizationId,
            Name = request.Name,
            Code = request.Code,
            IsActive = request.IsActive
        };
    }

    public static void ApplyUpdate(RoleEntity entity, RoleDtos.UpdateRoleRequestModel request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.IsActive = request.IsActive;
    }

    public static RoleDtos.RoleResponseModel ToResponse(RoleEntity entity)
    {
        return new RoleDtos.RoleResponseModel
        {
            Id = entity.Id,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name,
            Code = entity.Code,
            IsActive = entity.IsActive
        };
    }
}
