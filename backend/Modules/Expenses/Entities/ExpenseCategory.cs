using backend.Common;

namespace backend.Modules.Expenses.Entities;

public class ExpenseCategory : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#6B7280";
    public bool IsActive { get; set; } = true;
}
