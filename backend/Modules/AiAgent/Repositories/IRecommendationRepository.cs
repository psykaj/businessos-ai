using backend.Modules.AiAgent.Entities;

namespace backend.Modules.AiAgent.Repositories;

public interface IRecommendationRepository
{
    Task<Recommendation?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Recommendation>> GetAllByOrganizationIdAsync(Guid organizationId, bool includeApplied = false, CancellationToken cancellationToken = default);
    Task<(List<Recommendation> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        int page = 1,
        int pageSize = 20,
        string? category = null,
        string? priority = null,
        bool? isApplied = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(Recommendation recommendation, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Recommendation> recommendations, CancellationToken cancellationToken = default);
    void Update(Recommendation recommendation);
    void Delete(Recommendation recommendation);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
