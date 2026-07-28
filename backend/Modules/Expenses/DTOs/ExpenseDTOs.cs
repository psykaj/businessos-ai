namespace backend.Modules.Expenses.DTOs;

public record CreateExpenseCategoryDto(
    string Name,
    string Code,
    string? Description,
    string Color = "#6B7280"
);

public record ExpenseCategoryDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Code,
    string? Description,
    string Color,
    bool IsActive
);

public record CreateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    decimal TaxAmount,
    decimal TaxRate,
    string Currency,
    Guid ExpenseCategoryId,
    DateTime ExpenseDate,
    string? VendorName,
    string PaymentMethod,
    string? ReceiptUrl,
    bool IsRecurring,
    string? RecurringInterval
);

public record UpdateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    decimal TaxAmount,
    decimal TaxRate,
    string Currency,
    Guid ExpenseCategoryId,
    DateTime ExpenseDate,
    string? VendorName,
    string PaymentMethod,
    string Status,
    string? ReceiptUrl,
    bool IsRecurring,
    string? RecurringInterval
);

public record ExpenseDto(
    Guid Id,
    Guid OrganizationId,
    string Title,
    string? Description,
    decimal Amount,
    decimal TaxAmount,
    decimal TaxRate,
    string Currency,
    Guid ExpenseCategoryId,
    string CategoryName,
    DateTime ExpenseDate,
    string? VendorName,
    string PaymentMethod,
    string Status,
    string? ReceiptUrl,
    bool IsRecurring,
    string? RecurringInterval,
    DateTime? NextRecurringDate,
    string? ApprovedBy,
    DateTime? ApprovedAt,
    DateTime CreatedAt
);
