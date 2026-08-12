using backend.Modules.Outcomes.Entities;

namespace backend.Modules.Outcomes.Services;

public interface IOutcomeDetectionService
{
    // These methods would typically be called by event handlers
    // For example, when an invoice is paid or an automation completes
    
    Task<BusinessOutcome?> DetectInvoicePaymentOutcomeAsync(Guid businessId, Guid invoiceId, decimal amount, DateTime paymentDate, CancellationToken cancellationToken);
    
    Task<BusinessOutcome?> DetectAutomationOutcomeAsync(Guid businessId, Guid automationId, int successfulExecutions, decimal revenueRecovered, int timeSaved, CancellationToken cancellationToken);
    
    Task<BusinessOutcome?> DetectActionOutcomeAsync(Guid businessId, Guid actionId, OutcomeType expectedOutcome, decimal? impactValue, int? timeSaved, CancellationToken cancellationToken);
}
