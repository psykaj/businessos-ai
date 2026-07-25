namespace backend.Modules.CustomerSuccess.Loyalty.DTOs;

public record LoyaltyProgramDto(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    int PointsPerPurchase,
    int MinimumRedemptionPoints,
    int? PointsExpiryDays,
    bool IsDefault,
    DateTime CreatedAt
);

public record CreateLoyaltyProgramDto(
    string Name,
    string? Description,
    int PointsPerPurchase,
    int MinimumRedemptionPoints,
    int? PointsExpiryDays,
    bool IsDefault
);

public record UpdateLoyaltyProgramDto(
    string Name,
    string? Description,
    string Status,
    int PointsPerPurchase,
    int MinimumRedemptionPoints,
    int? PointsExpiryDays,
    bool IsDefault
);

public record LoyaltyTransactionDto(
    Guid Id,
    Guid CustomerId,
    string? CustomerName,
    Guid ProgramId,
    string? ProgramName,
    int PointsEarned,
    int PointsRedeemed,
    int Balance,
    string TransactionType,
    string? Description,
    DateTime? ExpiryDate,
    DateTime CreatedAt
);

public record EarnPointsDto(
    Guid CustomerId,
    Guid? ProgramId,
    decimal PurchaseAmount,
    string? Description
);

public record RedeemPointsDto(
    Guid CustomerId,
    Guid? ProgramId,
    int PointsToRedeem,
    string? Description
);

public record AdjustPointsDto(
    Guid CustomerId,
    Guid? ProgramId,
    int PointsDelta,
    string Reason
);

public record CustomerLoyaltySummaryDto(
    Guid CustomerId,
    int TotalBalance,
    int TotalEarned,
    int TotalRedeemed,
    IEnumerable<LoyaltyTransactionDto> RecentTransactions
);
