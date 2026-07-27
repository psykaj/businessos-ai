namespace backend.Modules.Inventory.DTOs;

public record CreateSupplierDto(
    string Name,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string? Address,
    string? City,
    string? Country,
    string? TaxId,
    string PaymentTerms = "Net 30"
);

public record UpdateSupplierDto(
    string Name,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string? Address,
    string? City,
    string? Country,
    string? TaxId,
    string PaymentTerms,
    bool IsActive
);

public record SupplierResponseDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string? Address,
    string? City,
    string? Country,
    string? TaxId,
    string PaymentTerms,
    double PerformanceScore,
    int TotalOrdersCount,
    int OnTimeDeliveriesCount,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record SupplierPerformanceDto(
    Guid SupplierId,
    string SupplierName,
    double PerformanceScore,
    int TotalOrdersCount,
    int OnTimeDeliveriesCount,
    double OnTimeDeliveryRate,
    double FulfillmentRate
);
