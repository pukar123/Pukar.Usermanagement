using EMS.Application.DTOs.Job;
using EMS.Application.Exceptions;
using EMS.Application.Services.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JobResponseModel>>> GetByRole(
        [FromQuery] int roleId,
        CancellationToken cancellationToken)
    {
        var items = await _jobService.GetByRoleAsync(roleId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobResponseModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _jobService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<JobResponseModel>> Create(
        [FromBody] CreateJobRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            var created = await _jobService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<JobResponseModel>> Update(
        int id,
        [FromBody] UpdateJobRequestModel request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _jobService.UpdateAsync(id, request, cancellationToken);
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
            var deleted = await _jobService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
