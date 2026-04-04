using JobPositionDtos = EMS.Application.DTOs.JobPosition;
using JobPositionEntity = EMS.Domain.DbModels.JobPosition;

namespace EMS.Application.Mapping;

internal static class JobPositionMapper
{
    public static JobPositionEntity ToEntity(JobPositionDtos.CreateJobPositionRequestModel request)
    {
        return new JobPositionEntity
        {
            OrganizationId = request.OrganizationId,
            Title = request.Title,
            Description = request.Description,
            Code = request.Code,
            IsActive = request.IsActive
        };
    }

    public static void ApplyUpdate(JobPositionEntity entity, JobPositionDtos.UpdateJobPositionRequestModel request)
    {
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Code = request.Code;
        entity.IsActive = request.IsActive;
    }

    public static JobPositionDtos.JobPositionResponseModel ToResponse(JobPositionEntity entity)
    {
        return new JobPositionDtos.JobPositionResponseModel
        {
            Id = entity.Id,
            OrganizationId = entity.OrganizationId,
            Title = entity.Title,
            Description = entity.Description,
            Code = entity.Code,
            IsActive = entity.IsActive
        };
    }
}
