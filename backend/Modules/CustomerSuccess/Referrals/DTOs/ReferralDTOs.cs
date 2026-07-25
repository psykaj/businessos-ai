namespace backend.Modules.CustomerSuccess.Referrals.DTOs;

public record ReferralDto(
    Guid Id,
    Guid ReferrerCustomerId,
    string? ReferrerName,
    Guid? ReferredCustomerId,
    string? ReferredName,
    string ReferralCode,
    string Status,
    bool RewardIssued,
    decimal RewardAmount,
    string? Notes,
    DateTime CreatedAt
);

public record CreateReferralDto(
    Guid ReferrerCustomerId,
    decimal RewardAmount,
    string? Notes
);

public record ConvertReferralDto(
    string ReferralCode,
    Guid ReferredCustomerId
);

public record ReferralAnalyticsDto(
    int TotalReferrals,
    int PendingCount,
    int ConvertedCount,
    int RewardedCount,
    decimal TotalRewardsIssued,
    double ConversionRatePercentage
);
