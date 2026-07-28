namespace backend.Modules.Taxes.DTOs;

public record CreateTaxRecordDto(
    string TaxName,
    string TaxCode,
    decimal Rate,
    string TaxType = "GST", // GST, VAT, SalesTax
    string CountryCode = "US",
    string? Description = null
);

public record TaxRecordDto(
    Guid Id,
    Guid OrganizationId,
    string TaxName,
    string TaxCode,
    decimal Rate,
    string TaxType,
    string CountryCode,
    bool IsActive,
    string? Description,
    DateTime CreatedAt
);

public record CalculateTaxRequestDto(
    decimal Amount,
    string TaxCode,
    bool IsInclusive = false
);

public record CalculateTaxResultDto(
    decimal BaseAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    decimal TaxRate,
    string TaxCode,
    string TaxName
);

public record TaxSummaryDto(
    decimal TotalTaxCollected,
    decimal TotalTaxPaid,
    decimal NetTaxLiability,
    List<TaxBreakdownDto> TaxBreakdowns
);

public record TaxBreakdownDto(
    string TaxCode,
    string TaxName,
    decimal Rate,
    decimal TaxCollected,
    decimal TaxPaid
);
