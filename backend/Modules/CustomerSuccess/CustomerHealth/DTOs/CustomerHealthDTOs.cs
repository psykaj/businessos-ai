namespace backend.Modules.CustomerSuccess.CustomerHealth.DTOs;

public record CustomerHealthDto(
    Guid Id,
    Guid CustomerId,
    string? CustomerName,
    int HealthScore,
    string RiskLevel,
    DateTime? LastPurchaseDate,
    DateTime? LastInteractionDate,
    decimal LifetimeValue,
    decimal OutstandingPayments,
    int SupportTicketCount,
    double? SatisfactionRating,
    int ReferralCount,
    int PurchaseFrequency,
    DateTime CalculatedAt
);

public record UpdateCustomerHealthMetricsDto(
    Guid CustomerId,
    DateTime? LastPurchaseDate,
    DateTime? LastInteractionDate,
    decimal? AdditionalPurchaseValue,
    decimal? OutstandingPayments,
    int? SupportTicketDelta,
    double? LatestRating,
    int? ReferralDelta
);

public record CustomerHealthSummaryDto(
    int TotalCustomers,
    int HealthyCount,
    int StableCount,
    int NeedsAttentionCount,
    int HighRiskCount,
    double AverageHealthScore
);
