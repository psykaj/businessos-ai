namespace backend.Modules.AccountsReceivable.DTOs;

public record AccountsReceivableDto(
    Guid Id,
    Guid OrganizationId,
    Guid InvoiceId,
    Guid? CustomerId,
    string CustomerName,
    decimal TotalAmount,
    decimal AmountPaid,
    decimal BalanceDue,
    DateTime DueDate,
    string Status,
    int DaysOverdue,
    DateTime? LastReminderSentAt,
    DateTime CreatedAt
);

public record AccountsReceivableAgingDto(
    decimal TotalOutstanding,
    decimal Current,
    decimal Days1To30,
    decimal Days31To60,
    decimal Days61To90,
    decimal Days90Plus,
    int TotalOverdueInvoices
);

public record SendReminderRequestDto(
    string? CustomNote
);
