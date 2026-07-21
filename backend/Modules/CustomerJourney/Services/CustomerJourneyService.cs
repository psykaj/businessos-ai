using backend.Interfaces;
using backend.Modules.CustomerJourney.DTOs;
using backend.Modules.CustomerJourney.Interfaces;
using backend.Modules.Webhooks.Interfaces;

namespace backend.Modules.CustomerJourney.Services;

public class CustomerJourneyService : ICustomerJourneyService
{
    private readonly IJourneyRepository _journeyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebhookDispatchService _webhookDispatchService;

    public CustomerJourneyService(
        IJourneyRepository journeyRepository,
        IUnitOfWork unitOfWork,
        IWebhookDispatchService webhookDispatchService)
    {
        _journeyRepository = journeyRepository;
        _unitOfWork = unitOfWork;
        _webhookDispatchService = webhookDispatchService;
    }

    public async Task<IReadOnlyList<CustomerJourneyDto>> GetJourneyHistoryAsync(Guid organizationId, Guid leadId, CancellationToken cancellationToken = default)
    {
        var history = await _journeyRepository.GetJourneyHistoryForLeadAsync(organizationId, leadId, cancellationToken);
        return history.Select(h => new CustomerJourneyDto
        {
            Id = h.Id,
            LeadId = h.LeadId,
            CurrentStage = h.CurrentStage,
            PreviousStage = h.PreviousStage,
            EnteredAt = h.EnteredAt
        }).ToList();
    }

    public async Task<CustomerJourneyDto> TransitionStageAsync(Guid organizationId, Guid leadId, string newStage, CancellationToken cancellationToken = default)
    {
        var latest = await _journeyRepository.GetLatestJourneyForLeadAsync(organizationId, leadId, cancellationToken);
        var previousStage = latest?.CurrentStage ?? "Unknown";

        if (previousStage == newStage)
        {
            // Already in this stage
            return new CustomerJourneyDto
            {
                Id = latest!.Id,
                LeadId = latest.LeadId,
                CurrentStage = latest.CurrentStage,
                PreviousStage = latest.PreviousStage,
                EnteredAt = latest.EnteredAt
            };
        }

        var newJourney = new Entities.CustomerJourney
        {
            OrganizationId = organizationId,
            LeadId = leadId,
            CurrentStage = newStage,
            PreviousStage = previousStage,
            EnteredAt = DateTime.UtcNow
        };

        await _journeyRepository.AddAsync(newJourney, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Fire webhook
        _ = _webhookDispatchService.EnqueueEventAsync(organizationId, "CustomerStageChanged", new
        {
            LeadId = leadId,
            PreviousStage = previousStage,
            NewStage = newStage,
            TransitionedAt = newJourney.EnteredAt
        }, CancellationToken.None);

        return new CustomerJourneyDto
        {
            Id = newJourney.Id,
            LeadId = newJourney.LeadId,
            CurrentStage = newJourney.CurrentStage,
            PreviousStage = newJourney.PreviousStage,
            EnteredAt = newJourney.EnteredAt
        };
    }
}
