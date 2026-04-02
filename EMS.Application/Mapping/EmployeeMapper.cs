using EMS.Application.DTOs.Employee;
using EMS.Domain.DbModels;

namespace EMS.Application.Mapping;

internal static class EmployeeMapper
{
    public static Employee ToEntity(CreateEmployeeRequestModel request)
    {
        return new Employee
        {
            OrganizationId = request.OrganizationId,
            DepartmentId = request.DepartmentId,
            LocationId = request.LocationId,
            ManagerId = request.ManagerId,
            EmployeeNumber = request.EmployeeNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            JobTitle = request.JobTitle,
            DateOfBirth = request.DateOfBirth,
            DateJoined = request.DateJoined,
            EmploymentStatus = request.EmploymentStatus,
            IsActive = request.IsActive
        };
    }

    public static void ApplyUpdate(Employee entity, UpdateEmployeeRequestModel request)
    {
        entity.OrganizationId = request.OrganizationId;
        entity.DepartmentId = request.DepartmentId;
        entity.LocationId = request.LocationId;
        entity.ManagerId = request.ManagerId;
        entity.EmployeeNumber = request.EmployeeNumber;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Email = request.Email;
        entity.PhoneNumber = request.PhoneNumber;
        entity.JobTitle = request.JobTitle;
        entity.DateOfBirth = request.DateOfBirth;
        entity.DateJoined = request.DateJoined;
        entity.EmploymentStatus = request.EmploymentStatus;
        entity.IsActive = request.IsActive;
    }

    public static EmployeeResponseModel ToResponse(Employee entity)
    {
        return new EmployeeResponseModel
        {
            Id = entity.Id,
            OrganizationId = entity.OrganizationId,
            DepartmentId = entity.DepartmentId,
            LocationId = entity.LocationId,
            ManagerId = entity.ManagerId,
            EmployeeNumber = entity.EmployeeNumber,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            JobTitle = entity.JobTitle,
            DateOfBirth = entity.DateOfBirth,
            DateJoined = entity.DateJoined,
            EmploymentStatus = entity.EmploymentStatus,
            IsActive = entity.IsActive,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc
        };
    }
}
