using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.Interfaces;

public interface IWorkflowTriggerDispatcher
{
    Task DispatchTriggerAsync(Guid organizationId, TriggerType triggerType, Dictionary<string, string> payload, CancellationToken cancellationToken = default);
}
