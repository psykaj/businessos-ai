using System.ComponentModel.DataAnnotations;

namespace backend.Modules.Forecasting.DTOs;

public class GenerateForecastDto
{
    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty;

    public int ForecastMonths { get; set; } = 3;
}
