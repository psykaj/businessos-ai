namespace backend.Modules.CashFlow.DTOs;

public record CashFlowSummaryDto(
    decimal TotalCashIn,
    decimal TotalCashOut,
    decimal CurrentCashPosition,
    decimal NetCashFlow,
    decimal OutstandingReceivables,
    decimal OutstandingPayables,
    decimal EstimatedNetProfit
);

public record MonthlyCashFlowPointDto(
    string Month,
    decimal CashIn,
    decimal CashOut,
    decimal NetFlow
);

public record CashFlowForecastDto(
    decimal ProjectedCashInNext30Days,
    decimal ProjectedCashOutNext30Days,
    decimal ProjectedCashPositionIn30Days,
    List<MonthlyCashFlowPointDto> ForecastDetails
);

public record CreateCashFlowEntryDto(
    DateTime EntryDate,
    string Type, // CashIn, CashOut
    string Category,
    decimal Amount,
    Guid? AccountId,
    string? ReferenceType,
    Guid? ReferenceId,
    string? Description
);

public record CashFlowEntryDto(
    Guid Id,
    Guid OrganizationId,
    DateTime EntryDate,
    string Type,
    string Category,
    decimal Amount,
    Guid? AccountId,
    string? ReferenceType,
    Guid? ReferenceId,
    string? Description,
    DateTime CreatedAt
);
