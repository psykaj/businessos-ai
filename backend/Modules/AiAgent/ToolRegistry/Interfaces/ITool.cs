using backend.Modules.AiAgent.ToolRegistry.Models;

namespace backend.Modules.AiAgent.ToolRegistry.Interfaces;

public interface ITool
{
    string Name { get; }
    string Description { get; }
    string Category { get; }
    string[] RequiredPermissions { get; }
    bool IsDestructive { get; }
    string ParametersSchema { get; }

    Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
}
