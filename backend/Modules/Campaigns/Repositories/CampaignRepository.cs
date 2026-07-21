using backend.Entities;
using backend.Modules.Campaigns.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.Campaigns.Repositories;

public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
{
    private readonly ApplicationDbContext _context;

    public CampaignRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Campaign> Items, int TotalCount)> GetCampaignsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Campaigns.Where(c => c.OrganizationId == organizationId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(c => c.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
