using FluentValidation;
using backend.Modules.CustomerFeedback.FeedbackAnalytics.DTOs;

namespace backend.Modules.CustomerFeedback.FeedbackAnalytics.Validators;

public class FeedbackAnalyticsFilterValidator : AbstractValidator<FeedbackAnalyticsFilterDto>
{
    public FeedbackAnalyticsFilterValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty().WithMessage("OrganizationId is required.");
        RuleFor(x => x.Months).InclusiveBetween(1, 36).WithMessage("Months range must be between 1 and 36.");
    }
}
