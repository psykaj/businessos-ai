using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.SuccessTasks.DTOs;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.SuccessTasks.Controllers;

[Route("api/customer-success/tasks")]
public class SuccessTasksController : BaseCustomerSuccessController
{
    private readonly ISuccessTaskService _taskService;

    public SuccessTasksController(ISuccessTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult> GetPaged(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? assignedUserId,
        [FromQuery] string? status,
        [FromQuery] string? priority,
        [FromQuery] string? taskType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _taskService.GetPagedAsync(
            GetOrganizationId(), customerId, assignedUserId, status, priority, taskType, page, pageSize);

        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SuccessTaskDto>> GetById(Guid id)
    {
        var task = await _taskService.GetByIdAsync(GetOrganizationId(), id);
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<SuccessTaskDto>> Create([FromBody] CreateSuccessTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(GetOrganizationId(), dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SuccessTaskDto>> Update(Guid id, [FromBody] UpdateSuccessTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(GetOrganizationId(), id, dto);
        return Ok(task);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _taskService.DeleteTaskAsync(GetOrganizationId(), id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("auto-generate")]
    public async Task<ActionResult> AutoGenerate()
    {
        await _taskService.AutoGenerateRetentionTasksAsync(GetOrganizationId());
        return Ok(new { Message = "Automated customer retention tasks generated successfully." });
    }
}
