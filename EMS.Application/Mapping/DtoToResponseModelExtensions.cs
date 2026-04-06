using EMS.Application.DTOs.Department;
using EMS.Application.DTOs.Organization;

namespace EMS.Application.Mapping;

/// <summary>
/// Maps application DTOs to HTTP response models (e.g. for 201 Created). Public for use from EMS.API.
/// </summary>
public static class DtoToResponseModelExtensions
{
    public static DepartmentResponseModel ToResponseModel(this DepartmentDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new DepartmentResponseModel
        {
            Id = dto.Id,
            OrganizationId = dto.OrganizationId,
            Name = dto.Name,
            Code = dto.Code,
            ParentDepartmentId = dto.ParentDepartmentId,
            IsActive = dto.IsActive,
        };
    }

    public static OrganizationResponseModel ToResponseModel(this OrganizationDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new OrganizationResponseModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            IsActive = dto.IsActive,
            Description = dto.Description,
            Motto = dto.Motto,
            LogoRelativePath = dto.LogoRelativePath,
        };
    }
}
