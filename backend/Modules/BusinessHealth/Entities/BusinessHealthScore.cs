using System.ComponentModel.DataAnnotations;
using backend.Common;
using backend.Entities;

namespace backend.Modules.BusinessHealth.Entities;

public class BusinessHealthScore : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public backend.Entities.Organization? Organization { get; set; }

    [Range(0, 100)]
    public decimal OverallScore { get; set; }

    [Range(0, 100)]
    public decimal FinancialHealth { get; set; }

    [Range(0, 100)]
    public decimal OperationalHealth { get; set; }

    [Range(0, 100)]
    public decimal CustomerHealth { get; set; }

    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Healthy"; // Excellent, Healthy, Warning, Critical
}
