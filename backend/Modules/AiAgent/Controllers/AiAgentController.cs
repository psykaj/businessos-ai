using backend.Modules.AiAgent.Executions.DTOs;
using backend.Modules.AiAgent.Executions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.Controllers;

[Route("api/ai-agent")]
public class AiAgentController : BaseAiAgentController
{
    private readonly IAiExecutionEngine _executionEngine;

    public AiAgentController(IAiExecutionEngine executionEngine)
    {
        _executionEngine = executionEngine;
    }

    [HttpPost("execute")]
    public async Task<ActionResult<ExecutionResponseDto>> ExecuteCommand([FromBody] ExecuteCommandRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Command))
        {
            return BadRequest(new { Message = "Command text is required." });
        }

        var orgId = GetOrganizationId();
        var userId = GetUserId();

        var response = await _executionEngine.ExecuteCommandAsync(orgId, userId, request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("status")]
    public IActionResult GetAgentStatus()
    {
        return Ok(new
        {
            Status = "Operational",
            Module = "AI Business Agent & Execution Engine",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow
        });
    }
}
