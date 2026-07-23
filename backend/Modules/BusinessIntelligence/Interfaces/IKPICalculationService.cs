using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IKPICalculationService
{
    Task<RecalculateKPIsResponseDto> RecalculateAllKPIsAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<KPIDto>> GetKPIsAsync(Guid organizationId, string? category = null, CancellationToken cancellationToken = default);
    Task<KPIDto?> GetKPIByNameAsync(Guid organizationId, string name, CancellationToken cancellationToken = default);
}
