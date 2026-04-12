using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pukar.Shared;
using Pukar.Usermanagement.Application.DTOs.Admin;
using Pukar.Usermanagement.Application.Services.Admin;

namespace Pukar.Usermanagement.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminManagementService _adminManagement;

    public AdminController(IAdminManagementService adminManagement)
    {
        _adminManagement = adminManagement;
    }

    [HttpGet("roles")]
    [ProducesResponseType(typeof(IReadOnlyList<RoleResponseModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RoleResponseModel>>> ListRoles(CancellationToken cancellationToken)
    {
        var roles = await _adminManagement.ListRolesAsync(cancellationToken);
        return Ok(roles);
    }

    [HttpPost("roles")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var roleName = await _adminManagement.CreateRoleAsync(request, cancellationToken);
            return Ok(new { role = roleName });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("roles/{roleId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(int roleId, CancellationToken cancellationToken)
    {
        try
        {
            await _adminManagement.DeleteRoleAsync(roleId, cancellationToken);
            return NoContent();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(IReadOnlyList<AdminUserResponseModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AdminUserResponseModel>>> ListUsers(CancellationToken cancellationToken)
    {
        var users = await _adminManagement.ListUsersAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("users/{userId:int}")]
    [ProducesResponseType(typeof(AdminUserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdminUserResponseModel>> GetUser(int userId, CancellationToken cancellationToken)
    {
        var user = await _adminManagement.GetUserByIdAsync(userId, cancellationToken);
        if (user is null)
            return NotFound();
        return Ok(user);
    }

    [HttpPut("users/{userId:int}")]
    [ProducesResponseType(typeof(AdminUserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdminUserResponseModel>> UpdateUser(
        int userId,
        [FromBody] UpdateUserRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _adminManagement.UpdateUserAsync(userId, request, cancellationToken);
            return Ok(updated);
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("users")]
    [ProducesResponseType(typeof(AdminUserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AdminUserResponseModel>> CreateUser([FromBody] CreateUserRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _adminManagement.CreateUserAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (DuplicateEmailException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("users/{userId:int}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRole(int userId, string roleName, CancellationToken cancellationToken)
    {
        try
        {
            await _adminManagement.AssignRoleAsync(userId, roleName, cancellationToken);
            return NoContent();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("users/{userId:int}/roles/{roleName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveRole(int userId, string roleName, CancellationToken cancellationToken)
    {
        try
        {
            await _adminManagement.RemoveRoleAsync(userId, roleName, cancellationToken);
            return NoContent();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
