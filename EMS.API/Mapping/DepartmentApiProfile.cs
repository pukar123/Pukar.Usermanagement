using AutoMapper;
using EMS.API.Models.Department;
using EMS.Application.DTOs.Department;

namespace EMS.API.Mapping;

public sealed class DepartmentApiProfile : Profile
{
    public DepartmentApiProfile()
    {
        CreateMap<CreateDepartmentRequest, DepartmentDTO>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<DepartmentDTO, DepartmentResponse>();
    }
}
