using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.CustomerFeedback.Entities;

public enum RatingEntityType
{
    Product,
    Service,
    SupportTicket,
    Agent,
    Company
}

public class Rating : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public RatingEntityType EntityType { get; set; } = RatingEntityType.Service;

    [Required]
    [MaxLength(100)]
    public string EntityId { get; set; } = string.Empty; // Guid or identifier of product/ticket/agent

    public Guid? CustomerId { get; set; }

    [MaxLength(150)]
    public string? CustomerEmail { get; set; }

    [Required]
    [Column(TypeName = "decimal(4, 2)")]
    public decimal RatingScore { get; set; } // e.g. 4.5 or 5.0

    public int MaxScore { get; set; } = 5; // Typically 5 or 10

    [MaxLength(2000)]
    public string? ReviewText { get; set; }

    public bool VerifiedPurchase { get; set; } = false;
}
