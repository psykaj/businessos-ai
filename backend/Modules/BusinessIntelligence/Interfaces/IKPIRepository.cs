using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IKPIRepository
{
    Task<KPI?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<KPI?> GetByNameAsync(string name, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<KPI>> GetAllByOrganizationIdAsync(Guid organizationId, string? category = null, CancellationToken cancellationToken = default);
    Task<(List<KPI> Items, int TotalCount)> GetPagedAsync(Guid organizationId, int page, int pageSize, string? search = null, string? category = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    Task UpsertKPIAsync(KPI kpi, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
