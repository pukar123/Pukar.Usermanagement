using JobDtos = EMS.Application.DTOs.Job;
using JobEntity = EMS.Domain.DbModels.Job;

namespace EMS.Application.Mapping;

internal static class JobMapper
{
    public static JobEntity ToEntity(JobDtos.CreateJobRequestModel request)
    {
        return new JobEntity
        {
            RoleId = request.RoleId,
            Name = request.Name,
            Code = request.Code,
            IsActive = request.IsActive
        };
    }

    public static void ApplyUpdate(JobEntity entity, JobDtos.UpdateJobRequestModel request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.IsActive = request.IsActive;
    }

    public static JobDtos.JobResponseModel ToResponse(JobEntity entity)
    {
        return new JobDtos.JobResponseModel
        {
            Id = entity.Id,
            RoleId = entity.RoleId,
            Name = entity.Name,
            Code = entity.Code,
            IsActive = entity.IsActive
        };
    }
}
