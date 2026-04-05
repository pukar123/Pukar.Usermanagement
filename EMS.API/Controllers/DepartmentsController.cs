using AutoMapper;
using EMS.API.Models.Department;
using EMS.Application.DTOs.Department;
using EMS.Domain.Helpers;
using EMS.Application.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    public DepartmentsController(IDepartmentService departmentService, IMapper mapper)
    {
        _departmentService = departmentService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DepartmentResponseModel>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _departmentService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentResponseModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _departmentService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponse>> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dto = _mapper.Map<DepartmentDTO>(request);
            var created = await _departmentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<DepartmentResponse>(created));
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DepartmentResponseModel>> Update(
        int id,
        [FromBody] UpdateDepartmentRequestModel request,
        CancellationToken cancellationToken)
    {
        var updated = await _departmentService.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _departmentService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
