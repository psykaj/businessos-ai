using backend.Entities;
using backend.Interfaces;

namespace backend.Modules.Campaigns.Interfaces;

public interface ICampaignRepository : IGenericRepository<Campaign>
{
    Task<(IReadOnlyList<Campaign> Items, int TotalCount)> GetCampaignsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default);
}
