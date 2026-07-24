using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Modules.CRM.Enums;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class AnalyticsTool : ITool
{
    private readonly ApplicationDbContext _context;

    public AnalyticsTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "AnalyticsTool";
    public string Description => "Business Analytics Tool: show today's revenue, dashboard metrics, and business overview.";
    public string Category => "Analytics";
    public string[] RequiredPermissions => new[] { "Analytics.View" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"action\":{\"type\":\"string\",\"enum\":[\"show_revenue\",\"show_dashboard\"]}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var action = parameters.TryGetValue("action", out var actObj) ? actObj?.ToString()?.ToLower() : "show_dashboard";

        var totalPaidInvoices = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == context.OrganizationId && i.Status == "Paid" && !i.IsDeleted)
            .SumAsync(i => (decimal?)i.Amount, cancellationToken) ?? 0m;

        var activeLeadsCount = await _context.Leads
            .AsNoTracking()
            .CountAsync(l => l.OrganizationId == context.OrganizationId && !l.IsDeleted, cancellationToken);

        var pendingTasksCount = await _context.CrmTasks
            .AsNoTracking()
            .CountAsync(t => t.OrganizationId == context.OrganizationId && t.Status != CrmTaskStatus.Completed && !t.IsDeleted, cancellationToken);

        var unpaidInvoicesCount = await _context.Invoices
            .AsNoTracking()
            .CountAsync(i => i.OrganizationId == context.OrganizationId && i.Status != "Paid" && !i.IsDeleted, cancellationToken);

        if (action == "show_revenue")
        {
            return ToolResult.Ok($"Total Revenue recorded to date is ${totalPaidInvoices:F2}.", new
            {
                TotalRevenue = totalPaidInvoices,
                Currency = "USD",
                UnpaidInvoicesCount = unpaidInvoicesCount
            });
        }

        return ToolResult.Ok($"Executive Dashboard Summary loaded for organization.", new
        {
            Revenue = totalPaidInvoices,
            ActiveLeads = activeLeadsCount,
            PendingTasks = pendingTasksCount,
            UnpaidInvoices = unpaidInvoicesCount,
            HealthStatus = "Optimal"
        });
    }
}
