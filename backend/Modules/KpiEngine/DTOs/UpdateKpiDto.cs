using System.ComponentModel.DataAnnotations;

namespace backend.Modules.KpiEngine.DTOs;

public class UpdateKpiDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public decimal? TargetValue { get; set; }

    [MaxLength(20)]
    public string? Frequency { get; set; }

    public bool? IsHigherBetter { get; set; }
}
