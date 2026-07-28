namespace backend.Modules.Accounting.DTOs;

public record CreateAccountDto(
    string AccountCode,
    string AccountName,
    string AccountType, // Asset, Liability, Equity, Revenue, Expense
    string? SubCategory,
    string Currency = "USD",
    decimal InitialBalance = 0,
    string? Description = null
);

public record UpdateAccountDto(
    string AccountName,
    string AccountType,
    string? SubCategory,
    string Currency,
    bool IsActive,
    string? Description
);

public record AccountDto(
    Guid Id,
    Guid OrganizationId,
    string AccountCode,
    string AccountName,
    string AccountType,
    string? SubCategory,
    string Currency,
    decimal CurrentBalance,
    bool IsActive,
    string? Description,
    DateTime CreatedAt
);
