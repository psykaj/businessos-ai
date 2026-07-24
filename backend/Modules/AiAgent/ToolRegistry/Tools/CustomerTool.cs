using backend.Modules.AiAgent.ToolRegistry.Interfaces;
using backend.Modules.AiAgent.ToolRegistry.Models;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.ToolRegistry.Tools;

public class CustomerTool : ITool
{
    private readonly ApplicationDbContext _context;

    public CustomerTool(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name => "CustomerTool";
    public string Description => "Customer Insights Tool: search customers and identify inactive leads/customers requiring attention.";
    public string Category => "CRM";
    public string[] RequiredPermissions => new[] { "CRM.View" };
    public bool IsDestructive => false;
    public string ParametersSchema => "{\"type\":\"object\",\"properties\":{\"action\":{\"type\":\"string\",\"enum\":[\"search_customer\",\"show_inactive_customers\"]},\"searchTerm\":{\"type\":\"string\"}}}";

    public async Task<ToolResult> ExecuteAsync(
        ToolExecutionContext context,
        IDictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        var action = parameters.TryGetValue("action", out var actObj) ? actObj?.ToString()?.ToLower() : "search_customer";
        var searchTerm = parameters.TryGetValue("searchTerm", out var stObj) ? stObj?.ToString() : null;

        if (action == "show_inactive_customers")
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var inactiveLeads = await _context.Leads
                .AsNoTracking()
                .Where(l => l.OrganizationId == context.OrganizationId && !l.IsDeleted && l.UpdatedAt < thirtyDaysAgo)
                .OrderBy(l => l.UpdatedAt)
                .Take(10)
                .Select(l => new { l.Id, Name = $"{l.FirstName} {l.LastName}", l.Email, l.Status, l.UpdatedAt })
                .ToListAsync(cancellationToken);

            return ToolResult.Ok($"Found {inactiveLeads.Count} inactive customers/leads without activity in the last 30 days.", inactiveLeads);
        }

        var query = _context.Leads.AsNoTracking().Where(l => l.OrganizationId == context.OrganizationId && !l.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var pattern = $"%{searchTerm.ToLower()}%";
            query = query.Where(l => EF.Functions.ILike(l.FirstName, pattern) || EF.Functions.ILike(l.LastName, pattern) || EF.Functions.ILike(l.Email, pattern));
        }

        var results = await query.Take(10).Select(l => new { l.Id, Name = $"{l.FirstName} {l.LastName}", l.Email, l.Phone, l.Status }).ToListAsync(cancellationToken);

        return ToolResult.Ok($"Found {results.Count} matching contacts.", results);
    }
}
