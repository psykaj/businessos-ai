using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.AiAgent.Entities;

public class ToolDefinition : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = "General";

    [Required]
    public string RequiredPermissions { get; set; } = string.Empty; // Comma-separated list of permissions

    public bool Enabled { get; set; } = true;

    public bool IsDestructive { get; set; } = false;

    public string? ParametersSchema { get; set; }
}
