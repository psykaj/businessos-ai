using System;
using System.Collections.Generic;

namespace backend.Modules.Benchmarking.DTOs;

public class BenchmarkMetricDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string ComparisonType { get; set; } = string.Empty; // SaaS, Ecommerce, ProfessionalServices
    public string MetricName { get; set; } = string.Empty;
    public decimal OrganizationValue { get; set; }
    public decimal BenchmarkMedian { get; set; }
    public decimal BenchmarkTopQuartile { get; set; }
    public decimal PercentileRank { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class CreateBenchmarkMetricRequest
{
    public string ComparisonType { get; set; } = "SaaS";
    public string MetricName { get; set; } = string.Empty;
    public decimal OrganizationValue { get; set; }
    public decimal BenchmarkMedian { get; set; }
    public decimal BenchmarkTopQuartile { get; set; }
}

public class BenchmarkComparisonSummaryDto
{
    public Guid OrganizationId { get; set; }
    public string SelectedIndustry { get; set; } = "Enterprise SaaS & AI Software";
    public int TotalMetricsCompared { get; set; }
    public int MetricsInTopQuartile { get; set; }
    public int MetricsAboveMedian { get; set; }
    public List<BenchmarkMetricDto> Comparisons { get; set; } = new();
}
