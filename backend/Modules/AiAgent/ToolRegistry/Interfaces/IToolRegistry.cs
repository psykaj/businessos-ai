using backend.Modules.AiAgent.Entities;
using backend.Modules.AiAgent.ToolRegistry.Models;

namespace backend.Modules.AiAgent.ToolRegistry.Interfaces;

public interface IToolRegistry
{
    void RegisterTool(ITool tool);
    ITool? GetTool(string name);
    IReadOnlyCollection<ITool> GetAllTools();
    Task SyncToolDefinitionsAsync(CancellationToken cancellationToken = default);
    Task<ToolResult> ExecuteToolAsync(
        string toolName,
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
}
