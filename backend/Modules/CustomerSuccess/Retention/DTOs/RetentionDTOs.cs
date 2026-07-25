namespace backend.Modules.CustomerSuccess.Retention.DTOs;

public record RetentionOverviewDto(
    double RetentionRatePercentage,
    double ChurnRatePercentage,
    int AtRiskCount,
    int TotalActiveCustomers,
    decimal RevenueAtRisk,
    IEnumerable<RetentionActionItemDto> RecommendedActions
);

public record RetentionActionItemDto(
    string ActionType,
    string Description,
    string RecommendedPriority,
    int AffectedCustomerCount
);
