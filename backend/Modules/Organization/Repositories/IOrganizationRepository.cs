using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Organization.Repositories;

public interface IOrganizationRepository : IGenericRepository<Entities.Organization>
{
    Task<Entities.Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Entities.Organization> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);
}
