using EMS.Application.DTOs.Role;
using EMS.Application.Exceptions;
using EMS.Application.Services.Roles;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleResponseModel>>> GetByOrganization(
        [FromQuery] int organizationId,
        CancellationToken cancellationToken)
    {
        var items = await _roleService.GetByOrganizationAsync(organizationId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleResponseModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _roleService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponseModel>> Create(
        [FromBody] CreateRoleRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            var created = await _roleService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoleResponseModel>> Update(
        int id,
        [FromBody] UpdateRoleRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _roleService.UpdateAsync(id, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _roleService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
