using backend.Modules.Taxes.DTOs;

namespace backend.Modules.Taxes.Interfaces;

public interface ITaxEngine
{
    Task<IEnumerable<TaxRecordDto>> GetTaxesAsync(Guid organizationId);
    Task<TaxRecordDto> CreateTaxAsync(Guid organizationId, CreateTaxRecordDto dto);
    Task<CalculateTaxResultDto> CalculateTaxAsync(Guid organizationId, CalculateTaxRequestDto request);
    Task<TaxSummaryDto> GetTaxSummaryAsync(Guid organizationId, DateTime? startDate = null, DateTime? endDate = null);
    Task SeedDefaultTaxesAsync(Guid organizationId, string countryCode = "US");
}
