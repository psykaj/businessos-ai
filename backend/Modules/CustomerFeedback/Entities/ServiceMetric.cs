using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public class ServiceMetric : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    public DateTime MetricDate { get; set; } = DateTime.UtcNow.Date;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AverageFirstResponseTimeMinutes { get; set; } = 0m;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AverageResolutionTimeMinutes { get; set; } = 0m;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal FirstContactResolutionRate { get; set; } = 0m; // percentage 0 to 100

    [Column(TypeName = "decimal(5, 2)")]
    public decimal CsatAverage { get; set; } = 0m;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal TicketReopenRate { get; set; } = 0m;

    public int OpenHighUrgencyCount { get; set; } = 0;

    public int TotalResolvedTickets { get; set; } = 0;

    public int TotalNewComplaints { get; set; } = 0;
}
