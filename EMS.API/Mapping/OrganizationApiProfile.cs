using AutoMapper;
using EMS.API.Models.Organization;
using EMS.Application.DTOs.Organization;

namespace EMS.API.Mapping;

public sealed class OrganizationApiProfile : Profile
{
    public OrganizationApiProfile()
    {
        CreateMap<CreateOrganizationRequest, OrganizationDTO>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<OrganizationDTO, OrganizationResponse>();
    }
}
