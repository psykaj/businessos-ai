namespace backend.Modules.AccountsPayable.DTOs;

public record CreateAccountsPayableDto(
    string BillNumber,
    Guid? SupplierId,
    string SupplierName,
    Guid? PurchaseOrderId,
    decimal TotalAmount,
    DateTime DueDate
);

public record AccountsPayableDto(
    Guid Id,
    Guid OrganizationId,
    string BillNumber,
    Guid? SupplierId,
    string SupplierName,
    Guid? PurchaseOrderId,
    decimal TotalAmount,
    decimal AmountPaid,
    decimal BalanceDue,
    DateTime DueDate,
    string Status,
    int DaysOverdue,
    DateTime CreatedAt
);

public record AccountsPayableAgingDto(
    decimal TotalOutstanding,
    decimal Current,
    decimal Days1To30,
    decimal Days31To60,
    decimal Days61To90,
    decimal Days90Plus,
    int TotalOverdueBills
);
