using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/activities")]
public class ActivitiesController : BaseCrmController
{
    private readonly ICrmActivityService _service;

    public ActivitiesController(ICrmActivityService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ActivityResponseDto>> CreateActivity(CreateActivityDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateActivityAsync(dto, orgId);
        return CreatedAtAction(nameof(GetActivity), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityResponseDto>> GetActivity(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetActivityAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ActivityResponseDto>>> GetAllActivities()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllActivitiesAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActivityResponseDto>> UpdateActivity(Guid id, UpdateActivityDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateActivityAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteActivityAsync(id, orgId);
        return NoContent();
    }
}
