using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Entities;
using backend.Modules.CRM.Enums;

namespace backend.Modules.CRM.Entities;

public class Deal : BaseEntity
{
    public Guid OrganizationId { get; set; }
    public backend.Entities.Organization? Organization { get; set; }

    public Guid? LeadId { get; set; }
    public Lead? Lead { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public string Title { get; set; } = string.Empty;

    public PipelineStage PipelineStage { get; set; } = PipelineStage.NewLead;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal Probability { get; set; } // Percentage 0-100

    public DateTime? ExpectedCloseDate { get; set; }

    public string Status { get; set; } = string.Empty; // Open, Won, Lost

    public Guid? OwnerId { get; set; }
    public User? Owner { get; set; }
}
