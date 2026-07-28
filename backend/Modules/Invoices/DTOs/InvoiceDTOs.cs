namespace backend.Modules.Invoices.DTOs;

public record CreateInvoiceItemDto(
    Guid? ProductId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount = 0,
    decimal TaxRate = 0
);

public record CreateFinanceInvoiceDto(
    string InvoiceNumber,
    Guid? CustomerId,
    string CustomerName,
    string? CustomerEmail,
    DateTime IssueDate,
    DateTime DueDate,
    string Currency = "USD",
    string? Notes = null,
    string? Terms = null,
    List<CreateInvoiceItemDto>? Items = null
);

public record FinanceInvoiceItemDto(
    Guid Id,
    Guid? ProductId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal TaxRate,
    decimal TaxAmount,
    decimal TotalAmount
);

public record FinanceInvoiceDto(
    Guid Id,
    Guid OrganizationId,
    string InvoiceNumber,
    Guid? CustomerId,
    string CustomerName,
    string? CustomerEmail,
    DateTime IssueDate,
    DateTime DueDate,
    string Status,
    decimal SubTotal,
    decimal TaxTotal,
    decimal DiscountTotal,
    decimal TotalAmount,
    decimal AmountPaid,
    decimal BalanceDue,
    string Currency,
    string? Notes,
    string? Terms,
    string? PdfUrl,
    List<FinanceInvoiceItemDto> Items,
    DateTime CreatedAt
);

public record InvoiceSummaryDto(
    decimal TotalInvoiced,
    decimal TotalPaid,
    decimal TotalOverdue,
    int TotalCount,
    int PendingCount,
    int OverdueCount
);
