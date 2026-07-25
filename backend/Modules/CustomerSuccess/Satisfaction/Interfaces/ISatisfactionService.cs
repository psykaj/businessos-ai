using backend.Modules.CustomerSuccess.Satisfaction.DTOs;

namespace backend.Modules.CustomerSuccess.Satisfaction.Interfaces;

public interface ISatisfactionService
{
    Task<CustomerFeedbackDto> SubmitFeedbackAsync(Guid orgId, SubmitFeedbackDto dto);
    Task<CustomerFeedbackDto> GetByIdAsync(Guid orgId, Guid id);
    Task<(IEnumerable<CustomerFeedbackDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, int? minRating, int? maxRating, string? feedbackType, int page, int pageSize);
    Task<SatisfactionSummaryDto> GetSummaryAsync(Guid orgId, string? feedbackType);
    Task<IEnumerable<CustomerFeedbackDto>> GetByCustomerAsync(Guid orgId, Guid customerId);
}
