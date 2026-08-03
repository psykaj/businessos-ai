using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum ChurnRiskLevel
{
    Low,
    Medium,
    High,
    Critical
}

public class CustomerSatisfactionScore : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [MaxLength(150)]
    public string? CustomerName { get; set; }

    [MaxLength(150)]
    public string? CustomerEmail { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal CurrentCsat { get; set; } = 0m; // percentage 0.0 to 100.0

    [Column(TypeName = "decimal(5, 2)")]
    public decimal CurrentNps { get; set; } = 0m; // -100.0 to 100.0

    [Column(TypeName = "decimal(4, 2)")]
    public decimal AverageRating { get; set; } = 0m; // 1.0 to 5.0

    public int TotalFeedbacks { get; set; } = 0;

    public int TotalComplaints { get; set; } = 0;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal RepeatComplaintRate { get; set; } = 0m; // percentage of recurring unresolved complaints

    [Column(TypeName = "decimal(5, 2)")]
    public decimal ResolutionSatisfaction { get; set; } = 0m; // percentage satisfied after resolution

    [Required]
    public ChurnRiskLevel ChurnRiskScore { get; set; } = ChurnRiskLevel.Low;

    public DateTime LastCalculatedAt { get; set; } = DateTime.UtcNow;
}
