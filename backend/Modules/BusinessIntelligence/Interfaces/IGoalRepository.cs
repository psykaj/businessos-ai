using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IGoalRepository
{
    Task<Goal?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Goal>> GetAllByOrganizationIdAsync(Guid organizationId, string? status = null, CancellationToken cancellationToken = default);
    Task<(List<Goal> Items, int TotalCount)> GetPagedAsync(Guid organizationId, int page, int pageSize, string? search = null, string? status = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    Task AddAsync(Goal goal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Goal goal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Goal goal, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
