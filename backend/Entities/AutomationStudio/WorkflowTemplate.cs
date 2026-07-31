using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Entities;

public class WorkflowTemplate : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty; // e.g., Sales, Marketing, HR

    [Column(TypeName = "jsonb")]
    public string Configuration { get; set; } = "{}"; // Entire workflow structure (triggers, conditions, actions) as JSON

    public bool IsSystemTemplate { get; set; } = true; // True if provided by BusinessOS

    public Guid? OrganizationId { get; set; } // Null if it's a global template, otherwise it's a custom template for a specific tenant
}
