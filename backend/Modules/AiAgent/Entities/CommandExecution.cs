using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.AiAgent.Entities;

public class CommandExecution : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Command { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ToolInvoked { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Running, Success, Failed, RequiresConfirmation

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? FinishedAt { get; set; }

    public string? ResultSummary { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ParametersJson { get; set; }
}
