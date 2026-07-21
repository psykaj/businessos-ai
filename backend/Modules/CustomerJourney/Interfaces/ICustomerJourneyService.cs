using backend.Modules.CustomerJourney.DTOs;

namespace backend.Modules.CustomerJourney.Interfaces;

public interface ICustomerJourneyService
{
    Task<IReadOnlyList<CustomerJourneyDto>> GetJourneyHistoryAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default);
    Task<CustomerJourneyDto> TransitionStageAsync(Guid organizationId, Guid leadId, string newStage, CancellationToken cancellationToken = default);
}
