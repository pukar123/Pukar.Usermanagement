using EMS.Application.DTOs.Department;
using EMS.Domain.DbModels;

namespace EMS.Application.Mapping;

internal static class DepartmentMapper
{
    public static void ApplyUpdate(Department entity, UpdateDepartmentRequestModel request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.ParentDepartmentId = request.ParentDepartmentId;
        entity.IsActive = request.IsActive;
    }

    public static DepartmentResponseModel ToResponse(Department entity)
    {
        return new DepartmentResponseModel
        {
            Id = entity.Id,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name,
            Code = entity.Code,
            ParentDepartmentId = entity.ParentDepartmentId,
            IsActive = entity.IsActive
        };
    }
}
