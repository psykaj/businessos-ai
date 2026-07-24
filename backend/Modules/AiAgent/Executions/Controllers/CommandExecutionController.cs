using backend.Modules.AiAgent.Controllers;
using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.Executions.Controllers;

[Route("api/ai-agent/executions")]
public class CommandExecutionController : BaseAiAgentController
{
    private readonly ICommandExecutionRepository _executionRepo;

    public CommandExecutionController(ICommandExecutionRepository executionRepo)
    {
        _executionRepo = executionRepo;
    }

    [HttpGet]
    public async Task<ActionResult> GetExecutions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        [FromQuery] string? tool = null,
        CancellationToken cancellationToken = default)
    {
        var orgId = GetOrganizationId();
        var (items, totalCount) = await _executionRepo.GetPagedAsync(orgId, page, pageSize, status, tool, cancellationToken);
        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CommandExecution>> GetExecutionById(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var execution = await _executionRepo.GetByIdAsync(id, orgId, cancellationToken);
        if (execution == null) return NotFound(new { Message = "Execution record not found." });
        return Ok(execution);
    }
}
