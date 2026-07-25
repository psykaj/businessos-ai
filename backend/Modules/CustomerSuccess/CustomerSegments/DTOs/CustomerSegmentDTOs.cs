namespace backend.Modules.CustomerSuccess.CustomerSegments.DTOs;

public record CustomerSegmentDto(
    Guid Id,
    string Name,
    string SegmentType,
    string? CriteriaJson,
    int CustomerCount,
    DateTime UpdatedAt
);

public record CustomerSegmentMemberDto(
    Guid CustomerId,
    string CustomerName,
    string Email,
    string SegmentName,
    decimal LifetimeValue,
    int HealthScore,
    string RiskLevel
);
