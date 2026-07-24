using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Modules.Workflow.Constants;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class WorkflowTool : ITool
{
    private readonly ApplicationDbContext _context;

    public WorkflowTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "WorkflowTool";
    public string Description => "Workflow Automation Tool: list and trigger automated workflows.";
    public string Category => "Workflow";
    public string[] RequiredPermissions => new[] { "Workflow.Execute" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"workflowName\":{\"type\":\"string\"},\"action\":{\"type\":\"string\",\"enum\":[\"trigger\",\"list\"]}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var action = parameters.TryGetValue("action", out var actObj) ? actObj?.ToString()?.ToLower() : "trigger";

        if (action == "list")
        {
            var activeWorkflows = await _context.Workflows
                .AsNoTracking()
                .Where(w => w.OrganizationId == context.OrganizationId && w.Status == WorkflowStatus.Active && !w.IsDeleted)
                .Select(w => new { w.Id, w.Name, Status = w.Status.ToString() })
                .ToListAsync(cancellationToken);

            return ToolResult.Ok($"Found {activeWorkflows.Count} active workflows.", activeWorkflows);
        }

        var workflow = await _context.Workflows
            .FirstOrDefaultAsync(w => w.OrganizationId == context.OrganizationId && w.Status == WorkflowStatus.Active && !w.IsDeleted, cancellationToken);

        if (workflow == null)
        {
            return ToolResult.Fail("No active workflow found to trigger.");
        }

        var execution = new Workflow.Entities.WorkflowExecution
        {
            Id = Guid.NewGuid(),
            OrganizationId = context.OrganizationId,
            WorkflowId = workflow.Id,
            Status = WorkflowExecutionStatus.Completed,
            StartedAt = DateTime.UtcNow,
            FinishedAt = DateTime.UtcNow,
            ExecutedBy = "AI Business Agent",
            TriggerDataJson = "{\"triggeredBy\":\"AI Business Agent\"}",
            ContextDataJson = "{}"
        };

        await _context.WorkflowExecutions.AddAsync(execution, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToolResult.Ok($"Workflow '{workflow.Name}' triggered successfully.", new { execution.Id, WorkflowName = workflow.Name, Status = execution.Status.ToString() });
    }
}
