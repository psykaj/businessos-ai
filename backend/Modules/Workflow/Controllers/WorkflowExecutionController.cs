using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/workflows/executions")]
public class WorkflowExecutionController : BaseWorkflowController
{
    private readonly IWorkflowLogService _logService;

    public WorkflowExecutionController(IWorkflowLogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<WorkflowPagedResult<WorkflowExecutionDto>>> GetExecutions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] WorkflowExecutionStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _logService.GetExecutionsByOrganizationIdAsync(orgId, pageNumber, pageSize, status, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkflowExecutionDto>> GetExecutionById(Guid id, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var execution = await _logService.GetExecutionByIdAsync(id, orgId, cancellationToken);
        if (execution == null) return NotFound();
        return Ok(execution);
    }

    [HttpGet("workflow/{workflowId:guid}")]
    public async Task<ActionResult<WorkflowPagedResult<WorkflowExecutionDto>>> GetExecutionsByWorkflow(
        Guid workflowId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var result = await _logService.GetExecutionsByWorkflowIdAsync(workflowId, orgId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }
}
