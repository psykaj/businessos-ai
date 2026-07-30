using System;

namespace backend.Modules.Benchmarks.DTOs;

public class BenchmarkDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal IndustryAverage { get; set; }
    public decimal TopQuartile { get; set; }
    public string Industry { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string Source { get; set; } = string.Empty;
}
