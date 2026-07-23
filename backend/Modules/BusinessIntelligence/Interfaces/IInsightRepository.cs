using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IInsightRepository
{
    Task<Insight?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Insight>> GetAllByOrganizationIdAsync(Guid organizationId, string? category = null, CancellationToken cancellationToken = default);
    Task<(List<Insight> Items, int TotalCount)> GetPagedAsync(Guid organizationId, int page, int pageSize, string? search = null, string? priority = null, string? category = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Insight> insights, CancellationToken cancellationToken = default);
    Task ClearAndAddAsync(Guid organizationId, IEnumerable<Insight> newInsights, CancellationToken cancellationToken = default);
    Task DeleteAsync(Insight insight, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
