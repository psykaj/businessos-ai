using backend.Modules.BusinessIntelligence.Entities;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<Report>> GetAllByOrganizationIdAsync(Guid organizationId, string? reportType = null, CancellationToken cancellationToken = default);
    Task<(List<Report> Items, int TotalCount)> GetPagedAsync(Guid organizationId, int page, int pageSize, string? search = null, string? reportType = null, string? sortBy = null, bool sortDescending = false, CancellationToken cancellationToken = default);
    Task AddAsync(Report report, CancellationToken cancellationToken = default);
    Task DeleteAsync(Report report, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
