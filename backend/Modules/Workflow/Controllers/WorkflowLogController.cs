using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/workflows/logs")]
public class WorkflowLogController : BaseWorkflowController
{
    private readonly IWorkflowLogService _logService;

    public WorkflowLogController(IWorkflowLogService logService)
    {
        _logService = logService;
    }

    [HttpGet("execution/{executionId:guid}")]
    public async Task<ActionResult<List<WorkflowExecutionLogDto>>> GetLogsByExecution(Guid executionId, CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var logs = await _logService.GetLogsByExecutionIdAsync(executionId, orgId, cancellationToken);
        return Ok(logs);
    }
}
