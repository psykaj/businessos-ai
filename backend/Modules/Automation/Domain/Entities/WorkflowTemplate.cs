using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;
using backend.Modules.Automation.Domain.Enums;

namespace backend.Modules.Automation.Domain.Entities;

public class WorkflowTemplate : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string TriggerType { get; set; } = WorkflowTriggerType.Manual.ToString();

    [Required]
    [Column(TypeName = "jsonb")]
    public string Configuration { get; set; } = "{}"; // JSON string representing the steps and config

    public bool IsActive { get; set; } = true;
}
