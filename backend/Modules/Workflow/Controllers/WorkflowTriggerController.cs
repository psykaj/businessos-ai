using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Workflow.Controllers;

[Route("api/workflows/triggers")]
public class WorkflowTriggerController : BaseWorkflowController
{
    private readonly IWorkflowTriggerDispatcher _dispatcher;

    public WorkflowTriggerController(IWorkflowTriggerDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("types")]
    public ActionResult<IEnumerable<string>> GetTriggerTypes()
    {
        var types = Enum.GetNames<TriggerType>();
        return Ok(types);
    }

    [HttpPost("test-dispatch")]
    public async Task<IActionResult> TestDispatch(
        [FromQuery] TriggerType triggerType,
        [FromBody] Dictionary<string, string> payload,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        await _dispatcher.DispatchTriggerAsync(orgId, triggerType, payload, cancellationToken);
        return Ok(new { Message = $"Trigger {triggerType} dispatched for Organization {orgId}." });
    }
}
