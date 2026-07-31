using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.AutomationStudio.Triggers.Interfaces;
using backend.Modules.AutomationStudio.ExecutionEngine.Interfaces;

namespace backend.Modules.AutomationStudio.Triggers.Services;

public class TriggerService : ITriggerService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWorkflowExecutionEngine _executionEngine;

    public TriggerService(
        ApplicationDbContext dbContext,
        IWorkflowExecutionEngine executionEngine)
    {
        _dbContext = dbContext;
        _executionEngine = executionEngine;
    }

    public async Task ProcessTriggerAsync(Guid organizationId, string triggerType, string payload)
    {
        // Find all active workflows for this organization that have this trigger type
        var workflowsToTrigger = await _dbContext.AutomationTriggers
            .Include(t => t.Workflow)
            .Where(t => t.OrganizationId == organizationId 
                     && t.Type == triggerType
                     && t.Workflow.IsActive 
                     && !t.Workflow.IsDeleted)
            .Select(t => t.WorkflowId)
            .Distinct()
            .ToListAsync();

        foreach (var workflowId in workflowsToTrigger)
        {
            // Execute the workflow asynchronously
            // In a highly scalable system, this would drop a message on a queue (e.g. RabbitMQ, Redis Pub/Sub)
            // For now, we await or fire-and-forget.
            _ = Task.Run(() => _executionEngine.ExecuteWorkflowAsync(workflowId, payload));
        }
    }
}
