using System;
using System.ComponentModel.DataAnnotations;
using backend.Common;

namespace backend.Modules.CommunicationHub.Entities;

public class CommunicationMetric : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public DateTime MetricDate { get; set; } // Daily aggregation date

    public CommunicationChannelType? ChannelType { get; set; } // Null represents consolidated aggregate across all channels

    public int TotalConversations { get; set; } = 0;

    public int ActiveConversations { get; set; } = 0;

    public int ResolvedConversations { get; set; } = 0;

    public decimal AverageResponseTimeMinutes { get; set; } = 0;

    public decimal AverageResolutionTimeMinutes { get; set; } = 0;

    public int TotalInboundMessages { get; set; } = 0;

    public int TotalOutboundMessages { get; set; } = 0;

    public decimal AverageCsatScore { get; set; } = 0;

    public int CsatResponseCount { get; set; } = 0;
}
