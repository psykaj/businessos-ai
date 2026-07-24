namespace backend.Modules.AiAgent.Context.Models;

public class AiContext
{
    public Guid OrganizationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<string> UserRoles { get; set; } = new();
    public List<string> UserPermissions { get; set; } = new();

    public decimal RevenueToday { get; set; }
    public int ActiveLeadsCount { get; set; }
    public int PendingTasksCount { get; set; }
    public int UnpaidInvoicesCount { get; set; }
    public string SystemSummary { get; set; } = string.Empty;
}
