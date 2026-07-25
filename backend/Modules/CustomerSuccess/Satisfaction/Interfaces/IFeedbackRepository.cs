using backend.Modules.CustomerSuccess.Satisfaction.Entities;

namespace backend.Modules.CustomerSuccess.Satisfaction.Interfaces;

public interface IFeedbackRepository
{
    Task<CustomerFeedback?> GetByIdAsync(Guid orgId, Guid id);
    Task<IEnumerable<CustomerFeedback>> GetByCustomerIdAsync(Guid orgId, Guid customerId);
    Task<(IEnumerable<CustomerFeedback> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, int? minRating, int? maxRating, string? feedbackType, int page, int pageSize);
    Task<double> GetAverageRatingAsync(Guid orgId, string? feedbackType);
    Task<Dictionary<int, int>> GetRatingDistributionAsync(Guid orgId, string? feedbackType);
    Task AddAsync(CustomerFeedback feedback);
    Task SaveChangesAsync();
}
