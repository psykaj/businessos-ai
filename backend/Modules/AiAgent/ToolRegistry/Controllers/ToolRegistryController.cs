using backend.Modules.AiAgent.Controllers;
using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AiAgent.ToolRegistry.Controllers;

[Route("api/ai-agent/tools")]
public class ToolRegistryController : BaseAiAgentController
{
    private readonly IToolRegistry _toolRegistry;

    public ToolRegistryController(IToolRegistry toolRegistry)
    {
        _toolRegistry = toolRegistry;
    }

    [HttpGet]
    public IActionResult GetAllTools()
    {
        var tools = _toolRegistry.GetAllTools().Select(t => new
        {
            t.Name,
            t.Description,
            t.Category,
            t.RequiredPermissions,
            t.IsDestructive,
            t.ParametersSchema
        });
        return Ok(tools);
    }

    [HttpGet("{name}")]
    public IActionResult GetToolByName(string name)
    {
        var tool = _toolRegistry.GetTool(name);
        if (tool == null) return NotFound(new { Message = $"Tool '{name}' not found." });
        return Ok(new
        {
            tool.Name,
            tool.Description,
            tool.Category,
            tool.RequiredPermissions,
            tool.IsDestructive,
            tool.ParametersSchema
        });
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncTools(CancellationToken cancellationToken)
    {
        await _toolRegistry.SyncToolDefinitionsAsync(cancellationToken);
        return Ok(new { Message = "Tool definitions synchronized successfully with database." });
    }
}
