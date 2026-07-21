using backend.Modules.CustomerJourney.Interfaces;
using backend.Persistence;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.CustomerJourney.Repositories;

public class JourneyRepository : GenericRepository<Entities.CustomerJourney>, IJourneyRepository
{
    private readonly ApplicationDbContext _context;

    public JourneyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Entities.CustomerJourney?> GetLatestJourneyForLeadAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerJourneys
            .Where(cj => cj.OrganizationId == organizationId && cj.LeadId == leadId && !cj.IsDeleted)
            .OrderByDescending(cj => cj.EnteredAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Entities.CustomerJourney>> GetJourneyHistoryForLeadAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerJourneys
            .Where(cj => cj.OrganizationId == organizationId && cj.LeadId == leadId && !cj.IsDeleted)
            .OrderByDescending(cj => cj.EnteredAt)
            .ToListAsync(cancellationToken);
    }
}
