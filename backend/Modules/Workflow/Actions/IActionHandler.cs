using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Actions;

public interface IActionHandler
{
    ActionType ActionType { get; }
    Task<ActionResult> ExecuteAsync(Guid organizationId, string actionConfiguration, Dictionary<string, string> context, CancellationToken cancellationToken = default);
}

public record ActionResult(
    bool Success,
    string OutputJson,
    string? ErrorMessage = null,
    Dictionary<string, string>? OutputContext = null
);
