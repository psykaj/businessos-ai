using backend.Interfaces;
using backend.Modules.CustomerJourney.Entities;

namespace backend.Modules.CustomerJourney.Interfaces;

public interface IJourneyRepository : IGenericRepository<Entities.CustomerJourney>
{
    Task<Entities.CustomerJourney?> GetLatestJourneyForLeadAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.CustomerJourney>> GetJourneyHistoryForLeadAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default);
}
