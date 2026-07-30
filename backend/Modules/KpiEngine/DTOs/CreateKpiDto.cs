using System.ComponentModel.DataAnnotations;

namespace backend.Modules.KpiEngine.DTOs;

public class CreateKpiDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public decimal TargetValue { get; set; }

    [MaxLength(20)]
    public string Frequency { get; set; } = "Monthly";

    public bool IsHigherBetter { get; set; } = true;
}
