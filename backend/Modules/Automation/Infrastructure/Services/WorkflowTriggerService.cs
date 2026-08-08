using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Automation.Application.Interfaces;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Infrastructure.Services;

public class WorkflowTriggerService : IWorkflowTriggerService
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkflowEngine _engine;

    public WorkflowTriggerService(ApplicationDbContext context, IWorkflowEngine engine)
    {
        _context = context;
        _engine = engine;
    }

    public async Task TriggerEventAsync(WorkflowTriggerType triggerType, Guid organizationId, object eventData)
    {
        var activeWorkflows = await _context.AiEngineWorkflows
            .Include(w => w.Steps)
            .Where(w => w.OrganizationId == organizationId 
                     && w.IsActive 
                     && w.TriggerType == triggerType.ToString()
                     && !w.IsDeleted)
            .OrderByDescending(w => w.Priority)
            .ToListAsync();

        foreach (var workflow in activeWorkflows)
        {
            // Execute in a fire-and-forget or background manner if needed
            // For now, executing directly in the request scope, 
            // but in production this should be enqueued via a background task system (e.g. Hangfire or simple IHostedService channel)
            await _engine.ExecuteWorkflowAsync(workflow, organizationId, eventData);
        }
    }
}
