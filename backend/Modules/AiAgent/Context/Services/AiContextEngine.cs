using backend.Modules.AiAgent.Context.Interfaces;
using backend.Modules.AiAgent.Context.Models;
using backend.Modules.CRM.Enums;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.AiAgent.Context.Services;

public class AiContextEngine : IAiContextEngine
{
    private readonly ApplicationDbContext _context;

    public AiContextEngine(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AiContext> BuildContextAsync(Guid organizationId, string userId, CancellationToken cancellationToken = default)
    {
        Guid.TryParse(userId, out var userGuid);

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userGuid, cancellationToken);

        var roles = await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userGuid)
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToListAsync(cancellationToken);

        var permissions = new List<string>
        {
            "CRM.View", "CRM.Edit",
            "Billing.View", "Billing.Create",
            "QRCode.Create",
            "Email.Send",
            "WhatsApp.Send",
            "BI.View", "BI.Create",
            "Workflow.Execute",
            "Analytics.View"
        };

        var paidRevenue = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.OrganizationId == organizationId && i.Status == "Paid" && !i.IsDeleted)
            .SumAsync(i => (decimal?)i.Amount, cancellationToken) ?? 0m;

        var activeLeads = await _context.Leads
            .AsNoTracking()
            .CountAsync(l => l.OrganizationId == organizationId && !l.IsDeleted, cancellationToken);

        var pendingTasks = await _context.CrmTasks
            .AsNoTracking()
            .CountAsync(t => t.OrganizationId == organizationId && t.Status != CrmTaskStatus.Completed && !t.IsDeleted, cancellationToken);

        var unpaidInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(i => i.OrganizationId == organizationId && i.Status != "Paid" && !i.IsDeleted, cancellationToken);

        return new AiContext
        {
            OrganizationId = organizationId,
            UserId = userId,
            UserName = user != null ? user.FullName : "System User",
            UserEmail = user?.Email ?? "user@businessos.ai",
            UserRoles = roles,
            UserPermissions = permissions,
            RevenueToday = paidRevenue,
            ActiveLeadsCount = activeLeads,
            PendingTasksCount = pendingTasks,
            UnpaidInvoicesCount = unpaidInvoices,
            SystemSummary = $"Org Context: {activeLeads} active leads, ${paidRevenue:F2} revenue, {pendingTasks} pending tasks, {unpaidInvoices} unpaid invoices."
        };
    }
}
