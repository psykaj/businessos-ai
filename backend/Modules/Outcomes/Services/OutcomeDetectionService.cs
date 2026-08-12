using Microsoft.EntityFrameworkCore;
using backend.Persistence;
using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Services;

public class OutcomeDetectionService : IOutcomeDetectionService
{
    private readonly ApplicationDbContext _context;
    private readonly IOutcomeAttributionService _attributionService;

    public OutcomeDetectionService(ApplicationDbContext context, IOutcomeAttributionService attributionService)
    {
        _context = context;
        _attributionService = attributionService;
    }

    public async Task<BusinessOutcome?> DetectInvoicePaymentOutcomeAsync(Guid businessId, Guid invoiceId, decimal amount, DateTime paymentDate, CancellationToken cancellationToken)
    {
        // For example, detect if this invoice was recently targeted by an Action or Automation
        // Look up recent actions for this invoice (using ActionCenter or AiActions)
        // Since we don't have the exact AI Action Entity imported here easily, we'll simulate the attribution logic.
        
        var confidence = await _attributionService.CalculateConfidenceAsync(businessId, "Invoice", invoiceId.ToString(), paymentDate, cancellationToken);

        if (confidence == OutcomeConfidence.Unknown)
        {
            // If we have absolutely no evidence that AI/Automation did this, we don't claim it.
            return null;
        }

        var outcome = new BusinessOutcome
        {
            BusinessId = businessId,
            OutcomeType = OutcomeType.RevenueRecovered,
            SourceType = OutcomeSourceType.Automation, // Assuming it was an automation reminder
            SourceId = invoiceId.ToString(), // Generic placeholder since we don't have the exact automation ID here
            RelatedEntityType = "Invoice",
            RelatedEntityId = invoiceId.ToString(),
            RevenueImpact = amount,
            Currency = "INR", // Could be dynamic
            Confidence = confidence,
            AttributionLevel = await _attributionService.DetermineAttributionLevelAsync(confidence, cancellationToken),
            MeasurementMethod = "Invoice payment after action",
            OccurredAt = paymentDate,
            RecordedAt = DateTime.UtcNow,
            Status = OutcomeStatus.Active,
            Explanation = $"₹{amount} recovered from invoice payment following automated reminder."
        };

        _context.BusinessOutcomes.Add(outcome);
        await _context.SaveChangesAsync(cancellationToken);
        return outcome;
    }

    public async Task<BusinessOutcome?> DetectAutomationOutcomeAsync(Guid businessId, Guid automationId, int successfulExecutions, decimal revenueRecovered, int timeSaved, CancellationToken cancellationToken)
    {
        if (successfulExecutions == 0 && revenueRecovered == 0 && timeSaved == 0) return null;

        var outcome = new BusinessOutcome
        {
            BusinessId = businessId,
            OutcomeType = revenueRecovered > 0 ? OutcomeType.RevenueRecovered : OutcomeType.AutomationCompleted,
            SourceType = OutcomeSourceType.Automation,
            SourceId = automationId.ToString(),
            RelatedEntityType = "AutomationWorkflow",
            RelatedEntityId = automationId.ToString(),
            RevenueImpact = revenueRecovered > 0 ? revenueRecovered : null,
            TimeSavedMinutes = timeSaved > 0 ? timeSaved : null,
            Confidence = OutcomeConfidence.High, // Direct system evidence
            AttributionLevel = AttributionLevel.Direct,
            MeasurementMethod = "Direct workflow execution analytics",
            OccurredAt = DateTime.UtcNow,
            RecordedAt = DateTime.UtcNow,
            Status = OutcomeStatus.Active,
            Explanation = $"Automation generated {(revenueRecovered > 0 ? $"₹{revenueRecovered} in revenue " : "")}{(timeSaved > 0 ? $"and saved {timeSaved} minutes" : "")}."
        };

        _context.BusinessOutcomes.Add(outcome);
        await _context.SaveChangesAsync(cancellationToken);
        return outcome;
    }

    public async Task<BusinessOutcome?> DetectActionOutcomeAsync(Guid businessId, Guid actionId, OutcomeType expectedOutcome, decimal? impactValue, int? timeSaved, CancellationToken cancellationToken)
    {
        var outcome = new BusinessOutcome
        {
            BusinessId = businessId,
            OutcomeType = expectedOutcome,
            SourceType = OutcomeSourceType.ActionCenter,
            SourceId = actionId.ToString(),
            RelatedEntityType = "AiAction",
            RelatedEntityId = actionId.ToString(),
            ChangeValue = impactValue,
            RevenueImpact = expectedOutcome == OutcomeType.RevenueIncrease || expectedOutcome == OutcomeType.RevenueRecovered ? impactValue : null,
            CostImpact = expectedOutcome == OutcomeType.CostReduction ? impactValue : null,
            TimeSavedMinutes = timeSaved,
            Confidence = OutcomeConfidence.High, // Since action completed
            AttributionLevel = AttributionLevel.Direct,
            MeasurementMethod = "Action execution result",
            OccurredAt = DateTime.UtcNow,
            RecordedAt = DateTime.UtcNow,
            Status = OutcomeStatus.Active,
            Explanation = "User completed recommended action resulting in immediate measurable impact."
        };

        _context.BusinessOutcomes.Add(outcome);
        await _context.SaveChangesAsync(cancellationToken);
        return outcome;
    }
}
