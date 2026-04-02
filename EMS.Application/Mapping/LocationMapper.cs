using EMS.Application.DTOs.Location;
using EMS.Domain.DbModels;

namespace EMS.Application.Mapping;

internal static class LocationMapper
{
    public static Location ToEntity(CreateLocationRequestModel request)
    {
        return new Location
        {
            OrganizationId = request.OrganizationId,
            Name = request.Name,
            Line1 = request.Line1,
            Line2 = request.Line2,
            City = request.City,
            Region = request.Region,
            PostalCode = request.PostalCode,
            Country = request.Country,
            IsActive = request.IsActive
        };
    }

    public static void ApplyUpdate(Location entity, UpdateLocationRequestModel request)
    {
        entity.Name = request.Name;
        entity.Line1 = request.Line1;
        entity.Line2 = request.Line2;
        entity.City = request.City;
        entity.Region = request.Region;
        entity.PostalCode = request.PostalCode;
        entity.Country = request.Country;
        entity.IsActive = request.IsActive;
    }

    public static LocationResponseModel ToResponse(Location entity)
    {
        return new LocationResponseModel
        {
            Id = entity.Id,
            OrganizationId = entity.OrganizationId,
            Name = entity.Name,
            Line1 = entity.Line1,
            Line2 = entity.Line2,
            City = entity.City,
            Region = entity.Region,
            PostalCode = entity.PostalCode,
            Country = entity.Country,
            IsActive = entity.IsActive
        };
    }
}
