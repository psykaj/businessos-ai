using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/tasks")]
public class TasksController : BaseCrmController
{
    private readonly ICrmTaskService _service;

    public TasksController(ICrmTaskService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.CreateTaskAsync(dto, orgId);
        return CreatedAtAction(nameof(GetTask), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponseDto>> GetTask(Guid id)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetTaskAsync(id, orgId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskResponseDto>>> GetAllTasks()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetAllTasksAsync(orgId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponseDto>> UpdateTask(Guid id, UpdateTaskDto dto)
    {
        var orgId = GetOrganizationId();
        var result = await _service.UpdateTaskAsync(id, dto, orgId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.DeleteTaskAsync(id, orgId);
        return NoContent();
    }
}
