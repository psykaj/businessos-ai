namespace backend.Modules.Payments.DTOs;

public record CreateFinancePaymentDto(
    string PaymentNumber,
    Guid? InvoiceId,
    Guid? BillId,
    Guid? AccountId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod = "BankTransfer",
    string PaymentType = "Incoming", // Incoming, Outgoing
    string? ReferenceNumber = null,
    string? Notes = null
);

public record FinancePaymentDto(
    Guid Id,
    Guid OrganizationId,
    string PaymentNumber,
    Guid? InvoiceId,
    Guid? BillId,
    Guid? AccountId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod,
    string PaymentType,
    string? ReferenceNumber,
    string? Notes,
    string Status,
    DateTime CreatedAt
);
