using System;

namespace backend.Modules.CustomerFeedback.ServiceQuality.DTOs;

public class ServiceMetricDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public DateTime MetricDate { get; set; }
    public decimal AverageFirstResponseTimeMinutes { get; set; }
    public decimal AverageResolutionTimeMinutes { get; set; }
    public decimal FirstContactResolutionRate { get; set; }
    public decimal CsatAverage { get; set; }
    public decimal TicketReopenRate { get; set; }
    public int OpenHighUrgencyCount { get; set; }
    public int TotalResolvedTickets { get; set; }
    public int TotalNewComplaints { get; set; }
}

public class RecordServiceMetricRequestDto
{
    public Guid OrganizationId { get; set; }
    public DateTime? MetricDate { get; set; }
    public decimal AverageFirstResponseTimeMinutes { get; set; }
    public decimal AverageResolutionTimeMinutes { get; set; }
    public decimal FirstContactResolutionRate { get; set; }
    public int TotalResolvedTickets { get; set; }
}
