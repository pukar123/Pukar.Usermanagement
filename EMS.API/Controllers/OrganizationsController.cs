using AutoMapper;
using EMS.API.Models.Organization;
using EMS.Application.DTOs.Organization;
using EMS.Application.Exceptions;
using EMS.Application.Services.Organizations;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public OrganizationsController(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrganizationResponseModel>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _organizationService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrganizationResponseModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _organizationService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationResponse>> Create(
        [FromBody] CreateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dto = _mapper.Map<OrganizationDTO>(request);
            var created = await _organizationService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<OrganizationResponse>(created));
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OrganizationResponseModel>> Update(
        int id,
        [FromBody] UpdateOrganizationRequestModel request,
        CancellationToken cancellationToken)
    {
        var updated = await _organizationService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _organizationService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
