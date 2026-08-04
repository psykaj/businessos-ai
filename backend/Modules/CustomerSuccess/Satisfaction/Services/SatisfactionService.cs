using backend.Modules.CustomerSuccess.CustomerHealth.DTOs;
using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using backend.Modules.CustomerSuccess.Satisfaction.DTOs;
using backend.Modules.CustomerSuccess.Satisfaction.Entities;
using CustomerFeedback = backend.Modules.CustomerSuccess.Satisfaction.Entities.CustomerFeedback;
using backend.Modules.CustomerSuccess.Satisfaction.Interfaces;
using backend.Modules.CustomerSuccess.SuccessTasks.Entities;
using backend.Modules.CustomerSuccess.SuccessTasks.Interfaces;

namespace backend.Modules.CustomerSuccess.Satisfaction.Services;

public class SatisfactionService : ISatisfactionService
{
    private readonly IFeedbackRepository _feedbackRepo;
    private readonly ICustomerHealthService _healthService;
    private readonly ISuccessTaskRepository _taskRepo;

    public SatisfactionService(
        IFeedbackRepository feedbackRepo,
        ICustomerHealthService healthService,
        ISuccessTaskRepository taskRepo)
    {
        _feedbackRepo = feedbackRepo;
        _healthService = healthService;
        _taskRepo = taskRepo;
    }

    public async Task<CustomerFeedbackDto> SubmitFeedbackAsync(Guid orgId, SubmitFeedbackDto dto)
    {
        var feedback = new Entities.CustomerFeedback
        {
            OrganizationId = orgId,
            CustomerId = dto.CustomerId,
            Rating = Math.Clamp(dto.Rating, 1, 5),
            Feedback = dto.Feedback,
            Channel = dto.Channel ?? "Web",
            FeedbackType = dto.FeedbackType ?? "CSAT",
            SubmittedAt = DateTime.UtcNow
        };

        await _feedbackRepo.AddAsync(feedback);
        await _feedbackRepo.SaveChangesAsync();

        // Update Customer Health Rating
        await _healthService.UpdateMetricsAsync(orgId, new UpdateCustomerHealthMetricsDto(
            dto.CustomerId,
            null,
            DateTime.UtcNow,
            null,
            null,
            null,
            feedback.Rating,
            null
        ));

        // Negative Feedback Alert: Auto create high priority SuccessTask for ratings <= 2
        if (feedback.Rating <= 2)
        {
            await _taskRepo.AddAsync(new SuccessTask
            {
                OrganizationId = orgId,
                CustomerId = dto.CustomerId,
                TaskType = "ContactDissatisfied",
                Title = $"Negative CSAT Alert ({feedback.Rating}/5 Stars)",
                Description = $"Customer left negative feedback: \"{feedback.Feedback ?? "No comment"}\". Urgent follow up required.",
                Priority = "High",
                Status = "Pending",
                DueDate = DateTime.UtcNow.AddDays(1)
            });
            await _taskRepo.SaveChangesAsync();
        }

        return MapToDto(feedback);
    }

    public async Task<CustomerFeedbackDto> GetByIdAsync(Guid orgId, Guid id)
    {
        var feedback = await _feedbackRepo.GetByIdAsync(orgId, id)
            ?? throw new KeyNotFoundException("Feedback record not found.");

        return MapToDto(feedback);
    }

    public async Task<(IEnumerable<CustomerFeedbackDto> Items, int TotalCount)> GetPagedAsync(
        Guid orgId, Guid? customerId, int? minRating, int? maxRating, string? feedbackType, int page, int pageSize)
    {
        var (items, totalCount) = await _feedbackRepo.GetPagedAsync(orgId, customerId, minRating, maxRating, feedbackType, page, pageSize);
        return (items.Select(MapToDto), totalCount);
    }

    public async Task<SatisfactionSummaryDto> GetSummaryAsync(Guid orgId, string? feedbackType)
    {
        double avgRating = await _feedbackRepo.GetAverageRatingAsync(orgId, feedbackType);
        var dist = await _feedbackRepo.GetRatingDistributionAsync(orgId, feedbackType);

        int total = dist.Values.Sum();
        int positive = (dist.GetValueOrDefault(4) + dist.GetValueOrDefault(5));
        int negative = (dist.GetValueOrDefault(1) + dist.GetValueOrDefault(2));

        double posPct = total > 0 ? (positive / (double)total) * 100.0 : 0.0;
        double negPct = total > 0 ? (negative / (double)total) * 100.0 : 0.0;

        return new SatisfactionSummaryDto(
            Math.Round(avgRating, 2),
            total,
            dist,
            Math.Round(posPct, 1),
            Math.Round(negPct, 1)
        );
    }

    public async Task<IEnumerable<CustomerFeedbackDto>> GetByCustomerAsync(Guid orgId, Guid customerId)
    {
        var items = await _feedbackRepo.GetByCustomerIdAsync(orgId, customerId);
        return items.Select(MapToDto);
    }

    private static CustomerFeedbackDto MapToDto(Entities.CustomerFeedback f)
    {
        return new CustomerFeedbackDto(
            f.Id,
            f.CustomerId,
            f.Customer?.Name ?? "Customer",
            f.Rating,
            f.Feedback,
            f.Channel,
            f.FeedbackType,
            f.SubmittedAt
        );
    }
}
