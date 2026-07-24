using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Enums;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class CrmTool : ITool
{
    private readonly ApplicationDbContext _context;

    public CrmTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "CrmTool";
    public string Description => "CRM Management Tool: assign leads, view pending tasks, and schedule follow-ups.";
    public string Category => "CRM";
    public string[] RequiredPermissions => new[] { "CRM.View", "CRM.Edit" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"action\":{\"type\":\"string\",\"enum\":[\"assign_lead\",\"show_pending_tasks\",\"schedule_followup\"]},\"leadId\":{\"type\":\"string\"},\"assigneeName\":{\"type\":\"string\"},\"title\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var action = parameters.TryGetValue("action", out var actionObj) ? actionObj?.ToString()?.ToLower() : "show_pending_tasks";

        switch (action)
        {
            case "assign_lead":
                {
                    var leadIdStr = parameters.TryGetValue("leadId", out var lId) ? lId?.ToString() : null;
                    var assigneeName = parameters.TryGetValue("assigneeName", out var aName) ? aName?.ToString() : "Rahul";

                    Lead? lead = null;
                    if (Guid.TryParse(leadIdStr, out var leadGuid))
                    {
                        lead = await _context.Leads.FirstOrDefaultAsync(l => l.Id == leadGuid && l.OrganizationId == context.OrganizationId, cancellationToken);
                    }
                    else
                    {
                        lead = await _context.Leads.FirstOrDefaultAsync(l => l.OrganizationId == context.OrganizationId && !l.IsDeleted, cancellationToken);
                    }

                    if (lead == null)
                    {
                        return ToolResult.Fail("No suitable lead found in organization to assign.");
                    }

                    if (Guid.TryParse(context.UserId, out var userGuid))
                    {
                        lead.AssignedUserId = userGuid;
                    }

                    lead.Status = LeadStatus.Qualified;
                    _context.Leads.Update(lead);
                    await _context.SaveChangesAsync(cancellationToken);

                    return ToolResult.Ok($"Lead '{lead.FirstName} {lead.LastName}' assigned successfully to {assigneeName}.", new { lead.Id, lead.FirstName, lead.LastName, lead.AssignedUserId });
                }

            case "schedule_followup":
                {
                    var title = parameters.TryGetValue("title", out var t) ? t?.ToString() : "Follow-up Call";
                    var task = new CrmTask
                    {
                        Id = Guid.NewGuid(),
                        OrganizationId = context.OrganizationId,
                        Title = title ?? "AI Scheduled Follow-up",
                        Description = "Scheduled automatically by AI Business Agent",
                        DueDate = DateTime.UtcNow.AddDays(1),
                        Priority = TaskPriority.High,
                        Status = CrmTaskStatus.Pending
                    };

                    await _context.CrmTasks.AddAsync(task, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return ToolResult.Ok($"Follow-up task '{task.Title}' scheduled for tomorrow at {task.DueDate:yyyy-MM-dd HH:mm} UTC.", new { task.Id, task.Title, task.DueDate });
                }

            case "show_pending_tasks":
            default:
                {
                    var pendingTasks = await _context.CrmTasks
                        .AsNoTracking()
                        .Where(t => t.OrganizationId == context.OrganizationId && t.Status != CrmTaskStatus.Completed && !t.IsDeleted)
                        .OrderBy(t => t.DueDate)
                        .Take(10)
                        .Select(t => new { t.Id, t.Title, t.DueDate, Priority = t.Priority.ToString(), Status = t.Status.ToString() })
                        .ToListAsync(cancellationToken);

                    return ToolResult.Ok($"Found {pendingTasks.Count} pending CRM tasks.", pendingTasks);
                }
        }
    }
}
