using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Common;

namespace backend.Modules.GrowthRecommendations.Entities;

public class GrowthRecommendation : BaseEntity
{
    [Required]
    public Guid OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public backend.Entities.Organization? Organization { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string RecommendationType { get; set; } = string.Empty; // FocusHighMargin, IncreaseMarketingBudget, ReengageInactiveCustomers, UpsellPremiumPlans, ReduceLowPerformingInventory, ImproveConversionRates

    public Guid? TargetEntityId { get; set; }

    [MaxLength(100)]
    public string TargetEntityType { get; set; } = string.Empty; // Product, Customer, Campaign, Inventory

    [Required]
    [MaxLength(500)]
    public string BusinessImpact { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedFinancialImpact { get; set; }

    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = "High"; // Critical, High, Medium, Low

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, 100)]
    public decimal ConfidenceScore { get; set; } = 85;

    [Required]
    public string SuggestedAction { get; set; } = string.Empty;

    [MaxLength(255)]
    public string ActionUrl { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Status { get; set; } = "Open"; // Open, InProgress, Completed, Dismissed

    public DateTime? ActionedAt { get; set; }

    public Guid? ActionedByUserId { get; set; }

    public string CurrentState { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
}
