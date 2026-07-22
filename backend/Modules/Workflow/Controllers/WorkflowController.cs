using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/workflows")]
public class WorkflowController : BaseWorkflowController
{
    private readonly IWorkflowService _workflowService;

    public WorkflowController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpGet]
    public async Task<ActionResult<WorkflowPagedResult<WorkflowDto>>> GetWorkflows(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] WorkflowStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _workflowService.GetPagedAsync(orgId, pageNumber, pageSize, search, status, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkflowDto>> GetWorkflowById(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var workflow = await _workflowService.GetByIdAsync(id, orgId, cancellationToken);
        if (workflow == null) return NotFound();
        return Ok(workflow);
    }

    [HttpPost]
    public async Task<ActionResult<WorkflowDto>> CreateWorkflow([FromBody] CreateWorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId().ToString();
        var created = await _workflowService.CreateAsync(orgId, userId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetWorkflowById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WorkflowDto>> UpdateWorkflow(Guid id, [FromBody] UpdateWorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var updated = await _workflowService.UpdateAsync(id, orgId, dto, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWorkflow(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        await _workflowService.DeleteAsync(id, orgId, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/execute")]
    public async Task<ActionResult<WorkflowExecutionDto>> ExecuteManual(Guid id, [FromBody] ExecuteWorkflowManualRequest request, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var userId = GetCurrentUserId().ToString();
        var execution = await _workflowService.ExecuteManualAsync(id, orgId, userId, request, cancellationToken);
        return Ok(execution);
    }
}
