using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.Interfaces;
using backend.Modules.Workflow.Triggers;
using Microsoft.Extensions.Logging;

namespace backend.Modules.Workflow.Services;

public class WorkflowTriggerDispatcher : IWorkflowTriggerDispatcher
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkflowExecutionEngine _executionEngine;
    private readonly TriggerRegistry _triggerRegistry;
    private readonly ILogger<WorkflowTriggerDispatcher> _logger;

    public WorkflowTriggerDispatcher(
        IWorkflowRepository workflowRepository,
        IWorkflowExecutionEngine executionEngine,
        TriggerRegistry triggerRegistry,
        ILogger<WorkflowTriggerDispatcher> logger)
    {
        _workflowRepository = workflowRepository;
        _executionEngine = executionEngine;
        _triggerRegistry = triggerRegistry;
        _logger = logger;
    }

    public async Task DispatchTriggerAsync(Guid organizationId, TriggerType triggerType, Dictionary<string, string> payload, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Dispatching trigger {TriggerType} for Organization {OrgId}", triggerType, organizationId);

        var workflows = await _workflowRepository.GetActiveWorkflowsByTriggerTypeAsync(organizationId, triggerType, cancellationToken);
        if (workflows.Count == 0)
        {
            _logger.LogInformation("No active workflows found for trigger {TriggerType} in Organization {OrgId}", triggerType, organizationId);
            return;
        }

        var handler = _triggerRegistry.GetHandler(triggerType);

        foreach (var workflow in workflows)
        {
            try
            {
                var triggerConfig = workflow.Trigger?.TriggerConfiguration ?? "{}";
                var context = handler != null
                    ? await handler.ExtractContextAsync(triggerConfig, payload)
                    : new Dictionary<string, string>(payload);

                // Add standard metadata to context
                context["OrganizationId"] = organizationId.ToString();
                context["WorkflowId"] = workflow.Id.ToString();
                context["TriggerType"] = triggerType.ToString();
                context["TriggeredAt"] = DateTime.UtcNow.ToString("O");

                _ = Task.Run(() => _executionEngine.ExecuteWorkflowAsync(workflow, context, "SystemTrigger", cancellationToken), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error dispatching trigger {TriggerType} for Workflow {WorkflowId}", triggerType, workflow.Id);
            }
        }
    }
}
